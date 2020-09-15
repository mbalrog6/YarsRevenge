public class AdvanceLevel : IState
{
    private readonly GameStateMachine _gameStateMachine;

    public AdvanceLevel(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }


    public void Tick()
    {
        GameStateMachine.Instance.ChangeTo = States.PLAY;
    }

    public void OnEnter()
    {
        GameManager.Instance.AdvanceToNextLevel();
    }

    public void OnExit()
    {
        
    }
}