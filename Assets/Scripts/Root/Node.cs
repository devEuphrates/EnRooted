using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node Parent;
    public List<Node> Child = new List<Node>();
    public float DistanceToParent = 0;
    public float DistanceAbove
    {
        get
        {
            if (!Parent)
                return 0;

            return DistanceToParent + Parent.DistanceAbove;
        }
    }
    public List<Node> PathToRoot
    {
        get
        {
            List<Node> path = new List<Node>() { this };
            if (!Parent)
                return path;

            Parent.AppendToPath(path);
            return path;
        }
    }

    public void AppendToPath(List<Node> path)
    {
        path.Add(this);

        if (Parent)
            Parent.AppendToPath(path);
    }
}
