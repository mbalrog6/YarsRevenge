using UnityEngine;
using UnityEngine.UI;
using YarsRevenge._Project.Audio;
using YarsRevenge._Project.Scripts.Audio.Audio_Scripts;

public class ExitMenuButton : ShakeButton
{
    [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
    [SerializeField] private Image _image;
    [SerializeField] private Image _imageHighlight;
    private Color _color;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _color = _image.color;
        _image.color = Color.white;
        _imageHighlight.enabled = true;
        
        if (_audioSource != null && _onEnterButtonSound != null)
        {
            _onEnterButtonSound.PlayOneShot(_audioSource);
        }
    }

    public override void OnExit()
    {
        _image.color = _color;
        _imageHighlight.enabled = false;
    }

    public override void OnClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
