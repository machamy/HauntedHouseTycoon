
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleScreamCardAction")]
public class SimpleScreamCardActionSO : BaseCardActionSO
{
    
    public override bool OnGuestEnter(Room room, CardData cardData, Guest guest)
    {
        if (guest.CanScream)
        {
            guest.Scream();
        }
        return breakChain;
    }
}
