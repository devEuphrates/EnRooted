using UnityEngine;

public class RootHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private GameObject _brenchPrefab;

    [Space]
    [SerializeField] Transform _rootHolder;

    [Space]
    [Header("Physics Settings")]
    [SerializeField] private float _rayDistance = 2000f;
    [SerializeField] private LayerMask _nodeLayer;

    [Header("Line Renderer Settings")]
    private Vector3 _initialPosition;

    [SerializeField] private WaterManager _waterManager;

    [SerializeField] private float _initialSpawnOffset;
    [SerializeField] private float _minNodeSpawnCancelValue;

    [SerializeField] private FloatSO _waterAmount;

    Branch _selectedBranch;
    Node _selectedNode;

    private bool _enabled = false;

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && TryFindingPoint(mousePos))
        {
            _enabled = true;
            _selectedBranch = _selectedNode.OwnerBranch;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _enabled = false;
            return;
        }

        if (!_enabled || !Input.GetMouseButton(0))
            return;

        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.transform.position.z * -1f));

        if (Vector3.Distance(mouseWorldPosition, _initialPosition) < _initialSpawnOffset)
            return;

        if (TryFindingPoint(mousePos))
            return;

        if (_selectedNode.ChildCount == 0)
        {
            _selectedBranch = _selectedNode.OwnerBranch;
            CreateSegment(mouseWorldPosition);
            _initialPosition = mouseWorldPosition;
            return;
        }

        CreateBranch(_selectedNode);
        CreateSegment(mouseWorldPosition);
    }

    RaycastHit[] hits = new RaycastHit[5];
    private bool TryFindingPoint(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        int cnt = Physics.RaycastNonAlloc(ray, hits, _rayDistance, _nodeLayer);
        if (cnt > 0)
        {
            if (!hits[cnt - 1].transform.TryGetComponent<Node>(out var node))
                return false;

            _initialPosition = node.transform.position;
            _selectedNode = node;
            return true;
        }

        return false;
    }

    private bool TryFindingPoinInArea(Vector3 pos)
    {
        int cnt = Physics.SphereCastNonAlloc(pos, _minNodeSpawnCancelValue, Vector3.zero, hits, _rayDistance, _nodeLayer);
        return cnt > 0 && hits[cnt - 1].transform.GetComponent<Node>() != _selectedNode;
    }

    private void CreateSegment(Vector3 position)
    {
        _initialPosition = _selectedNode.transform.position;
        int cnt = (int)((position - _initialPosition).magnitude / _initialSpawnOffset);
        Vector3 dir = (position - _initialPosition).normalized;

        for (int i = 1; i < cnt + 1; i++)
        {
            Vector3 pos = _initialPosition + dir * i * _initialSpawnOffset;

            if (TryFindingPoinInArea(pos))
            {
                _enabled = false;
                return;
            }

            var newNode = Instantiate(_nodePrefab, pos, Quaternion.identity).GetComponent<Node>();
            newNode.Parent = _selectedNode;
            _selectedBranch.AddNode(newNode);
            newNode.OwnerBranch = _selectedBranch;
            _selectedNode = newNode;
        }
    }

    private void CreateBranch(Node rootNode)
    {
        var branch = Instantiate(_brenchPrefab, Vector3.zero, Quaternion.identity, _rootHolder).GetComponent<Branch>();
        _selectedBranch = branch;
        branch.AddNode(rootNode);
    }
}
