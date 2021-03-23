using UnityEngine;

public class SwayMotor : MonoBehaviour
{
    [SerializeField] private float xDistanceDelta;
    [SerializeField] private float xTimeToAmplitude;
    [SerializeField] private float yDistanceDelta;
    [SerializeField] private float yTimeToAmplitude;
    [SerializeField] private float zDistanceDelta;
    [SerializeField] private float zTimeToAmplitude;
    [SerializeField][Range(0,100)] private float xRandomTick = 0.0f;
    [SerializeField][Range(0,100)] private float yRandomTick = 0.0f;
    [SerializeField][Range(0,100)] private float zRandomTick = 0.0f;
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis;
    [SerializeField] private bool zAxis;
    [SerializeField] private SwayProfile swayProfile;
    [SerializeField] private bool useLocalPosition;


    private Transform myTransform;
    private Vector3 _originalPosition;
    private float _xTime;
    private float _yTime;
    private float _zTime;
    private float _xDelta;
    private float _yDelta;
    private float _zDelta;
    private float _timeRunning;
    private float _fullRange;
    private float _xTimeDirection;
    private float _yTimeDirection;
    private float _zTimeDirection;

    private void Awake()
    {
        myTransform = GetComponent<Transform>();
        _xTime = xTimeToAmplitude / 2f;
        _yTime = yTimeToAmplitude / 2f;
        _zTime = zTimeToAmplitude / 2f;
        _xDelta = 0.5f;
        _yDelta = 0.5f;
        _zDelta = 0.5f;
        _xTimeDirection = 1f;
        _yTimeDirection = 1f;
        _zTimeDirection = 1f;

        if (useLocalPosition == true)
        {
            _originalPosition = myTransform.localPosition;
        }
        else
        {
            _originalPosition = myTransform.position;
        }
    }

    private void Update()
    {
        float value = 0.0f;
        Vector3 position = Vector3.zero;

        if (xAxis)
        {
            value = Mathf.Lerp(-xDistanceDelta, xDistanceDelta, _xDelta);
            position.x = value;
        }

        if (yAxis)
        {
            value = Mathf.Lerp(-yDistanceDelta, yDistanceDelta, _yDelta);
            position.y = value;
        }

        if (zAxis)
        {
            value = Mathf.Lerp(-zDistanceDelta, zDistanceDelta, _zDelta);
            position.z = value;
        }

        if (GetRandomChance(xRandomTick))
        {
            _xTime += Time.deltaTime * _xTimeDirection;
        }

        if (GetRandomChance(yRandomTick))
        {
            _yTime += Time.deltaTime * _yTimeDirection;
        }

        if (GetRandomChance(zRandomTick))
        {
            _zTime += Time.deltaTime * _zTimeDirection;
        }

        if (_xDelta > 1f)
        {
            _xDelta = 1;
            _xTime = xTimeToAmplitude;
            _xTimeDirection = -1f;
        }

        if (_xTime < 0f)
        {
            _xDelta = 0f;
            _xTime = 0f;
            _xTimeDirection = 1f;
        }
        
        if (_yDelta > 1f)
        {
            _yDelta = 1;
            _yTime = yTimeToAmplitude;
            _yTimeDirection = -1f;
        }

        if (_yTime < 0f)
        {
            _yDelta = 0f;
            _yTime = 0f;
            _yTimeDirection = 1f;
        }
        
        if (_zDelta > 1f)
        {
            _zDelta = 1;
            _zTime = zTimeToAmplitude;
            _zTimeDirection = -1f;
        }

        if (_zTime < 0f)
        {
            _zDelta = 0f;
            _zTime = 0f;
            _zTimeDirection = 1f;
        }

        _xDelta = _xTime / xTimeToAmplitude;
        _yDelta = _yTime / yTimeToAmplitude;
        _zDelta = _zTime / zTimeToAmplitude;

        if (useLocalPosition == true)
        {
            myTransform.localPosition = _originalPosition + position;
        }
        else
        {
            myTransform.position = myTransform.position + position;
        }
    }

    private bool GetRandomChance(float value)
    {
        if (value <= 0f)
            return true;

        if (Random.Range(0, 100) <= value)
        {
            return true;
        }

        return false;
    }
}