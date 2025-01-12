
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/ExitCardAction")]
public class ExitCardActionSO : BaseCardActionSO
{
    public override bool OnCustomerEnter(Room room, CardData cardData, Customer customer)
    {
        customer.Exit();
        return breakChain;
    }
}
