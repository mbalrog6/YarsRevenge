using UnityEngine;

public class Play : IState
{
    private GameStateMachine _stateMachine;

    public Play( GameStateMachine stateMachine )
    {
        _stateMachine = stateMachine;
    }
    public void Tick()
    {
        if (InputManager.Instance.PlayerInputDTO.Paused)
        {
            _stateMachine.ChangeTo = States.PAUSE;
        }
    }

    public void OnEnter()
    {
        _stateMachine.CurrentState = States.PLAY;
        _stateMachine.ChangeTo = States.NONE;
        
        Mediator.Instance.Publish<ShowDialogueCommand>(new ShowDialogueCommand());
        DialogueManager.Instance.ShowNextStoryElement();
        Debug.Log( $"In Play Enter");
    }

    public void OnExit()
    {
        
    }
}