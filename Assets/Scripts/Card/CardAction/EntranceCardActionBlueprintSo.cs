
using System;
using System.Collections.Generic;
using EventChannel;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "CardAction/EntranceCardActionSO")]
public class EntranceCardActionBlueprintSo : CardActionBlueprintSO
{
    [Serializable]
    public class EntranceAction : CardAction
    {
        [SerializeField] public CreateGuestChannelSO createGuestChannel;
        [SerializeField] public bool isRandomDirection = false;
        [SerializeField] public bool clockwise = true;
        public int customerAmount = 1;
        public Direction prevGuestDirection = Direction.None;
        
        public override bool OnNpcTurnEnter(Room room, CardData cardData)
        {
            int amount = customerAmount;
            Debug.Log($"[EntranceCardActionSO::OnTurnExit] {room.name} : Summon {amount} customers");

            DirectionFlag candidates = cardData.directions;
            List<Direction> directions = candidates.ToList();
        
            Direction dir = prevGuestDirection;
        
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
                GuestObject guestObject = createGuestChannel.RaiseCreateGuest(room.transform.position);
                guestObject.OrientingDirection = dir;
                guestObject.GetComponent<Entity>().MoveWithTransform(room);
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
    
    [SerializeField] private CreateGuestChannelSO createGuestChannel;
    [SerializeField] private bool isRandomDirection = false;
    [SerializeField] private bool clockwise = true;
    
    
    public override CardAction CreateCardAction()
    {
        return new EntranceAction()
        {
            createGuestChannel = createGuestChannel,
            isRandomDirection = isRandomDirection,
            clockwise = clockwise
        };
    }
    
    public override Type GetCardActionType()
    {
        return typeof(EntranceAction);
    }
}
