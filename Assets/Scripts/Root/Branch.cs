using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Branch : MonoBehaviour
{
    [SerializeField] List<Node> _nodes = new List<Node>();
    [SerializeField] LineRenderer _lineRenderer;

    Mesh _genMesh;

    MeshFilter _meshFilter;

    private void Awake()
    {
        _genMesh = new Mesh();
    }

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        SetLineRenderer();
        //SetMesh();
    }

    public void AddNode(Node node, int atIndex = -1)
    {
        if (atIndex <= 0)
            _nodes.Add(node);

        _nodes.Insert(atIndex, node);
        SetLineRenderer();
    }

    void SetLineRenderer()
    {
        Vector3[] positions = new Vector3[_nodes.Count];
        for (int i = 0; i < _nodes.Count; i++)
            positions[i] = _nodes[i].transform.position;

        _lineRenderer.positionCount = positions.Length;
        _lineRenderer.SetPositions(positions);
    }

    void SetMesh()
    {
        _lineRenderer.BakeMesh(_genMesh);
        _lineRenderer.positionCount = 0;
        _meshFilter.mesh = _genMesh;
    }
}
