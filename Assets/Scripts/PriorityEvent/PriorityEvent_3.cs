using System;
using System.Collections.Generic;
using UnityEngine.Events;


public class PriorityEvent<T0,T1,T2> : PriorityEventBase
{
    private SortedList<int,Action<T0,T1,T2>> _events = new SortedList<int, Action<T0,T1,T2>>();
    
    public void AddListener(Action<T0,T1,T2> listener, int priority= 0)
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
    
    public void RemoveListener(Action<T0,T1,T2> listener, int priority)
    {
        if (_events.ContainsKey(priority))
        {
            _events[priority] -= listener;
        }
    }
    
    public void RemoveListener(Action<T0,T1,T2> listener)
    {
        var keys = new List<int>(_events.Keys);
        foreach (var k in keys)
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
    
    public void Invoke(T0 arg,T1 arg1,T2 arg2)
    {
        foreach (Action<T0,T1,T2> e in _events.Values)
        {
            e?.Invoke(arg,arg1,arg2);
        }
    }
}
