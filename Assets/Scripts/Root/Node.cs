using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    public Branch OwnerBranch;
    public Node Parent;
    public List<Node> Child = new List<Node>();
    public float DistanceToParent = .3f;
    private WaterManager _waterManager;
    private WaterSource _waterSource;

    private float _timer;
    private FloatSO _waterAmount;
    [SerializeField] private GameObject _waterLinePrefab;
    private LineRenderer _waterLine;
    [SerializeField] private float _water_increase_amount;
    [SerializeField] private float _time_to_call_water_source;
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

    private void HighlightWaterLine()
    {
        Vector3[] positions = new Vector3[PathToRoot.Count];
        for (int i = 0; i < PathToRoot.Count; i++)
            positions[i] = PathToRoot[i].transform.position;


    }

    public void Init(FloatSO waterAmount, WaterManager manager = null)
    {
        if (manager != null)
        {
            _waterManager = manager;
        }
        _waterAmount = waterAmount;
        _waterAmount.OnValueChange += OnPlantWaterChanged;
        _waterLine = _waterLinePrefab.GetComponent<LineRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water Source"))
        {

            var water_source = other.gameObject.GetComponent<WaterSource>();
            _waterSource = water_source;
            if (_waterManager != null) _waterManager.HighlightWaterRenderer(this);
        }
    }
    private void OnPlantWaterChanged()
    {
        return;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Water Source"))
        {
            _timer += Time.deltaTime;
            if (_timer >= _time_to_call_water_source)
            {
                Debug.Log("w");
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
            var water_source = other.gameObject.GetComponent<WaterSource>();
            _waterSource = null;
        }
    }

}
