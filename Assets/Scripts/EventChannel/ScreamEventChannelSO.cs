
using UnityEngine;


[CreateAssetMenu(menuName = "EventChannel/ScreamEventChannel")]
public class ScreamEventChannelSO : ScriptableObject
{
    public delegate void ScreamEvent(ScreamEventArgs screamEventArg);
    public event ScreamEvent OnScream;
    public event ScreamEvent OnScreamModified;

    public void RaiseScreamEvent(ScreamEventArgs screamEventArg)
    {
        OnScream?.Invoke(screamEventArg);
        OnScreamModified?.Invoke(screamEventArg);
    }    
}

