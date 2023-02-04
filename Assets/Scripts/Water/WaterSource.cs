using UnityEngine;

public class WaterSource : MonoBehaviour
{
    [SerializeField] private float _baseValue;
    private float _currentValue;
    private float _targetValue;
    [SerializeField] private float _decrease_by_Value;
    [SerializeField] private float _approach_rate;
    Collider _collider;

    private void Awake()
    {
        _currentValue = _baseValue;
        _targetValue = _baseValue;
        _collider = gameObject.GetComponent<Collider>();
    }
    public void ModifyValue()
    {
        _targetValue = (_targetValue - _decrease_by_Value <= 0) ? 0 : _targetValue - _decrease_by_Value;
        if (_targetValue == 0) _collider.isTrigger = false;
    }
    void Update()
    {
        var temp = transform.localScale;
        _currentValue = Mathf.Lerp(_currentValue, _targetValue, _approach_rate);
        temp.y = _currentValue / _baseValue;
        transform.localScale = temp;
    }
}
