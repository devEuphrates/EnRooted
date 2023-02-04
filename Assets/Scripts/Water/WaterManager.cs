using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] private GameObject _water_line_prefab;
    [SerializeField] float _animDelay = .1f;

    public void  HighlightWaterRenderer(Node node)
    {
        LineRenderer line = Instantiate(_water_line_prefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        SpawnAsync(node, line);
    }

    async void SpawnAsync(Node node, LineRenderer line)
    {
        List<Node> path = node.PathToRoot;
        line.positionCount = 0;

        for (int i = 0; i < path.Count; i++)
        {
            line.positionCount++;
            line.SetPosition(i, path[i].transform.position);
            await Task.Delay((int)(_animDelay * 100));
        }
    }
}
