
public class GuestMoveEventArgs : EventArgs<GuestMoveEventArgs>
{
    public GuestParty GuestParty;
    public Room fromRoom;
    // public Room currentRoom;
    public Room toRoom;
    public bool isEnter;

    
    public override void Clear()
    {
        GuestParty = null;
        fromRoom = null;
        // currentRoom = null;
        toRoom = null;
        isEnter = false;
    }
}
