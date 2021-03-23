using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    [SerializeField] private float angularVelocity;

    private float _rotation;
    private Transform _myTransform;
    private Quaternion _myLocalRotation;
    private Vector3 _eulurRotation;

    private void Awake()
    {
        _myTransform = transform;
        _myLocalRotation = _myTransform.localRotation;
        _eulurRotation = _myLocalRotation.eulerAngles;
    }

    private void Update()
    {
        _eulurRotation = _myLocalRotation.eulerAngles;
        float rotatingValue = (_eulurRotation.z += angularVelocity) % 360;
        _eulurRotation.z = rotatingValue;
        _myLocalRotation = Quaternion.Euler(_eulurRotation);
        _myTransform.localRotation = _myLocalRotation;
    }
}
