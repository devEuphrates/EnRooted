using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TreeData SO", menuName = "SO Variables/TreeData")]
public class TreeData : ScriptableObject
{
    [SerializeField] private List<LevelData> _levelDataLists;
    [SerializeField] private int _currentLevel;
    public int CurrentLevel
    {
        get => _currentLevel;
        set => _currentLevel = value;
    }
    public List<LevelData> LevelDataList
    {
        get => _levelDataLists;
    }

    [System.Serializable]
    public struct LevelData
    {
        public int spawn_count;
        public List<Branch> Spawned_Branch_List;
        public FloatSO _minimumWaterAmountToSurvive;
        public FloatSO _waterAmountToLevelUp;
    }
}
