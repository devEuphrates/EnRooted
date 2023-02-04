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

        if (_selectedNode.Child.Count == 0)
        {
            _selectedBranch = _selectedNode.OwnerBranch;
            CreatePoint(mouseWorldPosition);
            _initialPosition = mouseWorldPosition;
            return;
        }

        CreateBranch(_selectedNode);
        CreatePoint(mouseWorldPosition);
    }

    private bool TryFindingPoint(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _nodeLayer))
        {
            if (!hit.transform.TryGetComponent<Node>(out var node))
                return false;

            _initialPosition = node.transform.position;
            _selectedNode = node;
            return true;
        }

        return false;
    }

    private void CreatePoint(Vector3 position)
    {
        var newNode = Instantiate(_nodePrefab, position, Quaternion.identity).GetComponent<Node>();
        newNode.Parent = _selectedNode;
        _selectedNode = newNode;
        _selectedBranch.AddNode(newNode);
        newNode.OwnerBranch = _selectedBranch;
        newNode.Init(_waterAmount, _waterManager);
    }

    private void CreateBranch(Node rootNode)
    {
        var branch = Instantiate(_brenchPrefab, Vector3.zero, Quaternion.identity, _rootHolder).GetComponent<Branch>();
        _selectedBranch = branch;
        branch.AddNode(rootNode);
    }
}
