using UnityEngine;

public class Pause : IState
{
    private readonly GameStateMachine _stateMachine;
    private MenuManager _menuManager;

    public Pause(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public void Tick()
    {
        if (_menuManager.MenuInputDTO.Paused)
        {
            _stateMachine.ChangeTo = States.PLAY;
        }
    }

    public void OnEnter()
    {
        _stateMachine.CurrentState = States.PAUSE;
        _menuManager = _stateMachine.PauseMenu;
        _menuManager.HasFocus = true;
        _menuManager.TweenToOrigin();
    }

    public void OnExit()
    {
        _menuManager.HasFocus = false;
        _menuManager.TweenToStart();
    }
}