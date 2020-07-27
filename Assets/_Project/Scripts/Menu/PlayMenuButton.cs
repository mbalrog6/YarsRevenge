using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenuButton : ShakeButton
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
    [SerializeField] private Image _image;
    private Color _color;

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
        
    }
}
