
using System;
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/ExitCardAction")]
public class ExitCardActionBlueprintSo : CardActionBlueprintSO
{
    [Serializable]
    public class ExitCardAction : CardAction
    {
        public override bool OnGuestEnter(Room room, CardData cardData, Guest guest)
        {
            guest.Exit();
            return breakChain;
        }
    }

    public override CardAction CreateCardAction()
    {
        return new ExitCardAction();
    }

    public override Type GetCardActionType()
    {
        return typeof(ExitCardAction);
    }
}
