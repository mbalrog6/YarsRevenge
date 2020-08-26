using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : IState
{
    private GameStateMachine _stateMachine;
    private AsyncOperation _operation;

    public Reset( GameStateMachine stateMachine )
    {
        _stateMachine = stateMachine;
    }
    public void Tick()
    {
        if (_operation.isDone)
        {
            _stateMachine.ChangeTo = States.MENU;
            AudioManager.Instance.StopAllSound();
        }
    }

    public void OnEnter()
    {
        _stateMachine.CurrentState = States.RESET;
        _stateMachine.ChangeTo = States.NONE;
        _operation = SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OnExit()
    {
        
    }
}