using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] private GameObject _water_line_prefab;
    [SerializeField] float _animDelay = .1f;
    [SerializeField] WaterEventSO _waterEvents;

    Dictionary<Node, LineRenderer> _lines = new Dictionary<Node, LineRenderer>();

    private void OnEnable()
    {
        _waterEvents.OnCreateWater += CreateWaterLine;
        _waterEvents.OnDestroyWater += DestroyWaterLine;
    }

    private void OnDisable()
    {
        _waterEvents.OnCreateWater -= CreateWaterLine;
        _waterEvents.OnDestroyWater -= DestroyWaterLine;
    }

    void  CreateWaterLine(Node node)
    {
        LineRenderer line = Instantiate(_water_line_prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<LineRenderer>();
        _lines[node] = line;
        SpawnAsync(node);
    }

    void DestroyWaterLine(Node node)
    {
        if (!_lines.ContainsKey(node))
            return;

        DespawnAsync(node);
    }

    async void SpawnAsync(Node node)
    {
        LineRenderer line = _lines[node];
        List<Node> path = node.PathToRoot;
        line.positionCount = 0;

        if (path.Count > 1)
        {
            line.positionCount = 2;
            line.SetPosition(0, path[0].transform.position);
            line.SetPosition(1, path[2].transform.position);
        }

        for (int i = 2; i < path.Count; i++)
        {
            line.positionCount++;

            if (line.positionCount <= i || i < 0)
                return;

            line.SetPosition(i, path[i].transform.position);
            await Task.Delay((int)(_animDelay * 100));
        }
    }

    async void DespawnAsync(Node node)
    {
        LineRenderer line = _lines[node];

        while (line.positionCount > 0)
        {
            line.positionCount--;
            await Task.Delay((int)(_animDelay * 100));
        }

        Destroy(_lines[node].gameObject);
        _lines.Remove(node);
    }
}
