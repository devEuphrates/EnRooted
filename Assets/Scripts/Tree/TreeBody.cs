using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBody : MonoBehaviour
{
    [SerializeField] private List<Transform> _branch_tr_points;

    public List<Transform> GetInitialPoints()
    {
        return _branch_tr_points;
    }
}
