
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 카드 정보 클래스ㅜ
/// 실제로 Room 안에 들어간 CardData는 복사되어 사용되어야함!
/// </summary>
[Serializable]
public class CardData : ICloneable
{
    public string cardName;
    public string cardDescription;
    // [FormerlySerializedAs("directions")] public List<Direction> directionsLegacy;
    public DirectionFlag directions;
    public Sprite cardSprite;
    public Sprite simpleCardSprite;
    public Sprite halfCardSprite;
    public Sprite fullCardSprite;
    public Sprite cardPlacedSprite;
    public CardActionContainer cardActionContainer;
    
    // TODO 각 Action이 arguments를 가지도록... 지금은 서로 간섭 가능
    public List<CardDataAgument> arguments; 
    
    public Deck returnDeck { get; set; }
    
    public void CleanAction()
    {
        cardActionContainer.actions.Clear();
    }
    
    public int GetArgumentIntDefault(CardDataAgument.Key key, int defaultValue)
    {
        foreach (var agument in arguments)
        {
            if (agument.key == key)
            {
                return agument.intValue;
            }
        }
        SetArgumentInt(key, defaultValue);
        return defaultValue;
    }

    public int GetArgumentInt(CardDataAgument.Key key)
    {
        foreach (var agument in arguments)
        {
            if (agument.key == key)
            {
                return agument.intValue;
            }
        }
        Debug.LogError($"[CardData::GetArgumentInt] {key} not found, return 0");
        return 0;
    }
    
    public void SetArgumentInt(CardDataAgument.Key key, int value)
    {
        foreach (var agument in arguments)
        {
            if (agument.key == key)
            {
                agument.intValue = value;
                return;
            }
        }
        arguments.Add(new CardDataAgument
        {
            key = key,
            intValue = value
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        var obj = new CardData
        {
            cardName = cardName,
            cardDescription = cardDescription,
            directions = directions,
            cardSprite = cardSprite,
            simpleCardSprite = simpleCardSprite,
            halfCardSprite = halfCardSprite,
            fullCardSprite = fullCardSprite,
            cardPlacedSprite = cardPlacedSprite,
            cardActionContainer = (CardActionContainer)cardActionContainer.Clone(),
            arguments = new List<CardDataAgument>(),
            returnDeck = returnDeck
        };

        foreach (var argument in arguments)
        {
            obj.arguments.Add((CardDataAgument)argument.Clone());
        }

        return obj;
    }
}
