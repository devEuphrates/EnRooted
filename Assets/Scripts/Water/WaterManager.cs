using System.Collections;
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

    void CreateWaterLine(Node node)
    {
        if (UpdateWaterLine(node))
            return;

        LineRenderer line = Instantiate(_water_line_prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<LineRenderer>();
        _lines[node] = line;
        //SpawnAsync(node);


        List<Node> path = node.PathToRoot;
        Vector3[] pos = new Vector3[path.Count];

        line.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
            pos[i] = path[i].transform.position;

        line.SetPositions(pos);
    }

    void DestroyWaterLine(Node node)
    {
        if (!_lines.ContainsKey(node))
            return;

        //DespawnAsync(node);

        Destroy(_lines[node]);
        _lines.Remove(node);
    }

    bool UpdateWaterLine(Node node)
    {
        foreach (var item in _lines)
        {
            if (item.Key.PathToRoot.Contains(node))
                return true;
        }

        List<Node> path = node.PathToRoot;
        LineRenderer line = null;
        Node selected = null;

        for (int i = 0; i < path.Count; i++)
        {
            if (_lines.ContainsKey(path[i]))
            {
                selected = path[i];
                line = _lines[selected];

                if (path[i].ChildCount > 1)
                    return false;

                break;
            }
        }

        if (line == null)
            return false;

        Vector3[] pos = new Vector3[path.Count];
        for (int i = 0; i < pos.Length; i++)
            pos[i] = path[i].transform.position;

        line.positionCount = pos.Length;
        line.SetPositions(pos);

        _lines.Remove(selected);
        _lines.Add(node, line);

        return true;
    }

    //async void SpawnAsync(Node node)
    //{
    //    LineRenderer line = _lines[node];
    //    List<Node> path = node.PathToRoot;
    //    line.positionCount = 0;

    //    for (int i = 0; i < path.Count; i++)
    //    {
    //        line.positionCount++;

    //        line.SetPosition(i, path[i].transform.position);
    //        await Task.Delay((int)(_animDelay * 100));
    //    }
    //}

    //async void DespawnAsync(Node node)
    //{
    //    LineRenderer line = _lines[node];

    //    while (line.positionCount > 0)
    //    {
    //        line.positionCount--;
    //        await Task.Delay((int)(_animDelay * 100));
    //    }

    //    Destroy(_lines[node].gameObject);
    //    _lines.Remove(node);
    //}
}
