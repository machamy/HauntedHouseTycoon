
using UnityEngine;

[CreateAssetMenu(menuName = "CardAction/EntranceCardActionSO")]
public class EntranceCardActionSO : BaseCardActionSO
{
    
    [SerializeField] private Customer customerPrefab;
    public override bool OnTurnExit(Room room, CardData cardData)
    {
        
        int amount = cardData.GetArgumentInt(CardDataAgument.Key.CustomerAmount);
        Debug.Log($"[EntranceCardActionSO::OnTurnExit] {room.name} : Summon {amount} customers");
        for (int i = 0; i < amount; i++)
        {
            Customer customer = Instantiate(customerPrefab);
            customer.GetComponent<Entity>().MoveWithTransform(room);
        }
        return breakChain;
    }
}
