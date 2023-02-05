using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Branch OwnerBranch;
    public Node Parent;
    
    List<Node> _children = new List<Node>();
    
    public float DistanceToParent = .3f;
    private WaterSource _waterSource;

    private float _timer;

    [SerializeField] FloatSO _waterAmount;
    [SerializeField] private float _water_increase_amount;
    [SerializeField] private float _time_to_call_water_source;
    [SerializeField] WaterEventSO _waterEvents;


    public void AddChild(Node child)
    {
        _children.Add(child);
        _sucking = false;
        _waterEvents.DestroyWaterLine(this);
    }

    public void RemoveChild(Node node) => _children.Remove(node);

    public int ChildCount => _children.Count;

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

    bool _sucking = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water Source") && _children.Count == 0)
        {
            _sucking = true;
            var water_source = other.attachedRigidbody.GetComponent<WaterSource>();
            _waterSource = water_source;
            _waterEvents.CreateWaterLine(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_sucking && other.gameObject.CompareTag("Water Source"))
        {
            _timer += Time.deltaTime;
            if (_timer >= _time_to_call_water_source)
            {
                _timer = 0f;
                if (_waterSource != null) _waterSource.ModifyVolume();
                _waterAmount.Value += _water_increase_amount;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_sucking && other.gameObject.CompareTag("Water Source"))
        {
            _waterEvents.DestroyWaterLine(this);
        }
    }
}