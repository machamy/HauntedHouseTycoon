
using System;
using System.Collections.Generic;

/// <summary>
/// 카드 액션 헬퍼 클래스.
/// 전체다 실행하고 싶으면 List에 접근해서 실행하도록
/// </summary>
[Serializable]
public class CardActionContainer: ICloneable
{
    public List<BaseCardActionSO> actions = new List<BaseCardActionSO>();
    
    /// <summary>
    /// 카드 설치 이벤트
    /// 처음 True를 반환할때까지 모든 Action을 순회함.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="cardData"></param>
    /// <returns></returns>
    public bool InvokeOnCardPlaced(Room room, CardData cardData)
    {
        foreach (var action in actions)
      {
            if (action.OnCardPlaced(room, cardData))
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 카드 제거 이벤트
    /// 처음 True를 반환할때까지 모든 Action을 순회함.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="cardData"></param>
    /// <returns></returns>
    public bool InvokeOnCardRemoved(Room room, CardData cardData)
    {
        foreach (var action in actions)
        {
            if (action.OnCardRemoved(room, cardData))
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 고객 입장 이벤트
    /// 처음 True를 반환할 때까지 모든 Action을 순회함.
    /// </summary>
    public bool InvokeOnCustomerEnter(Room room, CardData cardData, Guest guest){
        foreach (var action in actions)
        {
            if (action.OnCustomerEnter(room, cardData, guest))
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 고객 퇴장 이벤트
    /// 처음 True를 반환할 때까지 모든 Action을 순회함.
    /// </summary>
    public bool InvokeOnCustomerExit(Room room, CardData cardData, Guest guest){
        foreach (var action in actions)
        {
            if (action.OnCustomerExit(room, cardData, guest))
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 턴 시작 이벤트
    /// 처음 True를 반환할 때까지 모든 Action을 순회함.
    /// </summary>
    public bool InvokeOnTurnEnter(Room room, CardData cardData){
        foreach (var action in actions)
        {
            if (action.OnTurnEnter(room, cardData))
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 턴 종료 이벤트
    /// 처음 True를 반환할 때까지 모든 Action을 순회함.
    /// </summary>
    public bool InvokeOnTurnExit(Room room, CardData cardData){
        foreach (var action in actions)
        {
            if (action.OnTurnExit(room, cardData))
            {
                return true;
            }
        }

        return false;
    }
    
    public void AddAction(BaseCardActionSO action)
    {
        actions.Add(action);
    }

    public object Clone()
    {
        return new CardActionContainer
        {
            actions = new List<BaseCardActionSO>(actions)
        };
    }
}
