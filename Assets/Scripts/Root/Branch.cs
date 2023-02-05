using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Branch : MonoBehaviour
{
    [SerializeField] List<Node> _nodes = new List<Node>();
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _nodeHolder;

    Mesh _genMesh;

    MeshFilter _meshFilter;

    private void Awake()
    {
        _genMesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        SetLineRenderer();
    }

    public void AddNode(Node node, int atIndex = -1)
    {
        node.transform.parent = _nodeHolder;

        if (atIndex <= 0)
        {
            if (_nodes.Count > 0)
            {
                Node lastNode = _nodes[_nodes.Count - 1];

                lastNode.AddChild(node);
                node.Parent = lastNode;
            }

            _nodes.Add(node);
            SetLineRenderer();
            return;
        }

        _nodes[atIndex].Parent = node;
        _nodes[atIndex - 1].RemoveChild(_nodes[atIndex]);
        _nodes[atIndex - 1].AddChild(node);

        _nodes.Insert(atIndex, node);
        SetLineRenderer();
    }

    public Node GetNode(int index) => index == -1 ? _nodes[_nodes.Count - 1] : _nodes[index];

    void SetLineRenderer()
    {
        Vector3[] positions = new Vector3[_nodes.Count];
        for (int i = 0; i < _nodes.Count; i++)
            positions[i] = _nodes[i].transform.position;

        _lineRenderer.positionCount = positions.Length;
        _lineRenderer.SetPositions(positions);
        //_lineRenderer.Simplify(.1f);
        SetMesh();
    }

    void SetMesh()
    {
        _lineRenderer.BakeMesh(_genMesh);

        if (!_genMesh)
            return;

        _lineRenderer.positionCount = 0;
        _meshFilter.mesh = _genMesh;
    }
}
