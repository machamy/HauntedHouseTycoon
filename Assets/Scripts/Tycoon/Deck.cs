using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 덱
/// TODO : 드로우/버리기 제거, 보유 카드 다 들고있기
/// </summary>
[Serializable]
public class Deck
{
    [SerializeField] private IntVariableSO discardSize;
    [SerializeField] private IntVariableSO drawSize;
    
    [SerializeField] List<CardData> cardDataList = new List<CardData>();
    Queue<CardData> drawCardQueue = new Queue<CardData>();
    Queue<CardData> discardCardQueue = new Queue<CardData>();
    
    public List<CardData> CardDataListRef => cardDataList;
    public Queue<CardData> DrawCardQueueRef => drawCardQueue;
    public Queue<CardData> DiscardCardQueueRef => discardCardQueue;
    
    public CardData DrawCard()
    {
        if (drawCardQueue.Count <= 0)
        {
            if (discardCardQueue.Count <= 0)
                return null;
            ShuffleDiscardToDraw();
        }
        CardData cardMetaData = drawCardQueue.Dequeue();
        drawSize.Value = drawCardQueue.Count;
        return cardMetaData;
    }
    
    public void AddCard(CardData cardMetaData)
    {
        cardDataList.Add(cardMetaData);
    }
    
    public void SetupForCycle()
    {
        drawCardQueue = new Queue<CardData>(cardDataList);
        ShuffleDrawPool();
    }
    
    public void AddCardToDrawPool(CardData cardMetaData)
    {
        drawCardQueue.Enqueue(cardMetaData);
        drawSize.Value = drawCardQueue.Count;
    }
    
    public void AddCardToDiscardPool(CardData cardMetaData)
    {
        discardCardQueue.Enqueue(cardMetaData);
        discardSize.Value = discardCardQueue.Count;
    }
    
    public void ShuffleDrawPool()
    {
        List<CardData> drawList = new List<CardData>(drawCardQueue);
        drawList.Shuffle();
        drawCardQueue = new Queue<CardData>(drawList);
        drawSize.Value = drawCardQueue.Count;
        discardSize.Value = discardCardQueue.Count;
    }
    
    public void ShuffleDiscardPool()
    {
        List<CardData> discardList = new List<CardData>(discardCardQueue);
        discardList.Shuffle();
        discardCardQueue = new Queue<CardData>(discardList);
        drawSize.Value = drawCardQueue.Count;
        discardSize.Value = discardCardQueue.Count;
    }
    
    public void ShuffleDiscardToDraw()
    {
        List<CardData> discardList = new List<CardData>(discardCardQueue);
        discardList.Shuffle();
        drawCardQueue = new Queue<CardData>(discardList);
        discardCardQueue.Clear();
        discardSize.Value = discardCardQueue.Count;
        drawSize.Value = drawCardQueue.Count;
    }
    
    public void ShuffleAll()
    {
        List<CardData> resultList = new List<CardData>(drawCardQueue);
        resultList.AddRange(discardCardQueue);
        resultList.Shuffle();
        drawCardQueue = new Queue<CardData>(resultList);
    }
}