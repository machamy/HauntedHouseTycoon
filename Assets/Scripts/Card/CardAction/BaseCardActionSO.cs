using UnityEngine;

[CreateAssetMenu(fileName = "CardAction", menuName = "Card/CardAction", order = 0)]
public class BaseCardActionSO : ScriptableObject
{
    public string actionName;
    
    public bool breakChain = false;
    
    // Room에 CardData가 있긴하지만... 일단 이렇게
    // EventArgs 같은 클래스를 사용하면 더 깔끔함
    
    public virtual bool OnCardPlaced(Room room, CardData cardData)
    {
        return false;
    }
    
    public virtual bool OnCardRemoved(Room room,CardData cardData)
    {
        return false;
    }
    
    public virtual bool OnCustomerEnter(Room room, CardData cardData, Customer customer)
    {
        return false;
    }
    
    public virtual bool OnCustomerExit(Room room, CardData cardData, Customer customer)
    {
        return false;
    }
    
    public virtual bool OnTurnEnter(Room room, CardData cardData)
    {
        return false;
    }
    
    public virtual bool OnTurnExit(Room room, CardData cardData)
    {
        return false;
    }
}