using UnityEngine;

public class QotileDeath : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private float _timer; 
    private ExplosionTransionStartCommand _explosionStartCommand;
    public Warlord _warlord { get; set; }

    public QotileDeath(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
        _explosionStartCommand = new ExplosionTransionStartCommand();
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
        if (_warlord != null)
        {
            _explosionStartCommand.Position = _warlord.DeathPositoin;
            Debug.Log($"x: {_explosionStartCommand.Position.x}, y: {_explosionStartCommand.Position.y} - my pos x: {_warlord.DeathPositoin.x}, y: {_warlord.DeathPositoin.y}");
        }
        Mediator.Instance.Publish(_explosionStartCommand);
        _timer = Time.time + 2f;
    }

    public void OnExit()
    {
        _timer = 0f;
    }
    
}