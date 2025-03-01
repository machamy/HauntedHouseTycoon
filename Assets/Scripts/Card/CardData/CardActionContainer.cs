
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 카드 액션 헬퍼 클래스.
/// 전체다 실행하고 싶으면 List에 접근해서 실행하도록
/// </summary>
[Serializable]
public class CardActionContainer: ICloneable, ICopyable<CardActionContainer>
{
    [Serializable]
    public class CardActionContainerNode
    {
        [SerializeReference] public CardActionBlueprintSO actionBlueprint;
        [SerializeReference] public CardActionBlueprintSO.CardAction action;
    } 
    [SerializeField] private List<CardActionContainerNode> actionNodes = new ();
    
    public void OnValidate()
    {
        foreach (var cardActionContainerNode in actionNodes)
        {
            if (cardActionContainerNode.actionBlueprint == null)
            {
                continue;
            }

            if (cardActionContainerNode.action == null 
                || cardActionContainerNode.actionBlueprint.GetCardActionType() != cardActionContainerNode.action.GetType())
            {
                cardActionContainerNode.action = cardActionContainerNode.actionBlueprint.CreateCardAction();
            }
        }
    }
    public void AddAction(CardActionBlueprintSO actionBlueprint)
    {
        actionNodes.Add(new CardActionContainerNode
        {
            actionBlueprint = actionBlueprint,
            action = actionBlueprint.CreateCardAction()
        });
    }

    public object Clone()
    {
        var obj = new CardActionContainer();
        obj.CopyFrom(this);
        return obj;
    }
    
    public void Clear()
    {
        actionNodes.Clear();
    }

    public void CopyTo(CardActionContainer target)
    {
        target.actionNodes.Clear();
        foreach (var node in actionNodes)
        {
            target.actionNodes.Add(new CardActionContainerNode
            {
                actionBlueprint = node.actionBlueprint,
                action = node.action
            });
        }
    }

    public void CopyFrom(CardActionContainer other)
    {
        Clear();
        other.CopyTo(this);
    }

    
    /// <summary>
    /// 카드 설치 이벤트
    /// 처음 True를 반환할때까지 모든 Action을 순회함.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="cardData"></param>
    /// <returns></returns>
    public bool InvokeOnCardPlaced(Room room, CardData cardData)
    {
        foreach (var node in actionNodes)
      {
            if (node.action.OnCardPlaced(room, cardData))
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
        foreach (var node in actionNodes)
        {
            if (node.action.OnCardRemoved(room, cardData))
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
    public bool InvokeOnCustomerEnter(Room room, CardData cardData, GuestObject guestObject){
        foreach (var node in actionNodes)
        {
            if (node.action.OnGuestEnter(room, cardData, guestObject))
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
    public bool InvokeOnCustomerExit(Room room, CardData cardData, GuestObject guestObject){
        foreach (var node in actionNodes)
        {
            if (node.action.OnGuestExit(room, cardData, guestObject))
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 플레이어 턴 시작 이벤트
    /// 처음 True를 반환할 때까지 모든 Action을 순회함.
    /// </summary>
    public bool InvokeOnPlayerTurnEnter(Room room, CardData cardData){
        foreach (var node in actionNodes)
        {
            if (node.action.OnPlayerTurnEnter(room, cardData))
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// NPC 턴 시작 이벤트
    /// 처음 True를 반환할 때까지 모든 Action을 순회함.
    /// </summary>
    public bool InvokeOnNpcTurnEnter(Room room, CardData cardData){
        foreach (var node in actionNodes)
        {
            if (node.action.OnNpcTurnEnter(room, cardData))
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    ///  NPC 턴 종료 이벤트
    /// 처음 True를 반환할 때까지 모든 Action을 순회함.
    /// </summary>
    public bool InvokeOnNpcTurnExit(Room room, CardData cardData){
        foreach (var node in actionNodes)
        {
            if (node.action.OnNpcTurnExit(room, cardData))
            {
                return true;
            }
        }

        return false;
    }



}
