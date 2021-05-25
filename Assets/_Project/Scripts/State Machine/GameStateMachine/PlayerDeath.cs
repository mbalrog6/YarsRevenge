using UnityEngine;

public class PlayerDeath : IState
{
    private GameStateMachine _stateMachine;

    public PlayerDeath(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }
}
