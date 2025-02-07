
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "CardAction/EntranceCardActionSO")]
public class EntranceCardActionSO : BaseCardActionSO
{
    
    [FormerlySerializedAs("customerPrefab")] [SerializeField] private Guest guestPrefab;
    public override bool OnTurnExit(Room room, CardData cardData)
    {
        
        int amount = cardData.GetArgumentInt(CardDataAgument.Key.CustomerAmount);
        Debug.Log($"[EntranceCardActionSO::OnTurnExit] {room.name} : Summon {amount} customers");
        for (int i = 0; i < amount; i++)
        {
            Guest guest = Instantiate(guestPrefab);
            guest.GetComponent<Entity>().MoveWithTransform(room);
        }
        return breakChain;
    }
}
