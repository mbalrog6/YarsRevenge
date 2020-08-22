using UnityEngine;
using UnityEngine.UI;

public class PlayMenuButton : ShakeButton
{
    [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
    [SerializeField] private SimpleAudioEvent _onClickButtonSound;
    [SerializeField] private Image _image;
    private Color _color;
    private bool _initalized = false;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _color = _image.color;
        _image.color = Color.yellow;

        if (_initalized == false)
        {
            _initalized = true;
            return;
        }
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
        GameStateMachine.Instance.ChangeTo = States.LOADING;
    }
}
