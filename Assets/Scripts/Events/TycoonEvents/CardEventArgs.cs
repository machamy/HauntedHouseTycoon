
public class CardEventArgs : EventArgs<CardEventArgs>
{
    public int usedCost;
    public Room room;
    public CardData cardData;

    public override void Clear()
    {
        usedCost = 0;
        room = null;
        cardData = null;
    }
}
