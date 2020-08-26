using UnityEngine;

public class EntityStateMachine : MonoBehaviour
{
    [SerializeField] private Player player;
    public Warlord QotileEntity => _qotile;
    public int ChargeTimeAdvancementCount { get; set; } = 1;
    public int LaunchSpeedAdvancementCount { get; set; } = 1;
    public float ChargeTime => _chargeTime;
    public float LaunchSpeed => _launchSpeed;

    private Barrier2 barrier;
    private float _timer;
    private float _pauseDelay;
    private StateMachine _stateMachine;
    private Warlord _qotile;

    [SerializeField] private QotileInfo _qotileInfo;
    private float _chargeTime;
    private float _launchSpeed;

    private void Awake()
    {
        _qotile = GetComponent<Warlord>();
        
        _stateMachine = new StateMachine();

        var idle = new Idle(this);
        var chargeUp = new ChargeUp(this);
        var launchTowardsPlayer = new LaunchTowardsPlayer(this, player);
        var quotileDied = new Died( _qotile );
        var reset = new ResetQotile( this );

        _stateMachine.AddState(idle);
        _stateMachine.AddState(chargeUp);
        _stateMachine.AddState(launchTowardsPlayer);
        _stateMachine.AddState(quotileDied);

        _stateMachine.AddTransition(idle, chargeUp, () => Time.time > _timer);
        _stateMachine.AddTransition(chargeUp, launchTowardsPlayer, () => Time.time > _timer);
        _stateMachine.AddTransition(launchTowardsPlayer, idle, () => CheckForOffScreen());
        _stateMachine.AddTransition( reset, idle, () => Warlord.State == WarlordState.Idle);

        _stateMachine.AddAnyStateTransition(quotileDied, () => Warlord.State == WarlordState.Dead);
        _stateMachine.AddAnyStateTransition(reset, () => Warlord.State == WarlordState.Reset);

        _stateMachine.SetState(idle);
    }

    private void Start()
    {
        GameManager.Instance.OnBarrierChanged += UpdateBarrier;
        _timer = Time.time + 2f;
        _chargeTime = _qotileInfo.chargeUpTimeMax;
        _launchSpeed = _qotileInfo.launchSpeedMin;

       
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
            _pauseDelay = 0f;
        }

        DebugText.Instance.SetText($"Charge Time = {_chargeTime}");
        _launchSpeed = GetLaunchSpeed();
        _chargeTime = GetChargeTime();
        _stateMachine.Tick();
    }

    private float GetChargeTime()
    {
        if (ChargeTimeAdvancementCount % _qotileInfo.chargeUpAdvancementRate == 0)
        {
            ChargeTimeAdvancementCount = 1;
            _chargeTime -= _qotileInfo.chargeUpDecrementValue;
            _chargeTime = Mathf.Clamp(_chargeTime, _qotileInfo.chargeUpTimeMin, _qotileInfo.chargeUpTimeMax);
        }

        return _chargeTime;
    }

    private float GetLaunchSpeed()
    {
        if (LaunchSpeedAdvancementCount % _qotileInfo.launchSpeedAdvancementRate == 0)
        {
            LaunchSpeedAdvancementCount = 1;
            _launchSpeed += _qotileInfo.launchSpeedIncrementValue;
            _launchSpeed = Mathf.Clamp(_launchSpeed, _qotileInfo.launchSpeedMin, _qotileInfo.launchSpeedMax);
        }

        return _launchSpeed;
    }

    public void SetTimer(float time)
    {
        _timer = Time.time + time;
    }

    private bool CheckForOffScreen()
    {
        if (ScreenHelper.Instance.ScreenBounds.Contains(transform.position))
            return false;
        return true;
    }

    public void LoadNewQotileInfo(QotileInfo qotileInfo)
    {
        _qotileInfo = qotileInfo;
        
        _chargeTime = _qotileInfo.chargeUpTimeMax;
        _launchSpeed = _qotileInfo.launchSpeedMin;

        Warlord.State = WarlordState.Reset;
    }

    public void ResetQotile()
    { 
        ChargeTimeAdvancementCount = 1;
        LaunchSpeedAdvancementCount = 1;
    }

    private void UpdateBarrier(Barrier2 barrier)
    {
        this.barrier = this.barrier;
    }
}