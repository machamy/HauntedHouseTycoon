using System;
using System.Collections.Generic;
using UnityEngine.Events;


public class PriorityEvent : PriorityEventBase
{
    private SortedList<int,Action> _events = new SortedList<int, Action>();
    
    public void AddListener(Action listener, int priority = 0)
    {
        if (_events.ContainsKey(priority))
        {
            _events[priority] += listener;
        }
        else
        {
            _events.Add(priority, listener);
        }
    }
    
    public void RemoveListener(Action listener, int priority)
    {
        if (_events.ContainsKey(priority))
        {
            _events[priority] -= listener;
        }
    }
    
    public void RemoveListener(Action listener)
    {
        foreach (var k in _events.Keys)
        {
            _events[k] -= listener;
        }
    }
    
    public void ClearListeners(int priority)
    {
        if (_events.ContainsKey(priority))
        {
            _events.Remove(priority);
        }
    }
    
    public void ClearListeners()
    {
        _events.Clear();
    }
    
    public void Invoke()
    {
        foreach (Action e in _events.Values)
        {
            e?.Invoke();
        }
    }
}
