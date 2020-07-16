using UnityEngine;
public class EntityStateMachine : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Barrier barrier;

    private float _timer;
    private StateMachine _stateMachine;

    private void Start()
    {
        _timer = Time.time + 2f; 
        
        _stateMachine = new StateMachine();
        
        var idle = new Idle(this, barrier.transform.GetChild(0));
        var chargeUp = new ChargeUp(this);
        var launchTowardsPlayer = new LaunchTowardsPlayer(this, player);
        
        _stateMachine.AddState(idle);
        _stateMachine.AddState(chargeUp);
        _stateMachine.AddState(launchTowardsPlayer);

        _stateMachine.AddTransition(idle, chargeUp, () => Time.time > _timer);
        _stateMachine.AddTransition(chargeUp, launchTowardsPlayer, () => Time.time > _timer);
        _stateMachine.AddTransition(launchTowardsPlayer, idle, () => Time.time > _timer);
        
        _stateMachine.SetState(idle);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void SetTimer(float time)
    {
        _timer = Time.time + time; 
    }
    
}