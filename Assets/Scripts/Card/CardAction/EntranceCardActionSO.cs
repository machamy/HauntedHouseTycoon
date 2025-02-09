
using System;
using EventChannel;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "CardAction/EntranceCardActionSO")]
public class EntranceCardActionSO : BaseCardActionSO
{
    
    [SerializeField] private CreateGuestChannelSO createGuestChannel;
    public override bool OnNpcTurnEnter(Room room, CardData cardData)
    {
        
        int amount = cardData.GetArgumentInt(CardDataAgument.Key.CustomerAmount);
        Debug.Log($"[EntranceCardActionSO::OnTurnExit] {room.name} : Summon {amount} customers");
        for (int i = 0; i < amount; i++)
        {
            Guest guest = createGuestChannel.RaiseCreateGuest(room.transform.position);
            guest.GetComponent<Entity>().MoveWithTransform(room);
        }
        return breakChain;
    }


}
