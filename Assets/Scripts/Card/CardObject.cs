using System;
using UnityEngine;


public class CardObject : MonoBehaviour
{ 
    [SerializeField] private CardDisplay cardDisplayPrefab;
    [Header("Data")]
    [SerializeField] private CardData cardData;
    public CardData CardData => cardData;
    [Header("References")] 
    public GameObject slotGO => transform.parent.gameObject;
    public Transform slotTransform => transform.parent;
    [SerializeReference] private CardSettingSO cardSetting;
    [SerializeField] private CardSelection cardSelection;
    [SerializeField] private CardDisplay cardDisplay;
    
    public event Action<CardObject> OnCardLeaveHand;
    
    public CardSelection CardSelection => cardSelection;
    public CardSetting CardSetting => cardSetting.Setting;
    public CardDisplay CardDisplay => cardDisplay;



    private void Awake()
    {
        var displayHolder = FindFirstObjectByType<CardDisplayHolder>();
        cardDisplay = Instantiate(cardDisplayPrefab,displayHolder.transform);
        cardDisplay.cardObject = this;
    }

    private void OnEnable()
    {
        cardDisplay.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        cardDisplay.gameObject.SetActive(false);
    }

    public void Initialize(CardData data, CardSettingSO cardSetting)
    {
        cardData = data;
        this.cardSetting = cardSetting;
        cardDisplay.UpdateDisplay();
    }

    public void LeaveHand()
    {
        OnCardLeaveHand?.Invoke(this);
    }
    
    public int GetIdx() => transform.parent.GetSiblingIndex();
}
