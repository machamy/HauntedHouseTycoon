using System;
using Pools;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Poolable))]
public class CardObject : MonoBehaviour
{ 
    [FormerlySerializedAs("cardHolder")] [SerializeField] public BaseCardHolder CardHolder;
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


    private Poolable poolable;
    private void Awake()
    {
        var displayHolder = FindFirstObjectByType<CardDisplayHolder>();
        poolable = GetComponent<Poolable>();
        poolable.OnRelease += () =>
        {
            cardDisplay = PoolManager.Instance.Get(PoolManager.Poolables.CardDisplay).GetComponent<CardDisplay>();
            cardDisplay.cardObject = this;
            cardDisplay.transform.SetParent(displayHolder.transform);
        };
        cardDisplay = PoolManager.Instance.Get(PoolManager.Poolables.CardDisplay).GetComponent<CardDisplay>();
        cardDisplay.cardObject = this;
        cardDisplay.transform.SetParent(displayHolder.transform);
        // visibleIdx = GetRawIdx();
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
        cardDisplay.InitializeDisplay();
    }

    public void LeaveHand()
    {
        OnCardLeaveHand?.Invoke(this);
    }
    
    public int GetRawIdx() => transform.parent.GetSiblingIndex();

    public int GetVisibleIdx()
    {
        if (CardHolder == null)
        {
            return GetRawIdx();
        }

        return GetRawIdx() - CardHolder.VisibleStartIdx;
    }

}
