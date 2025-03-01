
using UnityEngine;


[CreateAssetMenu(menuName = "EventChannel/ScreamEventChannel")]
public class ScreamEventChannelSO : ScriptableObject
{
    public delegate void ScreamEvent(ScreamEventArg screamEventArg);
    public event ScreamEvent OnScream;
    public event ScreamEvent OnScreamModified;

    public void RaiseScreamEvent(ScreamEventArg screamEventArg)
    {
        OnScream?.Invoke(screamEventArg);
        OnScreamModified?.Invoke(screamEventArg);
    }    
}

public class ScreamEventArg
{
    public GuestObject GuestObject;
    public int modifier;
    public ScreamEventArg(GuestObject guestObject, int modifier)
    {
        this.GuestObject = guestObject;
        this.modifier = modifier;
    }
}
