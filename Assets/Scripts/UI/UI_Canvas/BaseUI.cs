
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();


    private void Awake()
    {
        // _uiManager = UIManager.Instance;
    }
    

    public abstract void Init();
    
    
    
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        if(_objects.TryGetValue(typeof(T), out var objects))
        {
            return objects[idx] as T;
        }
        return null;
    }
}
