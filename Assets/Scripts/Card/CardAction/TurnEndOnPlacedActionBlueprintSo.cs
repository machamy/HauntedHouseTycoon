
public class TurnEndOnPlacedActionBlueprintSo : CardActionBlueprintSO
{

    [System.Serializable]
    public class TurnEndOnPlacedAction : CardAction
    {
        public override bool OnCardPlaced(CardEventArgs cardEvent)
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
    
    
    public override CardAction CreateCardAction()
    {
        return new TurnEndOnPlacedAction();
    }
    
    public override System.Type GetCardActionType()
    {
        return typeof(TurnEndOnPlacedAction);
    }
}   
