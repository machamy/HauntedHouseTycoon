
using UnityEngine;

[CreateAssetMenu(menuName = "EventChannel/TurnEventChannel")]
public class TurnEventChannelSO : ScriptableObject
{
    public delegate void TurnEnterEvent();
    public delegate void TurnExitEvent();
    public event TurnEnterEvent OnPlayerTurnEnter;
    public event TurnExitEvent OnPlayerTurnExit;
    
    public event TurnEnterEvent OnNonPlayerTurnEnter;
    public event TurnExitEvent OnNonPlayerTurnExit;
    
    public void RaisePlayerTurnEnterEvent()
    {
        OnPlayerTurnEnter?.Invoke();
    }
    
    public void RaisePlayerTurnExitEvent()
    {
        OnPlayerTurnExit?.Invoke();
    }
    
    public void RaiseNonPlayerTurnEnterEvent()
    {
        OnNonPlayerTurnEnter?.Invoke();
    }
    
    public void RaiseNonPlayerTurnExitEvent()
    {
        OnNonPlayerTurnExit?.Invoke();
    }
}
