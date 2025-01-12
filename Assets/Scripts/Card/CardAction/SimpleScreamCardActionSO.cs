
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleScreamCardAction")]
public class SimpleScreamCardActionSO : BaseCardActionSO
{
    
    public override bool OnCustomerEnter(Room room, CardData cardData, Customer customer)
    {
        if (customer.CanScream)
        {
            customer.Scream();
        }
        return breakChain;
    }
}
