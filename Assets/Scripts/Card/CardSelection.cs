using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardObject))]
public class CardSelection : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerUpHandler,IPointerDownHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private CardObject cardObject;
    private CardSetting cardSetting;

    public CardObject CardObject => cardObject;
    public CardSetting CardSetting => cardSetting;
    
    private bool _isFocused = false;
    private bool _isDragging = false;
    private bool _isSelected = false;
    private bool _isUsed = false;
    
    
    public bool IsDragging => _isDragging;
    public bool IsSelected => _isSelected;
    public bool IsUsed => _isUsed;
    
    private void Awake()
    {
        _pointerDownTime = 0;
        cardObject = GetComponent<CardObject>();
        cardSetting = cardObject.CardSetting;
    }

    private void OnEnable()
    {
        turnEventChannelSo.OnPlayerTurnExit += OnPlayerTurnExit;
    }
    
    private void OnDisable()
    {
        turnEventChannelSo.OnPlayerTurnExit -= OnPlayerTurnExit;
    }

    #region Events
    [SerializeField] private TurnEventChannelSO turnEventChannelSo;
    public event Action<CardSelection> OnCardPointerEnter;
    public event Action<CardSelection> OnCardPointerExit;
    public event Action<CardSelection> OnCardPointerDown;
    public event Action<CardSelection,bool> OnCardPointerUp;
    public event Action<CardSelection,Room> OnCardPointerRoomEnter;
    public event Action<CardSelection,Room> OnCardPointerRoomExit;
    public event Action<CardSelection> OnCardDragStart;
    public event Action<CardSelection> OnCardDragEnd;
    
    
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isFocused = true;
        OnCardPointerEnter?.Invoke(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _isFocused = false;
        OnCardPointerExit?.Invoke(this);
    }
    
    private float _pointerDownTime;
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownTime = Time.time;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        bool isClick = Time.time - _pointerDownTime < 0.2f;
        OnCardPointerUp?.Invoke(this,isClick);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!cardSetting.isDraggable || !TycoonManager.Instance.TurnManager.IsPlayerTurn)
            return;
        var screenPoint = eventData.position;
        
        _isDragging = true;
        OnCardDragStart?.Invoke(this);
    }

    private Room _prevPointerRoom;
    
    public void OnDrag(PointerEventData eventData)
    {
        if(!_isDragging || !cardSetting.isDraggable)
            return;
        var targetPosUi = eventData.position;
        transform.position = targetPosUi;
        Field field = TycoonManager.Instance.Field;
        var targetPos = Camera.main.ScreenToWorldPoint(targetPosUi);
        Room room = field.WorldToRoom(targetPos);
        if(room != null && room != _prevPointerRoom)
        {
            if(_prevPointerRoom != null)
                OnCardPointerRoomExit?.Invoke(this,_prevPointerRoom);
            _prevPointerRoom = room;
            OnCardPointerRoomEnter?.Invoke(this,room);
        }
        else if(room == null && _prevPointerRoom != null)
        {
            OnCardPointerRoomExit?.Invoke(this,_prevPointerRoom);
            _prevPointerRoom = null;
        }
    }
    
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_prevPointerRoom)
        {
            OnCardPointerRoomExit?.Invoke(this,_prevPointerRoom);
        }
        if(!_isDragging || !cardSetting.isDraggable)
            return;
        
        var targetPosUi = eventData.position;
        transform.position = targetPosUi;
        Field field = TycoonManager.Instance.Field;
        var targetPos = Camera.main.ScreenToWorldPoint(targetPosUi);
        Room room = field.WorldToRoom(targetPos);
        if(room != null)
        {
            Vector3 uiSize = CardObject.CardDisplay.CardImageSize * CardObject.transform.lossyScale.x;
            room.PlaceCard(cardObject.CardData, uiSize);
            // TycoonManager.Instance.TurnManager.ReadyToPlayerTurnEnd();
            _isUsed = true;
            cardObject.LeaveHand();
        }
        transform.localPosition = Vector3.zero;
        OnCardDragEnd?.Invoke(this);
        _isDragging = false;
    }
    
    
    private void OnPlayerTurnExit()
    {
        if (IsUsed)
            return;
        if(_isDragging)
        {
            _prevPointerRoom.UnfocusColor();
            _prevPointerRoom = null;
            transform.localPosition = Vector3.zero;
            OnCardDragEnd?.Invoke(this);
            _isDragging = false;
        }
    }
    #endregion
}
