
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
        UpdateCard();
    }

    private void UpdateCard()
    {
        placedCard.UpdateDisplay(cardData);
    }
    private void UpdateCard(Vector3 uiSize)
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
        UpdateCard(uiSize);
        return true;
    }
    
    public bool DiscardCard()
    {
        Debug.Log($"DiscardCard {name}");
        cardData.cardActionContainer.InvokeOnCardRemoved(this, cardData);
        cardData = defaultCardData.cardData;
        if(originalCardData != null && originalCardData.cardName != "Blank")
        {
            if (originalCardData.returnDeck != null)
                // originalCardData.returnDeck.AddCardToDiscardPool(cardData);
                // TODO : 버려진 카드는 어디로??
                ;
        }
        UpdateCard();
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

    #region Event Handlers
    private void OnEnable()
    {
        turnEventChannelSo.OnPlayerTurnEnter += OnPlayerTurnEnter;
        turnEventChannelSo.OnPlayerTurnExit += OnPlayerTurnExit;
        roomEventChannelSo.OnCustomerRoomEnter += OnCustomerRoomEnter;
        roomEventChannelSo.OnCustomerRoomExit += OnCustomerRoomExit;
        screamEventChannelSo.OnScream += OnScream;
    }
    
    private void OnDisable()
    {
        turnEventChannelSo.OnPlayerTurnEnter -= OnPlayerTurnEnter;
        turnEventChannelSo.OnPlayerTurnExit -= OnPlayerTurnExit;
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
            cardData.cardActionContainer.InvokeOnTurnEnter(this, tmp);
        }
    }
    
    private void OnPlayerTurnExit()
    {
        if (cardData != null)
        {
            // print($"OnTurnExit {name}");
            var tmp = cardData;
            cardData.cardActionContainer.InvokeOnTurnExit(this, tmp);
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
            int roomScreamModifier = cardData.GetArgumentInt(CardDataAgument.Key.FearGlobalAmount);
            screamEventArg.modifier += roomScreamModifier;
        }
    }
    

    #endregion
}
