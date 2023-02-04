using UnityEngine;

public class Scroll : MonoBehaviour
{
    Transform _transform;

    [SerializeField] FloatSO _scrollSpeed;

    [SerializeField] FloatSO _shiftMultiplier;
    [SerializeField] FloatSO _ctrlMultiplier;

    [Space]
    [SerializeField] float _minDist = 3f;
    [SerializeField] float _maxDist = 30f;

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newZ = Mathf.Clamp(_transform.position.z + (scroll * _scrollSpeed.Value * Time.deltaTime), -_maxDist, -_minDist);
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, newZ);
    }
}
