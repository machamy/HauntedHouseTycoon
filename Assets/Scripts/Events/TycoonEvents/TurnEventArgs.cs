
using Define;

public class TurnEventArgs : EventArgs<TurnEventArgs>
{
    public TurnType turnType;
    
    public override void Clear()
    {
        turnType = TurnType.None;
    }
}
