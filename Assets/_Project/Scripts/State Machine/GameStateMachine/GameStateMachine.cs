using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private IState _briefPause;
    private bool _subscribe = true;

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
        _briefPause = new BriefPause(this);
        var option = new Option();
        var play = new Play(this);
        var loading = new Loading(this);
        var reset = new Reset(this );
        var swirlDeath = new SwirlDeath( this );
        var qotileDeath = new QotileDeath(this);
        var advanceLevel = new AdvanceLevel(this);
        
        _stateMachine.AddTransition( menu, loading, () => ChangeTo == States.LOADING);
        _stateMachine.AddTransition( loading, play, () => loading.IsFinished);
        
        _stateMachine.AddTransition( play, pause, () => ChangeTo == States.PAUSE);
        _stateMachine.AddTransition( pause, play, () => ChangeTo == States.PLAY);
        _stateMachine.AddTransition( pause, reset, () => ChangeTo == States.MENU);
        _stateMachine.AddTransition( reset, menu, () => ChangeTo == States.MENU);
        
        _stateMachine.AddTransition( play, _briefPause, () => ChangeTo == States.BRIEF_PAUSE);
        _stateMachine.AddTransition( _briefPause, play, () => ChangeTo == States.PLAY);
        
        _stateMachine.AddTransition( play, swirlDeath, () => ChangeTo == States.SWIRLDEATH);
        _stateMachine.AddTransition( play, qotileDeath, () => ChangeTo == States.QOTILEDEATH);
        _stateMachine.AddTransition( advanceLevel, play, () => ChangeTo == States.PLAY);
        
        _stateMachine.AddAnyStateTransition( advanceLevel, () => ChangeTo == States.ADVANCE_LEVEL);
        
        _stateMachine.SetState(menu);
    }

    private void Update()
    {
        _stateMachine.Tick();

        if (!_subscribe)
            return;

        if (IsScene_CurrentlyLoaded("UI") && Mediator.Instance != null )
        {
            Mediator.Instance.Subscribe<ShowDialogueCommand>(HandleShowDialogue);
            Mediator.Instance.Subscribe<HideDialogueCommand>(HandleHideDialogue);
            _subscribe = false; 
        }
    }
    
    private bool IsScene_CurrentlyLoaded(string sceneName_no_extention)
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName_no_extention)
            {
                //the scene is already loaded
                return true;
            }
        }

        return false;
    }

    private void HandleShowDialogue(ShowDialogueCommand command)
    {
        //BriefPauseTime = float.MaxValue;
        //ChangeTo = States.BRIEF_PAUSE;
    }

    private void HandleHideDialogue(HideDialogueCommand command)
    {
        //ChangeTo = States.PLAY;
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
    BRIEF_PAUSE,
    SWIRLDEATH,
    QOTILEDEATH,
    ADVANCE_LEVEL
}