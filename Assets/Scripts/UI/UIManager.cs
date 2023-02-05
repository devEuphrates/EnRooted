using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private FloatSO _waterAmount;
    //[SerializeField] private FloatSO _treeSize;
    [SerializeField] private TextMeshProUGUI _water_AmountUI;
    //[SerializeField] private TextMeshProUGUI _tree_SizeUI;
    void Start()
    {
        _waterAmount.OnValueChange += OnWaterChanged;
        //_treeSize.OnValueChange += OnTreeSizeChanged;
        _water_AmountUI.SetText(_waterAmount.Value.ToString());
        //_tree_SizeUI.SetText(_treeSize.Value.ToString());
    }

    private void OnWaterChanged()
    {
        _water_AmountUI.SetText(_waterAmount.Value.ToString());

    }
    //private void OnTreeSizeChanged()
    //{
    //    _tree_SizeUI.SetText(_treeSize.Value.ToString());
    //}
}
