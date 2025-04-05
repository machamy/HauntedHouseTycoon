
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionBlueprint"></param>
    public void AddAction(CardActionBlueprintSO actionBlueprint)
    {
        actionNodes.Add(new CardActionContainerNode
        {
            actionBlueprint = actionBlueprint,
            action = actionBlueprint.CreateCardAction()
        });
    }
    
    public void Initialize(Room room, CardData cardData)
    {
        foreach (var node in actionNodes)
        {
            node.action.Initialize(room, cardData);
        }
    }

    public void InvokeOnCardPlaced(CardEventArgs cardEvent)
    {
        foreach (var node in actionNodes)
        {
            if (node.action.OnCardPlaced(cardEvent))
            {
                break;
            }
        }
    }
    
    public void InvokeOnCardRemoved(CardEventArgs cardEvent)
    {
        foreach (var node in actionNodes)
        {
            if (node.action.OnCardRemoved(cardEvent))
            {
                break;
            }
        }
    }
    
    public void InvokeOnGuestEnter(GuestMoveEventArgs guestMoveEventArgs)
    {
        foreach (var node in actionNodes)
        {
            if (node.action.OnGuestEnter(guestMoveEventArgs))
            {
                break;
            }
        }
    }
    
    public void InvokeOnGuestExit(GuestMoveEventArgs guestMoveEventArgs)
    {
        foreach (var node in actionNodes)
        {
            if (node.action.OnGuestExit(guestMoveEventArgs))
            {
                break;
            }
        }
    }
    
    public void InvokeOnPlayerTurnEnter(TurnEventArgs turnEvent)
    {
        foreach (var node in actionNodes)
        {
            if (node.action.OnPlayerTurnEnter(turnEvent))
            {
                break;
            }
        }
    }
    
    public void InvokeOnNpcTurnEnter(TurnEventArgs turnEvent)
    {
        foreach (var node in actionNodes)
        {
            if (node.action.OnNpcTurnEnter(turnEvent))
            {
                break;
            }
        }
    }
    
    public void InvokeOnNpcTurnExit(TurnEventArgs turnEvent)
    {
        foreach (var node in actionNodes)
        {
            if (node.action.OnNpcTurnExit(turnEvent))
            {
                break;
            }
        }
    }
    
        
    
    
    #region Interface Implementations
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
    

    #endregion
    



}
