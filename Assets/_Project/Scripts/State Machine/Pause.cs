using UnityEngine;
using UnityEngine.UI;

public class Pause : IState
{
    private readonly GameStateMachine _stateMachine;
    private MenuManager _menuManager;
    private Image _image;
    private Vector3 _scale;

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
        _image = _menuManager.GetComponent<Image>();
        _scale = _image.transform.localScale;
        _image.transform.localScale = _scale * 1.02f;
        _menuManager.TweenToOrigin();
        AudioManager.Instance.PauseAudio();
    }

    public void OnExit()
    {
        _menuManager.HasFocus = false;
        _image.transform.localScale = _scale;
        if (_stateMachine.ChangeTo == States.PLAY)
        {
            _menuManager.TweenToStart();
            AudioManager.Instance.UnPauseAudio();
        }
    }
}