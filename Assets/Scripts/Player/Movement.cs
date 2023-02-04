using System.Security.Cryptography;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Transform _transform;

    [SerializeField] FloatSO _moveSpeed;
    [SerializeField] FloatSO _shiftMultiplier;
    [SerializeField] FloatSO _ctrlMultiplier;

    private void Awake()
    {
        _transform = transform;
    }

    void LateUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float mult = 1f;
        if (Input.GetKey(KeyCode.LeftControl))
            mult = _ctrlMultiplier.Value;

        if (Input.GetKey(KeyCode.LeftShift))
            mult = _shiftMultiplier.Value;

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftShift))
            mult = 1f;

        h *= _moveSpeed.Value * mult * Time.deltaTime;
        v *= _moveSpeed.Value * mult * Time.deltaTime;

        Vector3 move = new Vector3(h, v, 0);
        _transform.position += move;
    }
}
