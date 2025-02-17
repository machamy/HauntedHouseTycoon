﻿
using UnityEngine;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlacedCard placedCardPrefab;
    [SerializeField] private CardDataSO defaultCardData;
    [Header("References")]
    [SerializeField] private CardData originalCardData;
    [SerializeField] private CardData cardData;
    [SerializeField] private PlacedCard placedCard;
    [SerializeField] private SpriteRenderer focusRenderer;
    [Header("Properties")]
    [SerializeField] Vector2Int coordinate;
    [Header("Events")]
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    [SerializeField] private CustomerRoomEventSO roomEventChannelSo;
    [SerializeField] private ScreamEventChannelSO screamEventChannelSo;
    public Vector2Int Coordinate => coordinate;
    public CardData CardData => cardData;
    public void Init(Vector2Int position, CardData cardData)
    {
        this.coordinate = position;
        this.cardData = cardData;
        if(!placedCard)
            placedCard = Instantiate(placedCardPrefab);
        placedCard.transform.position = transform.position;
        var placedCardHolder = FindFirstObjectByType<PlacedCardHolder>();
        placedCard.transform.SetParent(placedCardHolder.transform);
        placedCard.room = this;
        UpdatePlacedCard();
    }

    public void UpdatePlacedCard()
    {
        placedCard.UpdateDisplay(cardData);
    }
    private void UpdatePlacedCard(Vector3 uiSize)
    {
        placedCard.UpdateDisplay(cardData,uiSize);
    }
    
    public bool PlaceCard(CardData cardData) => PlaceCard(cardData, Vector3.one);
    public bool PlaceCard(CardData cardData, Vector3 uiSize)
    {
        DiscardCard();
        originalCardData = cardData;
        this.cardData = cardData.Clone() as CardData;
        cardData.cardActionContainer.InvokeOnCardPlaced(this, this.cardData);
        UpdatePlacedCard(uiSize);
        return true;
    }
    
    public bool DiscardCard()
    {
        Debug.Log($"DiscardCard {name}");
        cardData.cardActionContainer.InvokeOnCardRemoved(this, cardData);
        cardData = defaultCardData.cardData;
        if(originalCardData != null && originalCardData.cardName != "Blank")
        {
            UnfocusColor();
            if (originalCardData.returnDeck != null)
                // originalCardData.returnDeck.AddCardToDiscardPool(cardData);
                // 버려진 카드는 그냥 버려짐
                ;
        }
        UpdatePlacedCard();
        return true;
    }
    
    private bool isFocused = false;
    public bool IsFocused => isFocused;
    
    public void FocusColor(Color color)
    {
        focusRenderer.color = color;
        focusRenderer.enabled = true;
    }
    
    public void UnfocusColor()
    {
        focusRenderer.enabled = false;
    }

    /// <summary>
    /// 해당 방에서, 들어온 방향을 기준으로 좌측우선 탐색을 통해 다음 방을 찾는다.
    /// 다음방도 들어온 방향을 가지고 있어야 한다.
    /// </summary>
    /// <param name="field"></param>
    /// <param name="currentRoom"></param>
    /// <param name="originDirection"></param>
    /// <returns></returns>
    public Room FindLeftmostRoom(Field field, Direction originDirection,out Direction targetDirection)
    {
        Direction targetDir = DirectionHelper.GetLeftmostDirection(originDirection, CardData.directions);
        Room nextRoom;
        int count = 0;
        while (targetDir != Direction.None && count++ < 4)
        {
            nextRoom = field.GetRoomByDirection(this, targetDir);
            if (nextRoom && nextRoom.CardData.directions.HasFlag(targetDir.Opposite().ToFlag()))
            {
                targetDirection = targetDir;
                return nextRoom;
            }
            targetDir = DirectionHelper.GetLeftmostDirection(targetDir, this.CardData.directions);
        }
        targetDirection = Direction.None;
        return null;
    }
    #region Event Handlers
    private void OnEnable()
    {
        turnEventChannelSo.OnPlayerTurnEnter += OnPlayerTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnEnter += OnNpcTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnExit += OnNpcTurnExit;
        roomEventChannelSo.OnCustomerRoomEnter += OnCustomerRoomEnter;
        roomEventChannelSo.OnCustomerRoomExit += OnCustomerRoomExit;
        screamEventChannelSo.OnScream += OnScream;
    }
    
    private void OnDisable()
    {
        turnEventChannelSo.OnPlayerTurnEnter -= OnPlayerTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnEnter -= OnNpcTurnEnter;
        turnEventChannelSo.OnNonPlayerTurnExit -= OnNpcTurnExit;
        roomEventChannelSo.OnCustomerRoomEnter -= OnCustomerRoomEnter;
        roomEventChannelSo.OnCustomerRoomExit -= OnCustomerRoomExit;
        screamEventChannelSo.OnScream -= OnScream;
    }

    private void OnPlayerTurnEnter()
    {
        if (cardData != null)
        {
            var tmp = cardData;
            // print($"OnTurnEnter {name}");
            cardData.cardActionContainer.InvokeOnPlayerTurnEnter(this, tmp);
        }
    }
    
    private void OnNpcTurnEnter()
    {
        if (cardData != null)
        {
            
            var tmp = cardData;
            cardData.cardActionContainer.InvokeOnNpcTurnEnter(this, tmp);
        }
    }
    
    private void OnNpcTurnExit()
    {
        if (cardData != null)
        {
            cardData.cardActionContainer.InvokeOnNpcTurnExit(this, cardData);
        }
    }
    
    private void OnCustomerRoomEnter(Guest guest, Room room)
    {
        if (room == this)
        {
            if (cardData != null)
            {
                cardData.cardActionContainer.InvokeOnCustomerEnter(this, cardData, guest);
            }
        }
    }
    
    private void OnCustomerRoomExit(Guest guest, Room room)
    {
        if (room == this)
        {
            if (cardData != null)
            {
                cardData.cardActionContainer.InvokeOnCustomerExit(this, cardData, guest);
            }
        }
    }
    
    private void OnScream(ScreamEventArg screamEventArg)
    {
        if (cardData != null)
        {
            int roomScreamModifier = cardData.GetArgumentIntDefault(CardDataAgument.Key.FearGlobalAmount,0);
            screamEventArg.modifier += roomScreamModifier;
        }
    }
    

    #endregion
}
