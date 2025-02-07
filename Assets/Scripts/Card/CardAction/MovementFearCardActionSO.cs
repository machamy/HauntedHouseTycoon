

using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/MovementFearCardAction")]
public class MovementFearCardActionSO : BaseCardActionSO
{
    public override bool OnCustomerEnter(Room room, CardData cardData, Guest guest)
    {
        int fearCoef = cardData.GetArgumentInt(CardDataAgument.Key.FearMovementCoefficient);
        guest.AddFear(fearCoef * guest.MovedDistance);
        return breakChain;
    }
}

