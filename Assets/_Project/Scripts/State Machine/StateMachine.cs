using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState _currentState { get; private set; }
    private List<IState> _states = new List<IState>();
    private List<StateTransition> _anyStateTransitions =  new List<StateTransition>();
    private Dictionary<IState, List<StateTransition>> _stateTransitons = new Dictionary<IState, List<StateTransition>>();

    public void Tick()
    {
        StateTransition transition = CheckForTransition();
        if (transition != null)
        {
            SetState(transition.To);
        }
        _currentState.Tick();
    }

    private StateTransition CheckForTransition()
    {
        if (_anyStateTransitions.Count > 0)
        {
            foreach (var transition in _anyStateTransitions)
            {
                if (transition.Condition() == true)
                {
                    return transition;
                }
            }
        }
        
        if (_stateTransitons.ContainsKey(_currentState))
        {
            foreach (var transition in _stateTransitons[_currentState])
            {
                if (transition.Condition() == true)
                {
                    return transition;
                }
            }
        }

        return null;
    }

    public void AddState(IState state)
    {
        _states.Add(state);
    }

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if (_stateTransitons.ContainsKey(from) == false)
        {
            _stateTransitons[from] = new List<StateTransition>();
        }

        var transition = new StateTransition(from, to, condition);
        _stateTransitons[from].Add(transition);
    }

    public void AddAnyStateTransition(IState to, Func<bool> condition)
    {
        var transition = new StateTransition(null, to, condition);
        _anyStateTransitions.Add(transition);
    }

    public void SetState(IState state)
    {
        if (_currentState == state)
            return;
        
        _currentState?.OnExit();

        _currentState = state;
        Debug.Log( $"Statemachine changed to state: {state.GetType().Name}");
        
        _currentState.OnEnter();
    }
}