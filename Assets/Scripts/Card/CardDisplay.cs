
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardDisplay : MonoBehaviour
{
    public CardObject cardObject;
    private CardSetting CardSetting => cardObject.CardSetting;
    private CardSelection _cardSelection;
    
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardFullRightImage;
    public Vector3 CardImageSize => cardImage.rectTransform.sizeDelta;
    private RectTransform rectTransform;
    
    private bool _isAnimating = false;
    public bool IsAnimating => _isAnimating;
    
    private bool _handlerInitialized = false;
    private void Start()
    {
        _cardSelection = cardObject.CardSelection;
        rectTransform = GetComponent<RectTransform>();
        InitHandlers();
    }
    
    public void InitHandlers()
    {
        if (_handlerInitialized)
            return;
        _cardSelection.OnCardPointerEnter.RemoveListener(OnPointerEnter);
        _cardSelection.OnCardPointerExit.RemoveListener(OnPointerExit);
        _cardSelection.OnCardPointerDown -= OnPointerDown;
        _cardSelection.OnCardPointerUp -= OnPointerUp;
        _cardSelection.OnCardPointerRoomEnter -= OnPointerRoomEnter;
        _cardSelection.OnCardPointerRoomExit -= OnPointerRoomExit;
        _cardSelection.OnCardDragStart -= OnDragStart;
        _cardSelection.OnCardDragEnd -= OnDragEnd;
        _cardSelection.OnCardPointerEnter.AddListener(OnPointerEnter,-10);
        _cardSelection.OnCardPointerExit.AddListener(OnPointerExit,-10);
        _cardSelection.OnCardPointerDown += OnPointerDown;
        _cardSelection.OnCardPointerUp += OnPointerUp;
        _cardSelection.OnCardPointerRoomEnter += OnPointerRoomEnter;
        _cardSelection.OnCardPointerRoomExit += OnPointerRoomExit;
        _cardSelection.OnCardDragStart += OnDragStart;
        _cardSelection.OnCardDragEnd += OnDragEnd;
        _handlerInitialized = true;
    }
    
    public void InitializeDisplay()
    {
        transform.localScale = CardSetting.defaultScale * Vector3.one;
        if(CardSetting.useDebugSprite)
        {
            cardImage.sprite = cardObject.CardData.cardSprite;
        }
        else
        {
            cardImage.sprite = cardObject.CardData.simpleCardSprite;
        }
        cardImage.SetNativeSize();
    }

    public void UpdateIndex()
    {
        transform.SetSiblingIndex(cardObject.GetRawIdx());
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
        HandPositioning();
        FollowCard();
        FollowRotation();
    }

    float curveYoffset = 0;
    float curveRotationOffset = 0;
    private void HandPositioning()
    {
        // 드래그중이면 패스
        if (_cardSelection.IsDragging || !cardObject.CardHolder)
        {
            curveYoffset = 0;
            curveRotationOffset = 0;
            return;
        }
        int visibleCardAmount = cardObject.CardHolder.CurrentVisibleCardAmount;
        CardCurveSO cardCurveSo = CardSetting.cardCurveSo;
        float normalizedIdx = visibleCardAmount == 1 ? 0: cardObject.GetVisibleIdx() / ((float) visibleCardAmount - 1);
        
        if (CardSetting.usePositionCurve)
            curveYoffset = cardCurveSo.positionCurve.Evaluate(normalizedIdx) *
                           CardSetting.cardCurveSo.positionCoefficient * visibleCardAmount;
        else
            curveYoffset = 0;
        // Debug.Log($"{curveYoffset} {normalizedIdx}");
        // 회전커브 사용, 포커스중이면 호버로테이션 확인
        if(CardSetting.useRotationCurve && (!_cardSelection.IsFocused || CardSetting.hoverRotation))
        {
            float normalizedIdxForRotation;
            if (visibleCardAmount == 1)
                normalizedIdxForRotation = 0.5f;
            else
            {
                normalizedIdxForRotation = cardObject.GetVisibleIdx() / ((float) visibleCardAmount - 1);
            }
            curveRotationOffset = cardCurveSo.rotationCurve.Evaluate(normalizedIdxForRotation) *
                                  CardSetting.cardCurveSo.rotationCoefficient * visibleCardAmount;
        }
        else
            curveRotationOffset = 0;
    }
    
    private void FollowCard()
    {
        var targetPos = cardObject.transform.position;
        targetPos.y += curveYoffset;
        if(CardSetting.followAnimation)
            targetPos = Vector3.Lerp(transform.position, targetPos, cardObject.CardSetting.followSpeed * Time.deltaTime);
        transform.position = targetPos;
        
    }
    private void FollowRotation()
    {
        Quaternion targetRotation = cardObject.transform.rotation;
        targetRotation.eulerAngles += new Vector3(0,0,curveRotationOffset);
        if(CardSetting.followRotation)
            targetRotation = Quaternion.Lerp(transform.rotation, targetRotation, cardObject.CardSetting.followRotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    private void OnPointerEnter(PointerEventData eventData, CardSelection cardSelection)
    {
        Debug.Log("CardDisplay OnPointerEnter");
        if (!CardSetting.isHoverable || cardSelection.IsUsed)
            return;
        
        Hover();
    }
    
    private void OnPointerExit(PointerEventData eventData,CardSelection cardSelection)
    {
        if(cardSelection.IsUsed)
            return;
        
        Unhover();
        if(isShowPopup)
        {
            showingPopup.Close();
        }
        
    }
    
    private void Hover()
    {
        if (!CardSetting.useDebugSprite)
        {
            ShowHalfCard();
        }
        transform.DOScale(CardSetting.hoverScale, CardSetting.hoverAnimationDuration);
        transform.SetAsLastSibling();
    }
    
    private void Unhover()
    {
        if (!CardSetting.useDebugSprite)
        {
            ShowSimpleCard();
        }
        transform.DOScale(CardSetting.defaultScale, CardSetting.hoverAnimationDuration);
        UpdateIndex();
    }
    
    private void ShowSimpleCard()
    {
        cardImage.sprite = cardObject.CardData.simpleCardSprite;
        cardImage.SetNativeSize();
    }
    
    private void ShowHalfCard()
    {
        cardImage.sprite = cardObject.CardData.halfCardSprite;
        cardImage.SetNativeSize();
    }
    
    
    
    private void OnPointerDown(PointerEventData eventData,CardSelection cardSelection)
    {
       
    }
    
    private bool isShowPopup = false;
    private BasePopupUI showingPopup;
    private void OnPointerUp(PointerEventData eventData,CardSelection cardSelection, bool isClick)
    {
        if (isClick)
        {
            if(eventData.button == PointerEventData.InputButton.Right && !isShowPopup)
            {
                CardPopupUI cardPopupUi = UIManager.Instance.CreateUI<CardPopupUI>(UI_Poups.CardPopup);
                cardPopupUi.Initialize(cardObject.CardData);
                isShowPopup = true;
                showingPopup = cardPopupUi;
                Unhover();
                cardPopupUi.OnClose += () =>
                {
                    if (_cardSelection.IsFocused)
                    {
                        Hover();
                    }
                    isShowPopup = false;
                };
            }
        }
    }
    
    private void OnPointerRoomEnter(PointerEventData eventData,CardSelection cardSelection, Room room)
    {
        if (room != null)
        {
            room.FocusSprite(cardObject.CardData.cardSprite, new Color(0.5f,1f,0f,0.5f));
        }
    }
    
    private void OnPointerRoomExit(PointerEventData eventData,CardSelection cardSelection, Room room)
    {
        if (room != null)
        {
            room.Unfocus();
        }
    }
    
    private void OnDragStart(PointerEventData eventData,CardSelection cardSelection)
    {
        if(!cardSelection.IsDragging)
            return;
        _isAnimating = true;
        transform.DOScale(CardSetting.dragScale, CardSetting.dragAnimationDuration);
        cardImage.DOFade(CardSetting.dragAlpha, CardSetting.dragAnimationDuration)
            .OnComplete(() => _isAnimating = false);
    }
    
    private void OnDragEnd(PointerEventData eventData, CardSelection cardSelection)
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
