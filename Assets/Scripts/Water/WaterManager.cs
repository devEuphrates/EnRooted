using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] private GameObject _water_line_prefab;
    private LineRenderer _active_water_line_renderer;

    

    // Update is called once per frame
    public void  HighlightWaterRenderer(Node node)
    {
        _active_water_line_renderer = Instantiate(_water_line_prefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        Vector3[] positions = new Vector3[node.PathToRoot.Count];
        for (int i = 0; i < node.PathToRoot.Count; i++)
        {
            positions[i] = node.PathToRoot[i].transform.position;
        }
        _active_water_line_renderer.positionCount = positions.Length;
        _active_water_line_renderer.SetPositions(positions);
    }
}
