public class ResetQotile : IState
{
    private EntityStateMachine _stateMachine;

    public ResetQotile( EntityStateMachine stateMachine )
    {
        _stateMachine = stateMachine;
    }
    public void Tick()
    {
        Warlord.State = WarlordState.Idle;
        _stateMachine.ResetQotile();
    }

    public void OnEnter()
    {
        
       
    }

    public void OnExit()
    {
        
    }
}