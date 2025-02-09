

using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleDecayCardAction")]
public class SimpleDecayCardActionSO : BaseCardActionSO
{
    public override bool OnCardPlaced(Room room, CardData cardData)
    {
        int DecayTurnAmount = cardData.GetArgumentInt(CardDataAgument.Key.DecayTurnAmount);
        cardData.SetArgumentInt(CardDataAgument.Key.DecayTurnRemain, DecayTurnAmount);
        Debug.Log($"[SimpleDecayCardActionSO::OnCardPlaced] {room.name} : {DecayTurnAmount} turns remain");
        Debug.Log($"[SimpleDecayCardActionSO::OnCardPlaced] {room.name} : ({cardData.GetArgumentInt(CardDataAgument.Key.DecayTurnRemain)})");
        return breakChain;
    }

    public override bool OnNpcTurnExit(Room room, CardData cardData)
    {
        int decayTurnRemain = cardData.GetArgumentInt(CardDataAgument.Key.DecayTurnRemain);
        cardData.SetArgumentInt(CardDataAgument.Key.DecayTurnRemain, decayTurnRemain-1);
        if(decayTurnRemain <= 0)
        {
            Debug.Log($"[SimpleDecayCardActionSO::OnTurnExit] {room.name} : Card decayed({decayTurnRemain})");
            cardData.CleanAction();
            room.FocusColor(new Color(0.4f, 0.4f, 0.4f, 0.5f));
            return true;
        }
        return breakChain;
    }
}
