
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleFearCardAction")]
public class SimpleFearCardActionSO : BaseCardActionSO
{
    public override bool OnGuestEnter(Room room, CardData cardData, Guest guest)
    {
        int fear = cardData.GetArgumentInt(CardDataAgument.Key.FearEnterAmount);
        guest.AddFear(fear);
        return breakChain;
    }
}

