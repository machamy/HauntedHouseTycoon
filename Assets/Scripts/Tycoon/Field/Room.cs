
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlacedCard placedCardPrefab;
    [SerializeField] private CardDataSO defaultCardData;
    [Header("References")]
    [SerializeField] private CardData originalCardData = null;
    [SerializeField] private CardData cardData = new CardData();
    [SerializeField] private PlacedCard placedCard;
    [SerializeField] private SpriteRenderer focusRenderer;
    [Header("Properties")]
    [SerializeField] Vector2Int coordinate;
    [Header("Entities")]
    [SerializeField] private List<Entity> entities = new List<Entity>();
    // [SerializeField] List<GuestObject> guests = new List<GuestObject>();
    [Header("Events")]
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    [SerializeField] private GuestRoomEventSO roomEventChannelSo;
    [SerializeField] private ScreamEventChannelSO screamEventChannelSo;
    public Vector2Int Coordinate => coordinate;
    public CardData CardData => cardData;

    private CardUseArea cardUseArea;
    public void Init(Vector2Int position, CardData cardData)
    {
        this.coordinate = position;
        originalCardData = cardData;
        this.cardData.CopyFrom(cardData);
        if(!placedCard)
            placedCard = Instantiate(placedCardPrefab);
        placedCard.transform.position = transform.position;
        var placedCardHolder = FindFirstObjectByType<PlacedCardHolder>();
        placedCard.transform.SetParent(placedCardHolder.transform);
        placedCard.room = this;
        UpdatePlacedCard();
    }

    private void Awake()
    {
        cardUseArea = GetComponentInChildren<CardUseArea>();
        cardUseArea.parent = this;
    }

    public void UpdatePlacedCard()
    {
        placedCard.UpdateDisplay(cardData);
        focusRenderer.sprite = cardData.cardSprite;
    }
    private void UpdatePlacedCard(Vector3 startSize)
    {
        placedCard.UpdateDisplay(cardData,startSize);
        focusRenderer.sprite = cardData.cardSprite;
    } 


    
    public bool PlaceCard(CardData cardData) => PlaceCard(cardData, Vector3.one);
    public bool PlaceCard(CardData cardData, Vector3 uiSize)
    {
        DiscardCard();
        originalCardData = cardData;
        this.cardData.CopyFrom(cardData);
        using (CardEventArgs placeEvent = CardEventArgs.Get())
        {
            placeEvent.room = this;
            placeEvent.cardData = cardData;
            cardData.OnCardPlaced(placeEvent);
            cardData.cardActionContainer.InvokeOnCardPlaced(placeEvent);
        }
        UpdatePlacedCard(uiSize);
        return true;
    }
    
    public bool DiscardCard()
    {
        Debug.Log($"DiscardCard {name}");
        using (CardEventArgs discardEvent = CardEventArgs.Get())
        {
            discardEvent.room = this;
            discardEvent.cardData = cardData;
            cardData.cardActionContainer.InvokeOnCardRemoved(discardEvent);
        }
        CardDataPool.Instance.Release(cardData);
        cardData = CardDataPool.Instance.Get();
        cardData.CopyFrom(defaultCardData.OriginalCardData);
        
        if(originalCardData != null && originalCardData.cardName != "Blank")
        {
            Unfocus();
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
    
    public void FocusSprite(Sprite sprite, Color color)
    {
        focusRenderer.sprite = sprite;
        focusRenderer.color = color;
        focusRenderer.enabled = true;
        isFocused = true;
    }
    
    public void FocusColor(Color color)
    {
        focusRenderer.color = color;
        focusRenderer.enabled = true;
    }
    
    public void Unfocus()
    {
        focusRenderer.enabled = false;
    }
    
    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }
    
    public bool HasEntity(Entity entity)
    {
        return entities.Contains(entity);
    }
    public void RemoveEntity(Entity entity)
    {
        entities.Remove(entity);
    }
    
    public Entity FindEntity(Predicate<Entity> match)
    {
        return entities.Find(match);
    }

    /// <summary>
    /// 해당 방에서, 들어온 방향을 기준으로 좌측우선 탐색을 통해 다음 방을 찾는다.
    /// 다음방도 들어온 방향을 가지고 있어야 한다.
    /// </summary>
    /// <param name="field"></param>
    /// <param name="orientingDirection">바라보는 방향(진입 경로의 반대)</param>
    /// <param name="targetDirection"></param>
    /// <returns></returns>
    public Room FindLeftmostRoom(Field field, Direction orientingDirection,out Direction targetDirection)
    {
        Direction targetDir = DirectionExtentions.GetLeftmostDirection(orientingDirection, CardData.directions);
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
            targetDir = targetDir.GetFirstClockwiseDirection(CardData.directions);
        }
        targetDirection = Direction.None;
        return null;
    }
    #region Event Handlers
    private void OnEnable()
    {
        turnEventChannelSo.OnPlayerTurnEnterEvent.AddListener(OnPlayerTurnEnter);
        turnEventChannelSo.OnNonPlayerTurnEnterEvent.AddListener(OnNpcTurnEnter);
        turnEventChannelSo.OnNonPlayerTurnExitEvent.AddListener(OnNpcTurnExit);
        roomEventChannelSo.OnGuestRoomEnter.AddListener(OnCustomerRoomEnter);
        roomEventChannelSo.OnGuestRoomExit.AddListener(OnCustomerRoomExit);
        screamEventChannelSo.OnScream += OnScream;
        if (cardUseArea)
        {
            cardUseArea.OnCardUse += OnCardUse;
            // cardUseArea.OnCardUseAreaEnter += OnCardUseAreaEnter;
            // cardUseArea.OnCardUseAreaExit += OnCardUseAreaExit;
        }
    }
    
    private void OnDisable()
    {
        turnEventChannelSo.OnNonPlayerTurnEnterEvent.RemoveListener(OnPlayerTurnEnter);
        turnEventChannelSo.OnNonPlayerTurnEnterEvent.RemoveListener(OnNpcTurnEnter);
        turnEventChannelSo.OnNonPlayerTurnExitEvent.RemoveListener(OnNpcTurnExit);
        roomEventChannelSo.OnGuestRoomEnter.RemoveListener(OnCustomerRoomEnter);
        roomEventChannelSo.OnGuestRoomExit.RemoveListener(OnCustomerRoomExit);
        screamEventChannelSo.OnScream -= OnScream;
        if (cardUseArea)
        {
            cardUseArea.OnCardUse -= OnCardUse;
            // cardUseArea.OnCardUseAreaEnter -= OnCardUseAreaEnter;
            // cardUseArea.OnCardUseAreaExit -= OnCardUseAreaExit;
        }
    }

    private void OnPlayerTurnEnter(TurnEventArgs e)
    {
        if (cardData != null)
        {
            // print($"OnTurnEnter {name}");
            cardData.cardActionContainer.InvokeOnPlayerTurnEnter(e);
        }
    }
    
    private void OnNpcTurnEnter(TurnEventArgs e)
    {
        // print($"OnNpcTurnEnter {name}");
        if (cardData != null)
        {
            cardData.cardActionContainer.InvokeOnNpcTurnEnter(e);
        }
    }
    
    private void OnNpcTurnExit(TurnEventArgs e)
    {
        if (cardData != null)
        {
            cardData.cardActionContainer.InvokeOnNpcTurnExit(e);
        }
    }
    
    private void OnCustomerRoomEnter(GuestMoveEventArgs e)
    {
        if (e.isEnter && e.toRoom == this)
        {
            if (cardData != null)
            {
                cardData.cardActionContainer.InvokeOnGuestEnter(e);
            }
        }
    }
    
    private void OnCustomerRoomExit(GuestMoveEventArgs e)
    {
        if (!e.isEnter && e.fromRoom == this)
        {
            if (cardData != null)
            {
                cardData.cardActionContainer.InvokeOnGuestExit(e);
            }
        }
    }
    
    private void OnScream(ScreamEventArgs screamEventArg)
    {
        if (cardData != null)
        {
            int roomScreamModifier = cardData.nonlocalVariables.GetIntDefault(CardDataVariables.Key.FearGlobalAmount,0);
            screamEventArg.modifier += roomScreamModifier;
        }
    }
    
    public void OnCardUse(CardUseArea cardUseArea, CardData cardData)
    {
        PlaceCard(cardData);
    }
    
    // Color focusColor = new Color(0.5f,1,0.0f,0.5f);
    // public void OnCardUseAreaEnter(CardUseArea cardUseArea, CardData cardData)
    // {
    //     FocusSprite(cardData.cardSprite, focusColor);
    // }
    //
    // public void OnCardUseAreaExit(CardUseArea cardUseArea, CardData cardData)
    // {
    //     Unfocus();
    // }

    #endregion
}
