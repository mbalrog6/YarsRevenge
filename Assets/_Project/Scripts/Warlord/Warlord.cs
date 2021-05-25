using DarkTonic.MasterAudio;
using UnityEngine;
using YarsRevenge._Project.Audio;

public class Warlord : MonoBehaviour
{
    [SerializeField] private int ammoProvided;
    [SerializeField] private float ammoCooldown;
    [SerializeField] private int _scoreForSwirl;
    [SerializeField] private int _scoreForWarlord;
    [SerializeField] private QotileFX _qotileFX;
    private GameStateMachine _gameStateMachine;

    public static WarlordState State { get; set; } = WarlordState.Idle;

    public bool CanGetAmmo => Time.time > _ammoProvidedTimer;
    public int Score => State == WarlordState.LaunchedTowardsPlayer ? _scoreForSwirl : _scoreForWarlord;

    public RectContainer WarlordRectContainer => _warlordBounds;
    private RectContainer _warlordBounds;
    private float _ammoProvidedTimer;
    private Vector3 _offscreenPosition = new Vector3(100f, 100f, 0f);

    private Animator _swirlAnimator;
    private Transform _swirlTransform;
    public Vector3 DeathPositoin { get; private set; }

    private void Awake()
    {
        _warlordBounds = new RectContainer(this.gameObject, 1f, 1f, 1f, 1f);

        _swirlAnimator = GetComponentInChildren<Animator>();
        _swirlTransform = _swirlAnimator.transform;
        _swirlTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        _gameStateMachine = FindObjectOfType<GameStateMachine>();
        _gameStateMachine.SetWarlordRef(this);
    }

    private void Update()
    {
        _warlordBounds.UpdateToTargetPosition();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Die();
        }
    }

    public int GetAmmo()
    {
        _ammoProvidedTimer = Time.time + ammoCooldown;
        return ammoProvided;
    }

    public void Die()
    {
        _qotileFX.DisableFX();
        DeathPositoin = transform.position;
        GameStateMachine.Instance.ChangeTo =
            State == WarlordState.LaunchedTowardsPlayer ? States.SWIRLDEATH : States.QOTILEDEATH;
        State = WarlordState.Dead;
        transform.position = _offscreenPosition;
        DeactivateSwirlAnimation();
        MasterAudio.PlaySoundAndForget("SwooshExplosion");
    }

    public void StopSound()
    {
        MasterAudio.StopBus("Qotile");
    }

    public void PlayLaunchAtSound()
    {
        _qotileFX.DisableFX();

        if (MasterAudio.IsSoundGroupPlaying("FastSwirl"))
            return;

        MasterAudio.PlaySoundAndForget("FastSwirl");
    }

    public void PlayChargingSound()
    {
        _qotileFX.EnableFX();

        if (MasterAudio.IsSoundGroupPlaying("wawaSound"))
            return;

        MasterAudio.PlaySoundAndForget("wawaSound");
    }

    private void OnDrawGizmos()
    {
        if (_warlordBounds == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMin, 0f),
            new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMin, 0f),
            new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMin, 0f),
            new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_warlordBounds.Bounds.xMin, _warlordBounds.Bounds.yMax, 0f),
            new Vector3(_warlordBounds.Bounds.xMax, _warlordBounds.Bounds.yMax, 0f));
    }

    public void ActivateSwirlAnimation()
    {
        _swirlTransform.gameObject.SetActive(true);
        _swirlAnimator.Play("SwirlAppear");
    }

    public void DeactivateSwirlAnimation()
    {
        _swirlTransform.gameObject.SetActive(false);
    }
}