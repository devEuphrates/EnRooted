using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBranch : MonoBehaviour
{
    private TreeBranch _rootTR;
    private List<TreeBranch> _childTR;
    private BezierRoute _bezierRoute;
    private LineRenderer _branch_line_renderer;
    [SerializeField] private GameObject branch_line;
    public BezierRoute BezierRoute
    {
        set => _bezierRoute = value;
        get => _bezierRoute;
    }
    public GameObject Branch_Line_Prefab
    {
        set => branch_line = value;
    }
    
    public TreeBranch RootBranch
    {
        get => _rootTR;
        set => _rootTR = value;
    }
    public List<TreeBranch> ChildBranch
    {
        get => _childTR;
        set => _childTR = value;
    }
    // Update is called once per frame
    public void CreateBranch()
    {
        var branch = Instantiate(branch_line);
        _branch_line_renderer = branch.GetComponent<LineRenderer>();
        _bezierRoute.GetPoints();
        Vector3[] positions = new Vector3[_bezierRoute.SpawnPoints.Length];
        for (int i = 0; i < _bezierRoute.SpawnPoints.Length; i++)
            positions[i] = _bezierRoute.SpawnPoints[i];

        _branch_line_renderer.positionCount = positions.Length;
        _branch_line_renderer.SetPositions(positions);
        //_lineRenderer.Simplify(.1f);
    }
}
