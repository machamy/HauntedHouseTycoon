
public class GuestMoveEventArgs : EventArgs<GuestMoveEventArgs>
{
    public GuestObject guestObject;
    public Room fromRoom;
    // public Room currentRoom;
    public Room toRoom;
    public bool isEnter;

    
    public override void Clear()
    {
        guestObject = null;
        fromRoom = null;
        // currentRoom = null;
        toRoom = null;
        isEnter = false;
    }
}
