
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// UI 부분이 "아닌" InteractionEvent를 처리하는 클래스
/// UI와 UI연관 행위는 직접 이벤트를 받아서 처리하도록 구현하자
/// </summary>
/// <seealso cref="CardSelection"/>
/// <seealso cref="CardUseArea.RaycastCardUseArea(UnityEngine.Vector3,UnityEngine.Camera)"/>
public class InteractionEventDispatcher : SingletonBehaviour<InteractionEventDispatcher>
{
    [SerializeField] private InputActionAsset inputActionAsset;

    [SerializeField,VisibleOnly(EditableIn.EditMode)] private InputActionReference pointerPress;
    [SerializeField,VisibleOnly(EditableIn.EditMode)] private InputActionReference pointerRelease;
    [SerializeField,VisibleOnly(EditableIn.EditMode)] private InputActionReference pointerHold;
    [SerializeField,VisibleOnly(EditableIn.EditMode)] private InputActionReference pointerTap;
    [SerializeField,VisibleOnly(EditableIn.EditMode)] private InputActionReference pointerPosition;
    
    // record 이용해서 할 수 있음
    
    public class PointerEventArgs
    {
        internal Vector2 pressedPosition = Vector2.zero;
        internal Vector2 position = Vector2.zero;
        internal Vector2 releasedPosition = Vector2.zero;
        internal float pressedTime = -1f;
        internal float time = -1f;
        internal float releaseTime = -1f;
        internal bool isPrevioslyDragging = false;
        
        public Vector2 PressedPosition => pressedPosition;
        public Vector2 Position => position;
        public Vector2 ReleasedPosition => releasedPosition;
        public float PressedTime => pressedTime;
        public float Time => time;
        public float ReleaseTime => releaseTime;
    }
    
    
    public event Action<PointerEventArgs> OnPointerPressEvent;
    public event Action<PointerEventArgs> OnPointerReleaseEvent;
    public event Action<PointerEventArgs> OnPointerHoldEvent;
    public event Action<PointerEventArgs> OnPointerDraggingEvent; 
    public event Action<PointerEventArgs> OnPointerDragEndEvent; 
    public event Action<PointerEventArgs> OnPointerTapEvent;
    public event Action<PointerEventArgs> OnPointerPositionEvent;

    private Vector2 _pressedPosition = Vector2.zero;
    private Vector2 _releasedPositione = Vector2.zero;
    private Vector2 _lastPosition = Vector2.zero;
    private float _pressedTime;
    private float _releaseTime;
    
    private bool _isDragging = false;
    private void Awake()
    {
        var inputActionMap = inputActionAsset.FindActionMap("Pointer");
        inputActionMap.Enable();
    }

    private void OnEnable()
    {
        pointerPress.action.performed += PointerPress;
        pointerRelease.action.performed += PointerRelease;
        pointerHold.action.performed += PointerHold;
        pointerTap.action.performed += PointerTap;
        pointerPosition.action.performed += PointerPosition;
        
    }

    public PointerEventArgs GetPointerEvent()
    {
        return new PointerEventArgs
        {
            pressedPosition = _pressedPosition,
            position = _lastPosition,
            releasedPosition = _releasedPositione,
            pressedTime = _pressedTime,
            time = Time.time,
            releaseTime = _releaseTime
        };
    }
    
    private void PointerPress(InputAction.CallbackContext context)
    {
        // Debug.Log($"Pointer {context.action.name}");
        _pressedTime = Time.time;
        _pressedPosition = _lastPosition;
        OnPointerPressEvent?.Invoke(GetPointerEvent());
    }
    
    private void PointerRelease(InputAction.CallbackContext context)
    {
        // Debug.Log($"Pointer {context.action.name}");
        _releaseTime = Time.time;
        _releasedPositione = _lastPosition;
        OnPointerReleaseEvent?.Invoke(GetPointerEvent());
        
        if (_isDragging)
        {
            _isDragging = false;
            OnPointerDragEndEvent?.Invoke(GetPointerEvent());
        }
    }
    
    private void PointerHold(InputAction.CallbackContext context)
    {
        // Debug.Log($"Pointer {context.action.name}");
        OnPointerHoldEvent?.Invoke(GetPointerEvent());
    }
    
    private void PointerTap(InputAction.CallbackContext context)
    {
        // Debug.Log($"Pointer {context.action.name}");
        OnPointerTapEvent?.Invoke(GetPointerEvent());
    }

    private void PointerPosition(InputAction.CallbackContext context)
    {
        _lastPosition = context.ReadValue<Vector2>();
        // Debug.Log($"Pointer {context.action.name}");
        OnPointerPositionEvent?.Invoke(GetPointerEvent());
        
        if (_isDragging)
        {
            OnPointerDraggingEvent?.Invoke(GetPointerEvent());
        }
        // 사실 움직일때만 호출되므로, 지금은 if 조건이 의미없음
        else if (Vector2.Distance(_lastPosition, context.ReadValue<Vector2>()) > 0.05f)
        {
            _isDragging = true;
            OnPointerDraggingEvent?.Invoke(GetPointerEvent());
        }
    }

    private void OnDisable()
    {
        pointerPress.action.performed -= PointerPress;
        pointerRelease.action.performed -= PointerRelease;
        pointerHold.action.performed -= PointerHold;
        pointerTap.action.performed -= PointerTap;
        pointerPosition.action.performed -= PointerPosition;
    }
    
    
    
    
    
    
}
