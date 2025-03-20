
using UnityEngine;

[CreateAssetMenu(menuName = "EventChannel/TurnEventChannel")]
public class TurnEventChannelSO : ScriptableObject
{
    public delegate void TurnEnterEvent(TurnEventArgs args);
    public delegate void TurnExitEvent(TurnEventArgs args);
    
    public PriorityEvent<TurnEventArgs> OnPlayerTurnEnterEvent = new PriorityEvent<TurnEventArgs>();
    public PriorityEvent<TurnEventArgs> OnPlayerTurnExitEvent = new PriorityEvent<TurnEventArgs>();
    public PriorityEvent<TurnEventArgs> OnNonPlayerTurnEnterEvent = new PriorityEvent<TurnEventArgs>();
    public PriorityEvent<TurnEventArgs> OnNonPlayerTurnExitEvent = new PriorityEvent<TurnEventArgs>();
    
    public void RaisePlayerTurnEnterEvent(TurnEventArgs args)
    {
        OnPlayerTurnEnterEvent?.Invoke(args);
    }
    
    public void RaisePlayerTurnExitEvent(TurnEventArgs args)
    {
        OnPlayerTurnExitEvent?.Invoke(args);
    }
    
    public void RaiseNonPlayerTurnEnterEvent(TurnEventArgs args)
    {
        OnNonPlayerTurnEnterEvent?.Invoke(args);
    }
    
    public void RaiseNonPlayerTurnExitEvent(TurnEventArgs args)
    {
        OnNonPlayerTurnExitEvent?.Invoke(args);
    }
}
