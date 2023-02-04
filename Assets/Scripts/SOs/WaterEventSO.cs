using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO Channels/Water Events")]
public class WaterEventSO : ScriptableObject
{
    public event Action<Node> OnCreateWater;
    public void CreateWaterLine(Node node) => OnCreateWater?.Invoke(node);

    public event Action<Node> OnDestroyWater;
    public void DestroyWaterLine(Node node) => OnDestroyWater?.Invoke(node);
}
