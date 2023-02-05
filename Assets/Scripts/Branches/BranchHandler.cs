using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BranchHandler : MonoBehaviour
{
    [SerializeField] private BezierRoute[] _bezierRoute;
    private int _treeLevel = 0;
    private event Action<BezierRoute> OnCreate;
    [SerializeField] private BranchCreator _branchCreator;
    [SerializeField] private TreeData _treeData;
    [SerializeField] private FloatSO _waterAmount;
    [SerializeField] private TreeBody _treeBody;
    private Dictionary<int, List<TreeBranch>> _level_branch_dict = new Dictionary<int, List<TreeBranch>>();

    public void Awake()
    {
        _waterAmount.OnValueChange += OnWaterIncrease;
    }
    public void Start()
    {
        _waterAmount.Value += 20;
    }

    public void OnWaterIncrease()
    {
        Debug.Log("test");
        if (_waterAmount.Value < _treeData.LevelDataList[_treeData.CurrentLevel]._minimumWaterAmountToSurvive.Value)

        {
            if (_treeData.CurrentLevel == 0) InitialRemove();
            else
            {
                LevelDown();
                _treeData.CurrentLevel -= 1;

            }
        }

        else if (_waterAmount.Value >= _treeData.LevelDataList[_treeData.CurrentLevel]._waterAmountToLevelUp.Value)
        {
            if (_treeData.CurrentLevel == 0) InitialAdd();
            else
            {
                LevelUp();
                _treeData.CurrentLevel += 1;
            }
        }
    }


    private void InitialAdd()
    {
        foreach (var tr_item in _treeBody.GetInitialPoints())
        {
            var item = _bezierRoute[UnityEngine.Random.Range(0, _bezierRoute.Length)];
            _branchCreator.AddBranch(tr_item, item);
        }

    }

    private void InitialRemove()
    {
        foreach (var tr_item in _treeBody.GetInitialPoints())
        {
            _branchCreator.DeleteBranch(tr_item);
        }
    }

    private void LevelUp()
    {
        var spawn_amount = _treeData.LevelDataList[_treeData.CurrentLevel].spawn_count;
        foreach (var branch in _level_branch_dict[_treeData.CurrentLevel])
        {
            _branchCreator.AddBranch(branch, spawn_amount, _bezierRoute);
        }
    }

    private void LevelDown()
    {
        foreach(var branch in _level_branch_dict[_treeData.CurrentLevel])
        {
            _branchCreator.DeleteBranch(branch);
        }
    }



}

