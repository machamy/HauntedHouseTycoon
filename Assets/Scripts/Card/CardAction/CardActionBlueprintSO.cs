using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardAction", menuName = "Card/CardAction", order = 0)]
public abstract class CardActionBlueprintSO : ScriptableObject
{
    /// <summary>
    /// 각 카드 액션의 변수가 저장되는 클래스
    /// TODO: 아마 그냥 Action까지 같이 생성하도록 해도 될듯.
    /// </summary>
    [Serializable]
    public class CardAction
    {
        public string actionName;
        public bool breakChain = false;
        
        public CardAction()
        {
            actionName = GetType().Name;
        }
        
        public virtual bool OnCardPlaced(Room room, CardData cardData)
        {
            return false;
        }

    
        public virtual bool OnCardRemoved(Room room,CardData cardData)
        {
            return false;
        }
    
        public virtual bool OnGuestEnter(Room room, CardData cardData, GuestObject guestObject)
        {
            return false;
        }
    
        public virtual bool OnGuestExit(Room room, CardData cardData, GuestObject guestObject)
        {
            return false;
        }
    
        public virtual bool OnPlayerTurnEnter(Room room, CardData cardData)
        {
            return false;
        }
    
        public virtual bool OnNpcTurnEnter(Room room, CardData cardData)
        {
            return false;
        }
    
        public virtual bool OnNpcTurnExit(Room room, CardData cardData)
        {
            return false;
        }
    }
    
    public string actionName;
    
    public bool breakChain = false;
    
    // Room에 CardData가 있긴하지만... 일단 이렇게
    // EventArgs 같은 클래스를 사용하면 더 깔끔함
    
    private void OnEnable()
    {
        if(string.IsNullOrWhiteSpace(actionName))
        {
            actionName = name;
        }
    }

    public abstract CardAction CreateCardAction();

    public abstract Type GetCardActionType();


}