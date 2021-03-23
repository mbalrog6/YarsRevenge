using UnityEngine;
using UnityEngine.UI;
using YarsRevenge._Project.Audio;

public class ResumeButton : ShakeButton
{
    [SerializeField] private PlaySound _onEnterButtonSound;
    [SerializeField] private PlaySound _onClickButtonSound;
    [SerializeField] private Image _image;
    private AudioSource _audioSource;
    private Color _color;
    private bool _initialized = false;

    private void Start()
    {
        _audioSource = AudioManager.Instance.RequestOmniPresentAudioSource();
    }

    public override void Awake()
    {
        base.Awake();
        #region Audio Mocking...
        if (_onEnterButtonSound == null)
        {
            _onEnterButtonSound = ScriptableObject.CreateInstance<MockSimpleAudioEvent>();
        }

        if (_onClickButtonSound == null)
        {
            _onClickButtonSound = ScriptableObject.CreateInstance<MockSimpleAudioEvent>();
        }
        #endregion
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _color = _image.color;
        _image.color = Color.yellow;

        if (_initialized == false)
        {
            _initialized = true;
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
        GameStateMachine.Instance.ChangeTo = States.PLAY;
    }
}