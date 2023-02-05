using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCreator : MonoBehaviour
{
    

    [SerializeField] private TreeBranch _treeBranch;

    public void AddBranch(Transform tr_point, BezierRoute route)
    {
        var tree_branch = Object.Instantiate(_treeBranch, tr_point.position, tr_point.rotation, tr_point);
        var b_route = Object.Instantiate(route, Vector3.zero, Quaternion.identity);
        b_route.transform.position = tree_branch.transform.position;
        tree_branch.BezierRoute = b_route;
        tree_branch.CreateBranch();
        return;
    }
    public void AddBranch(TreeBranch tree_branch, int spawnCount, BezierRoute[] routes)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var random_route_Number = UnityEngine.Random.Range(0, routes.Length);
            var random_point_Number = UnityEngine.Random.Range(0, tree_branch.BezierRoute.SpawnPoints.Length);
            var treeBranch = Object.Instantiate(_treeBranch,tree_branch.transform);
            var b_route = Object.Instantiate(routes[random_route_Number], Vector3.zero, Quaternion.identity);
            treeBranch.BezierRoute = b_route;
            tree_branch.transform.position = tree_branch.BezierRoute.SpawnPoints[random_point_Number];
            treeBranch.CreateBranch();
            tree_branch.ChildBranch.Add(treeBranch);

        }
    }
    public void DeleteBranch(Transform tr_point)
    {

    }
    public void DeleteBranch(TreeBranch tree_branch)
    {
        var parent_branch = _treeBranch.RootBranch;
        parent_branch.ChildBranch.Remove(tree_branch);
        foreach( var _child_branch in tree_branch.ChildBranch)
        {
            DeleteBranch(_child_branch);
            Destroy(_child_branch);
        }

    }

}
