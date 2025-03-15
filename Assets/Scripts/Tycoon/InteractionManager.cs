
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Internal;

public class InteractionManager : SingletonBehaviour<InteractionManager>
{

    [SerializeReference,VisibleOnly]private IFocusable _currentFocused;

    private void OnEnable()
    {
        InteractionEventDispatcher pointerEventDispatcher = InteractionEventDispatcher.Instance;
        pointerEventDispatcher.OnPointerTapEvent += OnClick;
    }
    
    private void OnDisable()
    {
        InteractionEventDispatcher pointerEventDispatcher = InteractionEventDispatcher.Instance;
        pointerEventDispatcher.OnPointerTapEvent -= OnClick;
    }


    public void OnClick(InteractionEventDispatcher.PointerEventArgs pointerEventArgs)
    {
        var target = RaycastIFocusable(pointerEventArgs.position);
        if(target == null)
        {
            _currentFocused?.OnFocusLost();
            _currentFocused = null;
            return;
        }
        if(target != _currentFocused)
        {
            _currentFocused?.OnFocusLost();
            _currentFocused = target;
            _currentFocused.OnFocus();
        }
    }


    public static IFocusable RaycastIFocusable(Vector3 position)
    {
        return RaycastIFocusable(position, Camera.main);
    }
    public static IFocusable RaycastIFocusable(Vector2 screenPos,LayerMask layerMask)
    {
        return RaycastIFocusable(screenPos, layerMask, Camera.main);
    }
    public static IFocusable RaycastIFocusable(Vector2 screenPos,LayerMask layerMask,Camera camera)
    {
        var ray = camera.ScreenPointToRay(screenPos);
        
        // Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
        var hits = new RaycastHit[10];
        var num = Physics.RaycastNonAlloc(ray, hits, 100f, layerMask.value);
        for (int i = 0; i < num; i++) 
        {
            var hit = hits[i];
            if(hit.collider.TryGetComponent<IFocusable>(out var cardUseArea))
            {
                return cardUseArea;
            }
        }
        return null;
    }

    public static IFocusable RaycastIFocusable(Vector3 position, Camera camera)
    {
        var ray = camera.ScreenPointToRay(position);
        
        var hits = new RaycastHit[10];
        var num = Physics.RaycastNonAlloc(ray, hits, 100f);
        for (int i = 0; i < num; i++) 
        {
            var hit = hits[i];
            if(hit.collider.TryGetComponent<IFocusable>(out var cardUseArea))
            {
                return cardUseArea;
            }
        }
        return null;
    }
}
