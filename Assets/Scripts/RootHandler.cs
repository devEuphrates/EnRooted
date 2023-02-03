using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;


    [Header("Line Settings")]
    [SerializeField] private GameObject _linePrefab;
    private LineRenderer _lineRenderer;

    [Header("Node Settings")]
    [SerializeField] private List<GameObject> _nodeList = new List<GameObject>();
    private int _activeNodeIndex = -1;
    [SerializeField] private GameObject _nodePrefab;


    [Header("Segment Settings")]
    private GameObject _segmentPrefab;
    [SerializeField] private List<GameObject> _segmentList = new List<GameObject>();

    [Header("Physics Settings")]
    [SerializeField] private float _rayDistance = 2000f;
    [SerializeField] private LayerMask _nodeLayer;

    [Header("Line Renderer Settings")]
    private Vector3 _initialPosition;
    [SerializeField] private float _initialSpawnOffset;
    [SerializeField] private float _minNodeSpawnValue;
    [SerializeField] private float _maxNodeSpawnValue;
    [SerializeField] private float _maxNodeSpawnCancelValue;
    [SerializeField] private float _minNodeSpawnCancelValue;

    private bool _enabled = false;
    private bool _canMove = false;
    private void Awake()
    {
        _lineRenderer = _linePrefab.GetComponent<LineRenderer>();
        _activeNodeIndex = _nodeList.Count-1;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryFindingPoint(Input.mousePosition);
            _enabled = true;
        }
        if (!_enabled)
            return;
        if (Input.GetMouseButton(0))
        {
            if (_activeNodeIndex != -1)
            {
                Vector2 _startPos = _nodeList[_activeNodeIndex - 1].transform.position;
                Vector2 _currentPos = _camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, _camera.transform.position.z * (-1)));
                Vector3 diff = _currentPos - _startPos;
                var offset = Vector3.Dot(_startPos.normalized, _currentPos.normalized);
                if (diff.magnitude >= _initialSpawnOffset && !_canMove)
                {
                    CreatePoint(_activeNodeIndex);
                    _nodeList[_activeNodeIndex].transform.position = _currentPos;
                    _canMove = true;
                }
                if (offset <= _minNodeSpawnValue)
                {
                    CreatePoint(_activeNodeIndex);
                    _initialPosition = _nodeList[_activeNodeIndex].transform.position;
                }
                else if (offset >= _minNodeSpawnCancelValue || offset <= _maxNodeSpawnCancelValue)
                {
                    _enabled = false;
                }
                if (_canMove)
                {
                    _nodeList[_activeNodeIndex].transform.position = _currentPos;

                }
            }

        }
    }

    private void TryFindingPoint(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _nodeLayer))
        {
            Debug.Log("hit");
            var node = hit.collider.gameObject;
            _initialPosition = node.transform.position;
            _nodeList.Add(node);

        }

    }

    private void CreatePoint(int parent_index)
    {
        var new_node = Instantiate(_nodePrefab, _nodeList[parent_index].transform.position, Quaternion.identity);
        _nodeList.Add(new_node);
        //_nodeList[parent_index].child = new_node;
        //new_node.parent = _nodeList[parent_index];
        _activeNodeIndex = _nodeList.IndexOf(new_node);
        //CreateSegment(_nodeList[parent_index],new_node);

    }

    private void CreateSegment(GameObject parent_node, GameObject child_node)
    {
        var segmentObj = Instantiate(_segmentPrefab);
    }
    private void UpdateSegment(GameObject parent_node, GameObject child_node)
    {

    }
    private void DivideSegment(GameObject parent_node, GameObject child_node)
    {

    }




}
