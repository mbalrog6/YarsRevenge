using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : IState
{
    private GameStateMachine _statemachine;
    public bool IsFinished => _operations.TrueForAll(t => t.isDone);
    private List<AsyncOperation> _operations;

    public Loading( GameStateMachine stateMachine )
    {
        _statemachine = stateMachine;
        _operations = new List<AsyncOperation>();
    }
    
    public void Tick()
    {
                
    }

    public void OnEnter()
    {
        _statemachine.CurrentState = States.LOADING;
        _statemachine.ChangeTo = States.NONE;
        _operations.Add( SceneManager.LoadSceneAsync("Initial Scene") );
        _operations.Add( SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive));
    }

    public void OnExit()
    {
        _operations.Clear();
        _statemachine.PauseMenu = GameObject.FindObjectOfType<MenuManager>();
    }
}