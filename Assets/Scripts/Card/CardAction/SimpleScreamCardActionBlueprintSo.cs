
using UnityEngine;

[CreateAssetMenu (menuName = "CardAction/SimpleScreamCardAction")]
public class SimpleScreamCardActionBlueprintSo : CardActionBlueprintSO
{
    
    [System.Serializable]
    public class SimpleScreamCardAction : CardAction
    {
        public override bool OnGuestEnter(Room room, CardData cardData, GuestObject guestObject)
        {
            if (guestObject.CanScream)
            {
                guestObject.Scream();
            }
            return breakChain;
        }
    }
    
    
    public override CardAction CreateCardAction()
    {
        return new SimpleScreamCardAction();
    }
    
    public override System.Type GetCardActionType()
    {
        return typeof(SimpleScreamCardAction);
    }
    
}
