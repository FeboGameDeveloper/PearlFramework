using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTransform;

    private Transform _myTransform;

    private void Start()
    {
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform;
        }

        _myTransform = transform;
    }

    private void Update()
    {
        transform.LookAt(_myTransform.position + _cameraTransform.rotation * Vector3.forward, _cameraTransform.rotation * Vector3.up);
    }
}