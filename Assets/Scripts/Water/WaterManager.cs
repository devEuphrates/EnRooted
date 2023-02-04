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

    void DestroyWaterLine(Node node) => DespawnAsync(node);

    async void SpawnAsync(Node node)
    {
        LineRenderer line = _lines[node];
        List<Node> path = node.PathToRoot;
        line.positionCount = 0;

        for (int i = 0; i < path.Count; i++)
        {
            line.positionCount++;
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
