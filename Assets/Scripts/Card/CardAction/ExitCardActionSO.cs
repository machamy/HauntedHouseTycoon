
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/ExitCardAction")]
public class ExitCardActionSO : BaseCardActionSO
{
    public override bool OnGuestEnter(Room room, CardData cardData, Guest guest)
    {
        guest.Exit();
        return breakChain;
    }
}
