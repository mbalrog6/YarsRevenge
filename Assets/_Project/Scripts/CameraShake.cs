using System;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _strength = 1f;
    [SerializeField] private int _profileIndex = 0;
    [SerializeField] private AnimationCurve[] shakeProfile = new AnimationCurve[3];

    public static event Action OnStartShake;
    public static event Action OnEndShake;

    private Transform _cameraTransform;
    private Vector3 _originalPosition;

    private static int _priority = 0;
    private static float _time;
    private static bool isRunning;
    private static bool isCustomShake = false;
    private static float _workingDuration = 1f;
    private static float _workingStrength = 1f;
    private static int _workingIndex = 0;

    private void Awake()
    {
       _cameraTransform = GetComponent<Camera>().transform;
       _originalPosition = _cameraTransform.transform.position;
    }

    private void Update()
    {
        if (_time > 1)
        {
            isRunning = false;
            _time = 0;
            _cameraTransform.position = _originalPosition;
            _priority = 0;
            OnEndShake?.Invoke();
        }
        
        if (isRunning == false)
        {
            return; 
        }

        float positionOffset;
        if (isCustomShake)
        {
            CalculateShake(out positionOffset, _workingDuration, _workingStrength, _workingIndex);
        }
        else
        {
            CalculateShake(out positionOffset, _duration, _strength, _profileIndex);
        }
        
        _cameraTransform.position =
            new Vector3(_originalPosition.x + positionOffset, _originalPosition.y, _originalPosition.z);
    }

    private void CalculateShake(out float positionOffset, float duration, float strength, int profileIndex)
    {
        _time += Time.deltaTime / duration;

        var animationValue = shakeProfile[profileIndex].Evaluate(_time);
        positionOffset = (animationValue * 2 - 1) * strength;
    }

    public static void CustomShake(float duration, float strength, int profileIndex, int priority = 0)
    {
        if (isRunning == false)
        {
            OnStartShake?.Invoke();
        }
        if (isRunning == true && _priority >= priority)
            return; 
        
        isRunning = true;
        _time = 0;
        isCustomShake = true;
        _priority = priority;

        _workingDuration = duration;
        _workingStrength = strength;
        _workingIndex = profileIndex;
    }
    
    public static void Shake()
    {
        if (isRunning == false)
        {
            OnStartShake?.Invoke();
        }
        isRunning = true;
        _time = 0;
        isCustomShake = false; 
    }

    [ContextMenu("Shake1")]
    public void Shake1()
    {
        Shake(); 
    }
}
