using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierRoute : MonoBehaviour
{
    [SerializeField] private Transform[] _controlPoints;
    private Vector3[] _spawnPoints = new Vector3[20];
    private Vector3 _gizmosPos;
    [SerializeField] private GameObject _gameObject ;
    public Transform[] ControlPoints
    {
        get => _controlPoints;
    }
    public Vector3[] SpawnPoints
    {
        get => _spawnPoints;
    }
    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += .05f)
        {
            _gizmosPos = Mathf.Pow((1 - t), 5) * _controlPoints[0].position + 5 * t * Mathf.Pow(1 - t, 4) * _controlPoints[1].position
                + 10 * t * t * Mathf.Pow(1 - t, 3) * _controlPoints[2].position + 10 * Mathf.Pow(t, 3) * Mathf.Pow(1 - t, 2) * _controlPoints[3].position +
                5 * Mathf.Pow(t, 4) * (1 - t) * _controlPoints[4].position + Mathf.Pow(t, 5) * _controlPoints[5].position;
            Gizmos.DrawSphere(_gizmosPos, .25f);
        }
        Gizmos.DrawLine(_controlPoints[0].position, _controlPoints[1].position);
        Gizmos.DrawLine(_controlPoints[2].position, _controlPoints[3].position);
        Gizmos.DrawLine(_controlPoints[4].position, _controlPoints[5].position);
    }

    public void GetPoints()
    {
        int i = 0;
        for (float t = 0; t <= 1; t += .05f)
        {   

            _gizmosPos = Mathf.Pow((1 - t), 5) * _controlPoints[0].position + 5 * t * Mathf.Pow(1 - t, 4) * _controlPoints[1].position
                + 10 * t * t * Mathf.Pow(1 - t, 3) * _controlPoints[2].position + 10 * Mathf.Pow(t, 3) * Mathf.Pow(1 - t, 2) * _controlPoints[3].position +
                5 * Mathf.Pow(t, 4) * (1 - t) * _controlPoints[4].position + Mathf.Pow(t, 5) * _controlPoints[5].position;
            _spawnPoints[i] = _gizmosPos;
            i++;
        }
    }
}
