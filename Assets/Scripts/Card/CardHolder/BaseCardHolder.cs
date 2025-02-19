using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class BaseCardHolder : MonoBehaviour
{
    /// <summary>
    /// 비활성화 될시 비활성화 될 오브젝트
    /// null 이면 자기 자신이 비활성화 됨
    /// 현재 구현은 BaseCardSellectableHolder에서만 사용함
    /// </summary>
    [SerializeField] protected GameObject parent;

    [SerializeField] private GameObject slotHolder;
    [SerializeField] private CardSettingSO cardSetting;
    [SerializeField] private GameObject cardobjectPrefab;
    [SerializeField] private GameObject cardSlotPrefab;
    [Header("Card Setting")]
    [SerializeField] protected float cardWidth => cardSetting.Setting.slotWidth;
    [SerializeField] protected float cardHeight => cardSetting.Setting.slotHeight;
    [SerializeField] protected float cardGap = 0f;
    protected List<CardObject> cardObjects = new List<CardObject>();
    public int CardCount => cardObjects.Count;
    
    public virtual int MaxVisibleCardAmount => CurrentVisibleCardAmount;
    public virtual int VisibleStartIdx => 0;
    public virtual int VisibleEndIdx => cardObjects.Count - 1;
    public virtual int CurrentVisibleCardAmount => cardObjects.Count;

    // /// <summary>
    // /// 해당 카드들로 카드를 초기화함. 필수아님
    // /// </summary>
    // /// <param name="initialCards"></param>
    // public virtual void Initialize(List<CardMetaData> initialCards)
    // {
    //     foreach (var co in cardObjects)
    //     {
    //         Destroy(co.CardDisplay.gameObject);
    //         if(co.transform.parent.CompareTag("Slot"))
    //             Destroy(co.transform.parent.gameObject);
    //         else
    //             Destroy(co.gameObject);
    //     }
    //     cardObjects = new List<CardObject>();
    //     foreach (var cardData in initialCards)
    //     {
    //         AddCardWithSlot(cardData);
    //     }
    // }
    
    public virtual void Enable()
    {
        if(parent != null)
            parent.SetActive(true);
        else
            gameObject.SetActive(true);

        foreach (var co in cardObjects)
        {
            co.CardDisplay.gameObject.SetActive(true);
        }
    }
    
    public virtual void Disable()
    {
        if(parent != null)
            parent.SetActive(false);
        else
            gameObject.SetActive(false);
        foreach (var co in cardObjects)
        {
            co.CardDisplay.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 슬롯과 함께 카드를 추가함.
    /// </summary>
    /// <param name="cardData">생성된 카드 오브젝트</param>
    /// <returns></returns>
    public virtual CardObject AddCardWithSlot(CardData cardData)
    {
        var slot = Instantiate(cardSlotPrefab, slotHolder.transform);
        var cardObject = slot.GetComponentInChildren<CardObject>();
        slot.transform.localScale = Vector3.one;
        var rectTransform = slot.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(cardWidth,cardHeight);
        cardObjects.Add(cardObject);
        cardObject.CardHolder = this;
        cardObject.Initialize(cardData,cardSetting);
        cardObject.CardSelection.OnCardPointerEnter += OnFocusBase;
        cardObject.CardSelection.OnCardPointerExit += OnUnfocusBase;
        cardObject.CardSelection.OnCardDragStart += OnCardDraggStartBase;
        cardObject.CardSelection.OnCardDragEnd += OnCardDraggingBase;
        cardObject.CardSelection.OnCardDragEnd += OnCardDragEndBase;

        cardObject.CardSelection.OnCardPointerDown += OnCardPointerDownBase;
        cardObject.CardSelection.OnCardPointerUp += OnCardPointerUpBase;
        
        cardObject.OnCardLeaveHand += OnCardLeaveHandBase;
        
        return cardObject;
    }
    
    
    /// <summary>
    /// 카드를 슬롯과 홀더에서 빼내고, 슬롯을 삭제한다.
    /// </summary>
    /// <param name="cardObject"></param>
    public virtual void RemoveCardFromHolder(CardObject cardObject)
    {
        if (!cardObjects.Contains(cardObject))
        {
            Debug.LogWarning("해당 카드는 존재하지 않습니다.");
            return;
        }
        GameObject slot = cardObject.transform.parent.gameObject;
        cardObjects.Remove(cardObject);
        cardObject.CardSelection.OnCardPointerEnter -= OnFocusBase;
        cardObject.CardSelection.OnCardPointerExit -= OnUnfocusBase;
        cardObject.CardSelection.OnCardDragStart -= OnCardDraggStartBase;
        cardObject.CardSelection.OnCardDragEnd -= OnCardDraggingBase;
        cardObject.CardSelection.OnCardDragEnd -= OnCardDragEndBase;

        cardObject.CardSelection.OnCardPointerDown -= OnCardPointerDownBase;
        cardObject.CardSelection.OnCardPointerUp -= OnCardPointerUpBase;
        
        cardObject.OnCardLeaveHand -= OnCardLeaveHandBase;
        
        cardObject.transform.SetParent(transform.parent);
        Destroy(slot);
    }

    /// <summary>
    /// 카드와 슬롯을 함께 삭제한다.
    /// </summary>
    /// <param name="cardObject"></param>
    public void DestoryCard(CardObject cardObject)
    {
        // int index = cardObjects.IndexOf(cardObject);
        var parent = cardObject.transform.parent;
        Destroy(cardObject.CardDisplay.gameObject);
        if(parent.CompareTag("Slot"))
            Destroy(parent.gameObject);
        else
            Destroy(cardObject.gameObject);
        
    }
    
    public CardObject this[int index]
    {
        get => cardObjects[index];
    }
    
    /// <summary>
    /// CardSlot 순서에 따라 Display 순서를 배치
    /// </summary>
    public void UpdateAllCardIndex()
    {
        foreach (var cardObject in cardObjects)
        {
            var cardDisplay = cardObject.CardDisplay;
            cardDisplay.UpdateIndex();
        }
    }
    #region Event Handlers
    

    
    protected virtual void OnFocus(CardSelection cardSelect)
    {
    }
    
    protected virtual void OnUnfocus(CardSelection cardSelect)
    {
        
    }

    protected virtual void OnCardDraggStart(CardSelection cardSelect)
    {
        
    }
    
    protected virtual void OnCardDragging(CardSelection cardSelect)
    {
        
    }
    
    protected virtual void OnCardDragEnd(CardSelection cardSelect)
    {
        
    }
    
    protected virtual void OnCardLeaveHand(CardObject cardObject)
    {
        RemoveCardFromHolder(cardObject);
        StartCoroutine(CheckForDestoryCardObject(cardObject));
        IEnumerator CheckForDestoryCardObject(CardObject cardObject)
        {
            CardDisplay cardDisplay = cardObject.CardDisplay;
            if(cardObject.CardSetting.forceStopAnimation)
                cardDisplay.StopAllDOTweens();
            yield return new WaitWhile(() => DOTween.IsTweening(cardObject.transform) || cardDisplay.IsAnimating);
            Destroy(cardDisplay.gameObject);
            Destroy(cardObject.gameObject);
        }
    }
    
    protected virtual void OnCardPointerDown(CardSelection cardSelect)
    {
        
    }
    
    protected virtual void OnCardPointerUp(CardSelection cardSelect,bool isClick)
    {
        
    }
    
    private void OnFocusBase (PointerEventData eventData, CardSelection cardSelect) => OnFocus(cardSelect);
    private void OnUnfocusBase (PointerEventData eventData, CardSelection cardSelect) => OnUnfocus(cardSelect);
    private void OnCardDraggStartBase (PointerEventData eventData, CardSelection cardSelect) => OnCardDraggStart(cardSelect);
    private void OnCardDraggingBase (PointerEventData eventData, CardSelection cardSelect) => OnCardDragging(cardSelect);
    private void OnCardDragEndBase (PointerEventData eventData, CardSelection cardSelect) => OnCardDragEnd(cardSelect);
    private void OnCardLeaveHandBase(CardObject cardSelect) => OnCardLeaveHand(cardSelect);
    
    private void OnCardPointerDownBase(PointerEventData eventData, CardSelection cardSelect) => OnCardPointerDown(cardSelect);
    
    private void OnCardPointerUpBase(PointerEventData eventData, CardSelection cardSelect,bool isClick) => OnCardPointerUp(cardSelect,isClick);
    
    #endregion

    
}
