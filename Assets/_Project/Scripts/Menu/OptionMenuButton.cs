using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using YarsRevenge._Project.Audio;
using YarsRevenge._Project.Scripts.Audio.Audio_Scripts;

public class OptionMenuButton : ShakeButton
{
    [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
    [SerializeField] private Image _image;
    [SerializeField] private Image _imageHighlight;
    [SerializeField] private TextMeshProUGUI _textElement;
    private Color _color;
    private AudioSource _audioSource;
    private String _originalText; 

    private void Start()
    {
        _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
        _originalText = _textElement.text;
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
        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        _textElement.text = "Sorry!";
        yield return new WaitForSeconds(1f);
        _textElement.text = _originalText;
    }
}
