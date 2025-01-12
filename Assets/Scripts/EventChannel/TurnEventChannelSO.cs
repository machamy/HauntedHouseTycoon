
using UnityEngine;

[CreateAssetMenu(menuName = "EventChannel/TurnEventChannel")]
public class TurnEventChannelSO : ScriptableObject
{
    public delegate void TurnEnterEvent();
    public delegate void TurnExitEvent();
    public event TurnEnterEvent OnTurnEnter;
    public event TurnExitEvent OnTurnExit;
    
    public void RaiseTurnEnterEvent()
    {
        OnTurnEnter?.Invoke();
    }
    
    public void RaiseTurnExitEvent()
    {
        OnTurnExit?.Invoke();
    }
}
