using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EventManager
{
    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private static Dictionary<Type, Delegate> _globalDelegates = new Dictionary<Type, Delegate>();

    public static void AddGlobalListener<T>(EventDelegate<T> listener) where T : GameEvent
    {
        Type eventType = typeof(T);
        if (!_globalDelegates.ContainsKey(eventType))
        {
            _globalDelegates[eventType] = listener;
        }
        else
        {
            _globalDelegates[eventType] = Delegate.Combine(_globalDelegates[eventType], listener);
        }
    }

    public static void RemoveGlobalListener<T>(EventDelegate<T> listener) where T : GameEvent
    {
        Type eventType = typeof(T);
        if (_globalDelegates.ContainsKey(eventType))
        {
            _globalDelegates[eventType] = Delegate.Remove(_globalDelegates[eventType], listener);
        }
    }

    public static void Raise<T>(T e) where T : GameEvent
    {
        Type eventType = typeof(T);
        if (_globalDelegates.ContainsKey(eventType))
        {
            (_globalDelegates[eventType] as EventDelegate<T>)?.Invoke(e);
        }
        e.RaiseLocal();
    }
}

public abstract class GameEvent
{
    public string Description;

    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private event EventDelegate<GameEvent> LocalListeners;

    public void AddLocalListener(EventDelegate<GameEvent> listener)
    {
        LocalListeners += listener;
    }

    public void RemoveLocalListener(EventDelegate<GameEvent> listener)
    {
        LocalListeners -= listener;
    }

    public void RaiseLocal()
    {
        LocalListeners?.Invoke(this);
    }
}

public class MyCustomEvent : GameEvent
{
    public int SomeValue;
}