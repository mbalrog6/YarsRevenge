using System;
using UnityEngine;

public class FullRevolutionRotation : MonoBehaviour, IFinishedEvent
{
    [SerializeField] private float numberOfRotations;
    [SerializeField] private AnimationCurve easingFunction;
    [SerializeField] private float durationForRotation;
    [SerializeField] private bool IsRunning = false;

    public event Action IsFinished;

    private float _timer;
    private float _normalizedTimer;
    private float _deltaAngle;
    private Vector3 _startAngle;
    private Vector3 _endAngle;
    private Vector3 _currentAngleEuler;
    private Quaternion _currentRotation;
    private Transform _myTransform;

    private void Awake()
    {
        _myTransform = transform;
        GetStartingValues();
    }

    private void Update()
    {
        if (IsRunning == false)
        {
            return;
        }

        if (_normalizedTimer >= 1f)
        {
            IsFinished?.Invoke();
            IsRunning = false;
        }

        _timer += Time.deltaTime;
        GetNewRotation();

        _myTransform.localRotation = _currentRotation;
    }

    private void GetNewRotation()
    {
        _normalizedTimer = _timer / durationForRotation;
        
        _deltaAngle = Mathf.Lerp(_startAngle.z, _endAngle.z, easingFunction.Evaluate(_normalizedTimer));
        _currentAngleEuler.z = _deltaAngle * numberOfRotations;
        _currentRotation = Quaternion.Euler(_currentAngleEuler);
    }

    private void GetStartingValues()
    {
        _timer = 0f;
        _normalizedTimer = 0f;
        _startAngle = _myTransform.localRotation.eulerAngles;
        _endAngle = _startAngle + new Vector3(0f, 0f, 360f);
        _currentRotation = _myTransform.localRotation;
        _currentAngleEuler = _startAngle;
    }

    public void StartRotation()
    {
        IsRunning = true;
        GetStartingValues();
    }
}
