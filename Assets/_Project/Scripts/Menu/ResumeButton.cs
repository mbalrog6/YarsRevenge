﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : ShakeButton
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SimpleAudioEvent _onEnterButtonSound;
    [SerializeField] private Image _image;
    private Color _color;
    private bool _initialized = false;

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
        GameStateMachine.Instance.ChangeTo = States.PLAY;
    }
}