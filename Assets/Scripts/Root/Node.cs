using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Branch OwnerBranch;
    public Node Parent;
    public List<Node> Child = new List<Node>();
    public float DistanceToParent = .3f;
    private WaterSource _waterSource;

    private float _timer;

    [SerializeField] FloatSO _waterAmount;
    [SerializeField] private float _water_increase_amount;
    [SerializeField] private float _time_to_call_water_source;
    [SerializeField] WaterEventSO _waterEvents;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water Source"))
        {

            var water_source = other.gameObject.GetComponent<WaterSource>();
            _waterSource = water_source;
            _waterEvents.CreateWaterLine(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Water Source"))
        {
            _timer += Time.deltaTime;
            if (_timer >= _time_to_call_water_source)
            {
                _timer = 0f;
                if (_waterSource != null) _waterSource.ModifyValue();
                _waterAmount.Value += _water_increase_amount;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water Source"))
        {
            _waterEvents.DestroyWaterLine(this);
        }
    }
}