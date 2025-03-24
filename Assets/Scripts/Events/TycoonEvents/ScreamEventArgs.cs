public class ScreamEventArgs : EventArgs<ScreamEventArgs>
{
    public GuestParty GuestParty;
    public int modifier;


    public override void Clear()
    {
        GuestParty = null;
        modifier = 0;
    }
}