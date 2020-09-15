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
        DebugText.Instance.SetText("Small Explosion Here");
        if (Time.time > _timer)
        {
            GameStateMachine.Instance.ChangeTo = States.PLAY;
        }
    }

    public void OnEnter()
    {
        _timer = Time.time + 2f;
    }

    public void OnExit()
    {
        
    }
}