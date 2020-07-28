using System;
using UnityEngine;
public class EntityStateMachine : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Barrier barrier;
    public Warlord WarlordEntity => _warlord;

    private float _timer;
    private float _pauseDelay;
    private StateMachine _stateMachine;
    private Warlord _warlord;

    private void Awake()
    {
        _warlord = GetComponent<Warlord>();
    }

    private void Start()
    {
        _timer = Time.time + 2f; 
        
        _stateMachine = new StateMachine();
        
        var idle = new Idle(this, barrier.transform.GetChild(0));
        var chargeUp = new ChargeUp(this, barrier.transform.GetChild(0));
        var launchTowardsPlayer = new LaunchTowardsPlayer(this, player);
        var dead = new Died(GetComponent<Warlord>());
        
        _stateMachine.AddState(idle);
        _stateMachine.AddState(chargeUp);
        _stateMachine.AddState(launchTowardsPlayer);
        _stateMachine.AddState(dead);

        _stateMachine.AddTransition(idle, chargeUp, () => Time.time > _timer);
        _stateMachine.AddTransition(chargeUp, launchTowardsPlayer, () => Time.time > _timer);
        _stateMachine.AddTransition(launchTowardsPlayer, idle, () => CheckForOffScreen());
        
        _stateMachine.AddAnyStateTransition(dead, () => Warlord.State == WarlordState.Dead);
        
        _stateMachine.SetState(idle);
    }

    private void Update()
    {
        if (GameStateMachine.Instance.CurrentState == States.PAUSE)
        {
            _pauseDelay += Time.deltaTime;
            return;
        }

        if (_pauseDelay > 0)
        {
            _timer += _pauseDelay;
            _pauseDelay = 0f;
        }
        
        
            _stateMachine.Tick();
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
    
}