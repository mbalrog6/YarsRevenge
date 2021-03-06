﻿using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance => _instance;
    private static InputManager _instance;

    public InputDTO PlayerInputDTO => _playerInputDTO;

    private PlayerInput _playerInput;
    private InputDTO _playerInputDTO;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
             Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);

            _playerInput = new PlayerInput();
            _playerInputDTO = new InputDTO();
        }
    }

    private void Update()
    {
        _playerInput.Tick();
        _playerInput.SetInput(ref _playerInputDTO);
    }
    
}
