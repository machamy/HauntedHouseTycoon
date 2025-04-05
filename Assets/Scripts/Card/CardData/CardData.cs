
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
public class CardData : ICloneable<CardData>, ICopyable<CardData>
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
    public CardActionContainer cardActionContainer = new CardActionContainer();
    public CardDataVariables nonlocalVariables = new CardDataVariables();

    
    public Deck returnDeck { get; set; }
    
    // 업글은 3장 모이면 되는거 아니었나? 
    public void UpgradeTemporary()
    {
        //TODO   
    }
    public void UpgradePermanent()
    {
        //TODO
    }
    
    public void CleanAction()
    {
        cardActionContainer.Clear();
    }
    
    public void OnValidate()
    {
        cardActionContainer.OnValidate();
    }
    
    public void OnCardPlaced(CardEventArgs cardEvent)
    {
        cardActionContainer.Initialize(cardEvent.room, this);
    }


    public bool IsBlank()
    {
        return String.IsNullOrWhiteSpace(cardName) || cardName.Equals("Blank");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public CardData Clone()
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
        target.nonlocalVariables.CopyFrom(nonlocalVariables);
        target.returnDeck = returnDeck;

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
        nonlocalVariables.Clear();
    }
}
