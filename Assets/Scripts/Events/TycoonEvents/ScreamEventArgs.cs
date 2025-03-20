public class ScreamEventArgs : EventArgs<ScreamEventArgs>
{
    public GuestObject GuestObject;
    public int modifier;


    public override void Clear()
    {
        GuestObject = null;
        modifier = 0;
    }
}