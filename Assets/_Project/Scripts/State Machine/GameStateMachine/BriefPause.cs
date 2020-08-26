using UnityEngine;

public class BriefPause : IState
{
    private float _timer;
    private GameStateMachine _stateMachine;

    public BriefPause( GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public void Tick()
    {
        if (Time.time > _timer)
        {
            _stateMachine.ChangeTo = States.PLAY;
        }
    }

    public void OnEnter()
    {
        _timer = Time.time + _stateMachine.BriefPauseTime;
        _stateMachine.CurrentState = States.BRIEF_PAUSE;
        _stateMachine.ChangeTo = States.NONE;
    }

    public void OnExit()
    {
        
    }
}