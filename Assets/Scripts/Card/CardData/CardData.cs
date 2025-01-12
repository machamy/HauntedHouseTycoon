
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CardData : ICloneable
{
    public string cardName;
    public string cardDescription;
    public List<Direction> directions;
    public Sprite cardSprite;
    public Sprite cardPlacedSprite;
    public CardActionContainer cardActionContainer;
    public List<CardDataAgument> arguments; // TODO 직렬화 Dict으로
    
    public Deck returnDeck { get; set; }
    
    
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
            directions = new List<Direction>(),
            cardSprite = cardSprite,
            cardPlacedSprite = cardPlacedSprite,
            cardActionContainer = cardActionContainer,
            arguments = new List<CardDataAgument>(),
            returnDeck = returnDeck
        };
        foreach (var direction in directions)
        {
            obj.directions.Add(direction);
        }

        foreach (var argument in arguments)
        {
            obj.arguments.Add((CardDataAgument)argument.Clone());
        }

        return obj;
    }
}
