
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
    public Customer customer;
    public int modifier;
    public ScreamEventArg(Customer customer, int modifier)
    {
        this.customer = customer;
        this.modifier = modifier;
    }
}
