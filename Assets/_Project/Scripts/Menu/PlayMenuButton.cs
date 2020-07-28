using UnityEngine;
using UnityEngine.UI;

public class PlayMenuButton : ShakeButton
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
    [SerializeField] private Image _image;
    private Color _color;
    private bool _initalized = false;

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
        GameStateMachine.Instance.ChangeTo = States.LOADING;
    }
}
