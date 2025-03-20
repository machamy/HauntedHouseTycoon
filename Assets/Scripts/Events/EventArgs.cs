
using System.Collections.Generic;

public abstract class EventArgs : System.EventArgs
{
    public abstract void Clear();
}
public abstract class EventArgs<T> : EventArgs where T : EventArgs<T>, new()
{
    private static Queue<T> pool = new Queue<T>();

    public static T Get()
    {
        return pool.Count > 0 ? pool.Dequeue() : new T();
    }

    public void Release()
    {
        Clear();
        pool.Enqueue((T)this);
    }
}