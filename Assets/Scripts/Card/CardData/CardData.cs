
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
    public CardActionContainer cardActionContainer= new CardActionContainer();
    
    // TODO 각 Action이 arguments를 가지도록... 지금은 서로 간섭 가능
    public List<CardDataAgument> arguments = new List<CardDataAgument>(); 
    
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
        var pool = CardDataPool.Instance;
        var obj = pool.Get();
        CopyTo(obj);
        return obj;
    }
    
    public void CopyTo(CardData target)
    {
        target.cardName = cardName;
        target.cardDescription = cardDescription;
        target.directions = directions;
        target.cardSprite = cardSprite;
        target.simpleCardSprite = simpleCardSprite;
        target.halfCardSprite = halfCardSprite;
        target.fullCardSprite = fullCardSprite;
        target.cardPlacedSprite = cardPlacedSprite;
        target.cardActionContainer.CopyFrom(cardActionContainer);
        target.arguments.Clear();
        target.returnDeck = returnDeck;

        foreach (var argument in arguments)
        {
            target.arguments.Add((CardDataAgument)argument.Clone());
        }
    }
    
    public void CopyFrom(CardData target)
    {
        target.CopyTo(this);
    }
    
    public void Reset()
    {
        cardName = "";
        directions = DirectionFlag.None;
        cardDescription = "";
        CleanAction();
        arguments.Clear();
    }
}
