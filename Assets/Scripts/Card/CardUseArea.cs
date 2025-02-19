
using UnityEngine;
using UnityEngine.Events;


public class CardUseArea : MonoBehaviour
{

    public bool useUnityEvent = false;
    public MonoBehaviour parent;
    public delegate void CardUseAreaEvent(CardUseArea cardUseArea, CardData cardData);
    
    
    
    // public event CardUseAreaEvent OnCardUseAreaEnter;
    public event CardUseAreaEvent OnCardUse;
    // public event CardUseAreaEvent OnCardUseAreaExit;
    
    
    public UnityEvent<CardData> OnCardUseUnityEvent;
    
    public void RaiseOnCardUseEvent(CardData cardData)
    {
        OnCardUse?.Invoke(this, cardData);
        if (useUnityEvent)
        {
            OnCardUseUnityEvent?.Invoke(cardData);
        }
    }
    
    // public void RaiseOnCardUseAreaEnterEvent(CardData cardData)
    // {
    //     OnCardUseAreaEnter?.Invoke(this, cardData);
    // }
    //
    // public void RaiseOnCardUseAreaExitEvent(CardData cardData)
    // {
    //     OnCardUseAreaExit?.Invoke(this, cardData);
    // }
    //
    
    
    
    public static CardUseArea RaycastCardUseArea(Vector3 uiPosition) => RaycastCardUseArea(uiPosition, Camera.main);
    
    public static CardUseArea RaycastCardUseArea(Vector3 uiPosition, Camera camera)
    {
        var ray = camera.ScreenPointToRay(uiPosition);

        LayerMask layerMask = LayerMask.GetMask("CardUseArea");
        // Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
        var hits = new RaycastHit[10];
        var num = Physics.RaycastNonAlloc(ray, hits, 100f, layerMask.value);
        for (int i = 0; i < num; i++) 
        {
            var hit = hits[i];
            if(hit.collider.TryGetComponent<CardUseArea>(out var cardUseArea))
            {
                return cardUseArea;
            }
        }
        // contactFilter2D.useLayerMask = false;
        // num = Physics2D.Raycast(ray.origin, ray.direction, contactFilter2D, hits);
        // for (int i = 0; i < num; i++) 
        // {
        //     var hit = hits[i];
        //     Debug.Log($"{hit.collider.name} {hit.collider.gameObject.layer} {hit.transform.gameObject.layer}");
        //     if(hit.collider.TryGetComponent<CardUseArea>(out var cardUseArea))
        //     {
        //         return cardUseArea;
        //     }
        // }

        return null;
    }
    
}
