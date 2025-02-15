
public class TurnEndOnPlacedActionSO : BaseCardActionSO
{

    public override bool OnCardPlaced(Room room, CardData cardData)
    {
        if (room == null)
            return false;
        var tm = TycoonManager.Instance.TurnManager;
        if (tm.IsPlayerTurn)
        {
            tm.ReadyToPlayerTurnEnd();
        }
        return false;
    }
}   
