
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class CardDisplay : MonoBehaviour
{
    public CardObject cardObject;
    private CardSetting CardSetting => cardObject.CardSetting;
    private CardSelection _cardSelection;
    
    [SerializeField] private Image cardImage;
    public Vector3 CardImageSize => cardImage.rectTransform.sizeDelta;
    private RectTransform rectTransform;
    
    private bool _isAnimating = false;
    public bool IsAnimating => _isAnimating;
    private void Start()
    {
        _cardSelection = cardObject.CardSelection;
        rectTransform = GetComponent<RectTransform>();
        
        InitHandlers();
    }
    
    private void InitHandlers()
    {
        _cardSelection.OnCardPointerEnter += OnPointerEnter;
        _cardSelection.OnCardPointerExit += OnPointerExit;
        _cardSelection.OnCardPointerDown += OnPointerDown;
        _cardSelection.OnCardPointerUp += OnPointerUp;
        _cardSelection.OnCardPointerRoomEnter += OnPointerRoomEnter;
        _cardSelection.OnCardPointerRoomExit += OnPointerRoomExit;
        _cardSelection.OnCardDragStart += OnDragStart;
        _cardSelection.OnCardDragEnd += OnDragEnd;
    }
    
    public void UpdateDisplay()
    {
        transform.localScale = CardSetting.defaultScale * Vector3.one;
        cardImage.sprite = cardObject.CardData.cardSprite;
        
    }

    public void UpdateIndex()
    {
        transform.SetSiblingIndex(cardObject.GetIdx());
    }
    
    public void DoFade(float targetAlpha, float duration,bool stopCurrent = true)
    {
        if(stopCurrent)
            cardImage.DOKill();
        cardImage.DOFade(targetAlpha, duration);
    }

    
    public float GetClampedVisiblePos(float postion){
        return Mathf.Clamp(postion
            , rectTransform.rect.height * 0.5f * CardSetting.hoverScale
            , Screen.height - rectTransform.rect.height * 0.5f * CardSetting.hoverScale);
    }
    
    private void Update()
    {
        FollowCard();
    }
    
    private void FollowCard()
    {
        var targetPos = cardObject.transform.position;
        if(CardSetting.followAnimation)
            targetPos = Vector3.Lerp(transform.position, targetPos, cardObject.CardSetting.followSpeed * Time.deltaTime);
        transform.position = targetPos;
    }

    private void OnPointerEnter(CardSelection cardSelection)
    {
        if (!CardSetting.isHoverable || cardSelection.IsUsed)
            return;
        transform.DOScale(CardSetting.hoverScale, CardSetting.hoverAnimationDuration);
    }
    
    private void OnPointerExit(CardSelection cardSelection)
    {
        if(cardSelection.IsUsed)
            return;
        transform.DOScale(CardSetting.defaultScale, CardSetting.hoverAnimationDuration);
    }
    
    private void OnPointerDown(CardSelection cardSelection)
    {
       
    }
    
    private void OnPointerUp(CardSelection cardSelection, bool isClick)
    {
        
    }
    
    private void OnPointerRoomEnter(CardSelection cardSelection, Room room)
    {
        if (room != null)
        {
            room.FocusColor(new Color(0.5f, 1f, 0f, 0.5f));
        }
    }
    
    private void OnPointerRoomExit(CardSelection cardSelection, Room room)
    {
        if (room != null)
        {
            room.UnfocusColor();
        }
    }
    
    private void OnDragStart(CardSelection cardSelection)
    {
        if(!cardSelection.IsDragging)
            return;
        _isAnimating = true;
        transform.DOScale(CardSetting.dragScale, CardSetting.dragAnimationDuration);
        cardImage.DOFade(CardSetting.dragAlpha, CardSetting.dragAnimationDuration)
            .OnComplete(() => _isAnimating = false);
    }
    
    private void OnDragEnd(CardSelection cardSelection)
    {
        if(!cardSelection.IsDragging || cardSelection.IsUsed)
            return;
        transform.DOScale(CardSetting.defaultScale, CardSetting.dragAnimationDuration);
        cardImage.DOFade(CardSetting.defaultAlpha, CardSetting.dragAnimationDuration);
    }
    
    public void StopAllDOTweens()
    {
        transform.DOKill();
        cardImage.DOKill();
        _isAnimating = false;
    }
}
