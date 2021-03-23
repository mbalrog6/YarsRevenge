using UnityEngine;

public class QotileDeath : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private float _timer; 

    public QotileDeath(GameStateMachine gameStateMachine)
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
    }

    public void OnExit()
    {
        _timer = 0f;
    }
}