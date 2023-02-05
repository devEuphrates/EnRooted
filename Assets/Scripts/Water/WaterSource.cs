using UnityEngine;

public class WaterSource : MonoBehaviour
{
    [SerializeField] Transform _scaled;
    [SerializeField] Transform _displayer;
    [SerializeField] float _downscaleValue = .1f;
    float _startedScale;

    private void Start()
    {
        _startedScale = _scaled.localScale.y;
        _displayer.localScale = _scaled.localScale;
    }

    public void ModifyVolume()
    {
        _scaled.localScale = new Vector3(_scaled.localScale.x, Mathf.Clamp(_scaled.localScale.y - _downscaleValue, 0, _startedScale), _scaled.localScale.z);
        if (_scaled.localScale.y < _downscaleValue)
            Destroy(gameObject);
    }
}
