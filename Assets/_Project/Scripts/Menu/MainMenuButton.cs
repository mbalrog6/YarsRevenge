using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : ShakeButton
{
    [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
    [SerializeField] private SimpleAudioEvent _onClickButtonSound;
    [SerializeField] private Image _image;
    private Color _color;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = AudioManager.Instance.RequestOmniPresentAudioSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _color = _image.color;
        _image.color = Color.yellow;
        
        if (_audioSource != null && _onEnterButtonSound != null)
        {
            _onEnterButtonSound.PlayOneShot(_audioSource);
        }
    }

    public override void OnExit()
    {
        _image.color = _color;
    }

    public override void OnClick()
    {
        _onClickButtonSound.PlayOneShot(_audioSource);
        GameStateMachine.Instance.ChangeTo = States.MENU;
    }
}
