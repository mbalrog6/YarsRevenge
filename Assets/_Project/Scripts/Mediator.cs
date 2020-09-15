using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void MediatorCallback<T>(T command) where T : ICommand;
public class Mediator : MonoBehaviour
{
    private static Mediator _instance;
    public static Mediator Instance => _instance;
    
    private Dictionary<System.Type, System.Delegate> _subscribers = new Dictionary<Type,Delegate>();

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this; 
            DontDestroyOnLoad(this);
        }
    }

    public void Subscribe<T>(MediatorCallback<T> callback) where T : ICommand
    {
        if(callback == null) throw new System.ArgumentNullException("callback");
        var type = typeof(T);
        if (_subscribers.ContainsKey(type))
        {
            _subscribers[type] = System.Delegate.Combine(_subscribers[type], callback);
        }
        else
        {
            _subscribers.Add(type, callback);
        }
    }

    public void DeleteSubscriber<T>(MediatorCallback<T> callback) where T : ICommand
    {
        if(callback == null) throw new System.ArgumentNullException("callback");
        var type = typeof(T);
        if (_subscribers.ContainsKey(type))
        {
            var delegateFunc = _subscribers[type];
            delegateFunc = System.Delegate.Remove(delegateFunc, callback );
            if (delegateFunc == null)
            {
                _subscribers.Remove(type);
            }
            else
            {
                _subscribers[type] = delegateFunc;
            }
        }
    }

    public void Publish<T>(T command) where T : ICommand
    {
        var type = typeof(T);
        if (_subscribers.ContainsKey(type))
        {
            _subscribers[type].DynamicInvoke(command);
        }
    }
}
