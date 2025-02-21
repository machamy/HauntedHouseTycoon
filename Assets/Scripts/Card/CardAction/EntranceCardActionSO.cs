
using System.Collections.Generic;
using EventChannel;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "CardAction/EntranceCardActionSO")]
public class EntranceCardActionSO : BaseCardActionSO
{
    
    [SerializeField] private CreateGuestChannelSO createGuestChannel;
    [SerializeField] private bool isRandomDirection = false;
    [SerializeField] private bool clockwise = true;
    public override bool OnNpcTurnEnter(Room room, CardData cardData)
    {
        int amount = cardData.GetArgumentInt(CardDataAgument.Key.CustomerAmount);
        Debug.Log($"[EntranceCardActionSO::OnTurnExit] {room.name} : Summon {amount} customers");

        DirectionFlag candidates = cardData.directions;
        List<Direction> directions = candidates.ToList();
        
        Direction dir = (Direction)cardData
            .GetArgumentIntDefault
                (CardDataAgument.Key.PrevGuestDirection, (int)Direction.None);
        
        if (isRandomDirection && directions.Count > 0)
        {
            dir = directions[Random.Range(0, directions.Count)];
        }
        else if (dir == Direction.None && directions.Count > 0) 
        {
            dir = directions[0];
        }
        
        for (int i = 0; i < amount; i++)
        {
            Guest guest = createGuestChannel.RaiseCreateGuest(room.transform.position);
            guest.OrientingDirection = dir;
            guest.GetComponent<Entity>().MoveWithTransform(room);
            if (isRandomDirection)
            {
                dir = directions[Random.Range(0, directions.Count)];
            }
            else
            {
                dir = clockwise ?
                    DirectionExtentions.GetClockwiseDirection(dir, candidates) 
                    : DirectionExtentions.GetCounterClockwiseDirection(dir, candidates);
            }
        }
        return breakChain;
    }



}
