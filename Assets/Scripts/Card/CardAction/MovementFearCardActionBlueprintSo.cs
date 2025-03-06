

using System;
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/MovementFearCardAction")]
public class MovementFearCardActionBlueprintSo : CardActionBlueprintSO
{
    [Serializable]
    public class EntranceAction : CardAction
    {
        public int FearMovementCoefficient = 2;
        
        public override bool OnGuestEnter(Room room, CardData cardData, GuestObject guestObject)
            {
                int fearCoef = FearMovementCoefficient;
                guestObject.AddFearSimple(fearCoef * guestObject.MovedDistance);
                return breakChain;
            }
    }
    
    public override CardAction CreateCardAction()
    {
        return new EntranceAction();
    }
    
    public override Type GetCardActionType()
    {
        return typeof(EntranceAction);
    }
    
}

