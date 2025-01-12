
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleFearCardAction")]
public class SimpleFearCardActionSO : BaseCardActionSO
{
    public override bool OnCustomerEnter(Room room, CardData cardData, Customer customer)
    {
        int fear = cardData.GetArgumentInt(CardDataAgument.Key.FearEnterAmount);
        customer.AddFear(fear);
        return breakChain;
    }
}

