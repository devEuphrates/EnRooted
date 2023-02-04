using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float SO", menuName = "SO Variables/Float")]
public class FloatSO : ScriptableObject
{
    [SerializeField] float _value;
    public event Action OnValueChange;
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChange?.Invoke();
        }
    }
}
