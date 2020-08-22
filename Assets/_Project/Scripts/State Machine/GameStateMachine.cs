using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] private MenuManager mainMenu;
    [SerializeField] private MenuManager pauseMenu;

    public MenuManager MainMneu => mainMenu;
    public MenuManager PauseMenu
    {
        get { return pauseMenu; }
        set { pauseMenu = value; }
    }

    public States CurrentState { get; set; }

    public static GameStateMachine Instance => _instance;
    private static GameStateMachine _instance;
    public States ChangeTo { get; set; } = States.NONE;
    public float BriefPauseTime { get; set; } = .05f;

    private StateMachine _stateMachine;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        
        _stateMachine = new StateMachine();

        var menu = new Menu(this);
        var pause = new Pause(this);
        var briefPause = new BriefPause(this);
        var option = new Option();
        var play = new Play(this);
        var loading = new Loading(this);
        var reset = new Reset(this );
        
        _stateMachine.AddTransition( menu, loading, () => ChangeTo == States.LOADING);
        _stateMachine.AddTransition( loading, play, () => loading.IsFinished);
        
        _stateMachine.AddTransition( play, pause, () => ChangeTo == States.PAUSE);
        _stateMachine.AddTransition( pause, play, () => ChangeTo == States.PLAY);
        _stateMachine.AddTransition( pause, reset, () => ChangeTo == States.MENU);
        _stateMachine.AddTransition( reset, menu, () => ChangeTo == States.MENU);
        
        _stateMachine.AddTransition( play, briefPause, () => ChangeTo == States.BRIEF_PAUSE);
        _stateMachine.AddTransition( briefPause, play, () => ChangeTo == States.PLAY);
        
        _stateMachine.SetState(menu);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}

public enum States
{
    NONE, 
    PLAY, 
    PAUSE, 
    MENU, 
    OPTION,
    LOADING,
    RESET,
    BRIEF_PAUSE
}