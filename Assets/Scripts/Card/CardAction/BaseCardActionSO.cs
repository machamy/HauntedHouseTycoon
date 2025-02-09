using UnityEngine;

[CreateAssetMenu(fileName = "CardAction", menuName = "Card/CardAction", order = 0)]
public class BaseCardActionSO : ScriptableObject
{
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
    
    public virtual bool OnCardPlaced(Room room, CardData cardData)
    {
        return false;
    }
    
    public virtual bool OnCardRemoved(Room room,CardData cardData)
    {
        return false;
    }
    
    public virtual bool OnGuestEnter(Room room, CardData cardData, Guest guest)
    {
        return false;
    }
    
    public virtual bool OnGuestExit(Room room, CardData cardData, Guest guest)
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