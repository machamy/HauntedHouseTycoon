
using System;
using System.Collections.Generic;

public abstract class EventArgs : System.EventArgs
{
    public abstract void Clear();
}
public abstract class EventArgs<T> : EventArgs, IDisposable where T : EventArgs<T>, new()
{
    private static Queue<T> pool = new Queue<T>();
    protected bool isArgsVaild = true;
    public bool IsArgsVaild => isArgsVaild;
    protected EventArgs()
    {
    }

    public static T Get()
    {
        var res = pool.Count > 0 ? pool.Dequeue() : new T();
        res.isArgsVaild = true;
        return res;
    }

    public void Release()
    {
        Clear();
        pool.Enqueue((T)this);
        isArgsVaild = false;
    }

    public void Dispose()
    {
        Release();
    }
}