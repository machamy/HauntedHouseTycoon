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
    
    
    public bool IsFocused => _isFocused;
    public bool IsDragging => _isDragging;
    public bool IsSelected => _isSelected;
    public bool IsUsed => _isUsed;
    
    private void Awake()
    {
        _pointerDownTimeDict = new Dictionary<int, float>();
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
    public PriorityEvent<PointerEventData,CardSelection> OnCardPointerEnter = new PriorityEvent<PointerEventData, CardSelection>();
    public PriorityEvent<PointerEventData,CardSelection> OnCardPointerExit = new PriorityEvent<PointerEventData, CardSelection>();
    public event Action<PointerEventData,CardSelection> OnCardPointerDown;
    public event Action<PointerEventData,CardSelection,bool> OnCardPointerUp;
    public event Action<PointerEventData,CardSelection,Room> OnCardPointerRoomEnter;
    public event Action<PointerEventData,CardSelection,Room> OnCardPointerRoomExit;
    public event Action<PointerEventData,CardSelection> OnCardDragStart;
    public event Action<PointerEventData,CardSelection> OnCardDragEnd;
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isFocused = true;
        OnCardPointerEnter?.Invoke(eventData,this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _isFocused = false;
        OnCardPointerExit?.Invoke(eventData,this);
    }
    
    private Dictionary<int,float> _pointerDownTimeDict = new Dictionary<int, float>();
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownTimeDict[eventData.pointerId] = Time.time;
        OnCardPointerDown?.Invoke(eventData,this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        bool isClick = Time.time - _pointerDownTimeDict[eventData.pointerId] < 0.2f;
        _pointerDownTimeDict.Remove(eventData.pointerId);
        OnCardPointerUp?.Invoke(eventData,this,isClick);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!cardSetting.isDraggable || !TycoonManager.Instance.TurnManager.IsPlayerTurn)
            return;
        var screenPoint = eventData.position;
        
        _isDragging = true;
        OnCardDragStart?.Invoke(eventData,this);
    }

    private Room _prevPointerRoom;
    
    private PointerEventData _lastDragEventData;
    public void OnDrag(PointerEventData eventData)
    {
        _lastDragEventData = eventData;
        if(!_isDragging || !cardSetting.isDraggable)
            return;
        var targetPosUi = eventData.position;
        transform.position = targetPosUi;
        // Field field = TycoonManager.Instance.Field;
        CardUseArea cardUseArea = CardUseArea.RaycastCardUseArea(targetPosUi);
        Room room = (Room) cardUseArea?.parent;
        if(room != null && room != _prevPointerRoom)
        {
            if(_prevPointerRoom != null)
                OnCardPointerRoomExit?.Invoke(eventData,this,_prevPointerRoom);
            _prevPointerRoom = room;
            OnCardPointerRoomEnter?.Invoke(eventData,this,room);
        }
        else if(room == null && _prevPointerRoom != null)
        {
            OnCardPointerRoomExit?.Invoke(eventData,this,_prevPointerRoom);
            _prevPointerRoom = null;
        }
    }
    
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_prevPointerRoom)
        {
            OnCardPointerRoomExit?.Invoke(eventData,this,_prevPointerRoom);
        }
        if(!_isDragging || !cardSetting.isDraggable)
            return;
        
        var targetPosUi = eventData.position;
        transform.position = targetPosUi;
        CardUseArea cardUseArea = CardUseArea.RaycastCardUseArea(targetPosUi);
        if(cardUseArea != null)
        {
            cardUseArea.RaiseOnCardUseEvent(cardObject.CardData);
            _isUsed = true;
            cardObject.LeaveHand();
        }
        // if(room != null)
        // {
        //     Vector3 uiSize = CardObject.CardDisplay.CardImageSize * CardObject.transform.lossyScale.x;
        //     room.PlaceCard(cardObject.CardData, uiSize);
        //     // TycoonManager.Instance.TurnManager.ReadyToPlayerTurnEnd();
        //     _isUsed = true;
        //     cardObject.LeaveHand();
        // }
        transform.localPosition = Vector3.zero;
        OnCardDragEnd?.Invoke(eventData,this);
        _isDragging = false;
    }
    
    
    private void OnPlayerTurnExit()
    {
        if (IsUsed)
            return;
        if(_isDragging)
        {
            _prevPointerRoom.Unfocus();
            _prevPointerRoom = null;
            transform.localPosition = Vector3.zero;
            OnCardDragEnd?.Invoke(_lastDragEventData,this);
            _isDragging = false;
        }
    }
    #endregion
}
