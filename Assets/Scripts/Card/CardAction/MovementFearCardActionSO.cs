

using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/MovementFearCardAction")]
public class MovementFearCardActionSO : BaseCardActionSO
{
    public override bool OnCustomerEnter(Room room, CardData cardData, Customer customer)
    {
        int fearCoef = cardData.GetArgumentInt(CardDataAgument.Key.FearMovementCoefficient);
        customer.AddFear(fearCoef * customer.MovedDistance);
        return breakChain;
    }
}

