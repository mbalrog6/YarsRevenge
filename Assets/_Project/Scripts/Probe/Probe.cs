using System;
using DarkTonic.MasterAudio;
using UnityEngine;
using YarsRevenge._Project.Audio;

[RequireComponent(typeof(FaceTowardsRotator))]
public class Probe : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Warlord warlord;
    [SerializeField] private int _ScoreValue;
    [SerializeField] private MoveForward2D _mover;
    [SerializeField] private GameObject _probeGlow;
    [SerializeField] private IonZone _ionZone;

    public event Action OnDie;
    public event Action OnRespawn;

    public RectContainer ProbeRectContainer => _probeRectContainer;
    public bool IsDead => _isDead;
    public float Radius => radius;
    public int Score => _ScoreValue;

    private bool _isDead;
    private RectContainer _probeRectContainer;
    private GameObject _visual;
    private FaceTowardsRotator _rotator;
    private Transform _initialRotatorTarget;
    private float _timer = 0f;
    private float _respawnTimer;
    private float _respawnDecrementTimer;
    private float _speedIncrementTimer;
    private float _pauseDelay;
    [SerializeField] private ProbeInfo _probeInfo;
    private float _speedLerpIncrement;
    private float _rechargeLerpIncrement;
    private float _speedLerpProgression;
    private float _rechargeLerpProgression;


    private void Awake()
    {
        _isDead = false;
        _visual = transform.GetChild(0).gameObject;
        _probeRectContainer = new RectContainer(this.gameObject, .125f, .125f, .5f, .5f);
        _rotator = GetComponent<FaceTowardsRotator>();
        _initialRotatorTarget = _rotator.Target;

        SetProbeInfo(_probeInfo);
    }

    private void Update()
    {
        if (GameStateMachine.Instance.CurrentState == States.PAUSE || 
        GameStateMachine.Instance.CurrentState == States.BRIEF_PAUSE)
        {
            _pauseDelay += Time.deltaTime;
            return;
        }

        if (_pauseDelay > 0)
        {
            _timer += _pauseDelay;
            _respawnDecrementTimer += _pauseDelay;
            _speedIncrementTimer += _pauseDelay;
            _pauseDelay = 0f;
        }

        if (Time.time > _respawnDecrementTimer)
        {
            _respawnDecrementTimer = Time.time + _probeInfo.probeRespawnChangeTimer;
            _rechargeLerpProgression += _rechargeLerpIncrement;
            _rechargeLerpProgression = Mathf.Clamp(_rechargeLerpProgression, 0f, 1f);
            SetRespawnTimer();
        }

        if (Time.time > _speedIncrementTimer)
        {
            _speedIncrementTimer = Time.time + _probeInfo.speedIncrementTimer;
            _speedLerpProgression += _speedLerpIncrement;
            _speedLerpProgression = Mathf.Clamp(_speedLerpProgression, 0f, 1f);
            SetProbeSpeed();
        }

        if (IsDead)
        {
            if (Time.time > _timer && Warlord.State == WarlordState.Idle)
            {
                Respawn();
            }
            else
            {
                return;
            }
        }

        _probeRectContainer.UpdateToTargetPosition();
        CheckForIonCollision();
    }

    private void SetProbeSpeed()
    {
         _mover.Speed = Mathf.Lerp(_probeInfo.probeSpeedMin, _probeInfo.probeSpeedMax, _speedLerpProgression);
    }

    private void SetRespawnTimer()
    {
        _respawnTimer = Mathf.Lerp(_probeInfo.probeRespawnTimerMax, _probeInfo.probeRespawnTimerMin, _rechargeLerpProgression);
    }

    public void Die()
    {
        if (_isDead)
        {
            return;
        }
        _isDead = true;
        _visual.SetActive(false);
        MasterAudio.PlaySoundAndForget("Explosion Small_02");
        _timer = Time.time + _respawnTimer;
        OnDie?.Invoke();
        _probeGlow.SetActive(false);
    }

    public void Respawn()
    {
        _isDead = false;
        _visual.SetActive(true);
        _rotator.SetTarget(_initialRotatorTarget);
        transform.position = warlord.transform.position;
        OnRespawn?.Invoke();
    }

    public void SetProbeInfo(ProbeInfo probeInfo)
    {
        _probeInfo = probeInfo;
        _speedLerpIncrement = 1f / _probeInfo.speedIncrementRate;
        _rechargeLerpIncrement = 1f / _probeInfo.probeRespawnRateChange;
        SetRespawnTimer();
        SetProbeSpeed();
    }

    public void Reset( ProbeInfo probeInfo )
    {
        SetProbeInfo(_probeInfo);
        _isDead = true;
        _visual.SetActive(false);
        _timer = Time.time + _respawnTimer;
    }

    private void CheckForIonCollision()
    {
        if (_isDead)
            return;
        
        if (_probeRectContainer.Bounds.Overlaps(_ionZone.IonRectContainer.Bounds))
        {
            if (_probeGlow.activeSelf == false) 
            {
                _probeGlow.SetActive(true);
            }
        }
        else
        {
            if (_probeGlow.activeSelf == true)
            {
                _probeGlow.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {

        if (_probeRectContainer == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMin, 0f),
            new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMin, 0f),
            new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMin, 0f),
            new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_probeRectContainer.Bounds.xMin, _probeRectContainer.Bounds.yMax, 0f),
            new Vector3(_probeRectContainer.Bounds.xMax, _probeRectContainer.Bounds.yMax, 0f));
        
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
