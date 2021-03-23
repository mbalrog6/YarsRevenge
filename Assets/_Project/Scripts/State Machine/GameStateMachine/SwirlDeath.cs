
using UnityEngine;

public class SwirlDeath : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private float _timer; 

    public SwirlDeath( GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }
    public void Tick()
    {
        if (Time.time > _timer)
        {
            GameStateMachine.Instance.ChangeTo = States.ADVANCE_LEVEL;
        }
    }

    public void OnEnter()
    {
        _timer = Time.time + 2f;
        GameManager.Instance.AddLife(1);
    }

    public void OnExit()
    {
        _timer = 0f; 
    }
}
