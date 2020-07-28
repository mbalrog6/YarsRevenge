﻿using UnityEngine;

public class Menu : IState
{
    private readonly GameStateMachine _stateMachine;
    private MenuManager _menuManager;

    public Menu( GameStateMachine stateMachine )
    {
        _stateMachine = stateMachine;
    }
    public void Tick()
    {
                
    }

    public void OnEnter()
    {
        _stateMachine.CurrentState = States.MENU;
        _stateMachine.ChangeTo = States.NONE;
        _menuManager = GameObject.FindObjectOfType<MenuManager>();
        _menuManager.HasFocus = true; 
    }

    public void OnExit()
    {
        _menuManager.HasFocus = false;
    }
}