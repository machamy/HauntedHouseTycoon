
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CardDataPool : IObjectPool<CardData>
{
    private static CardDataPool _instance;
    public static CardDataPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardDataPool();
            }
            return _instance;
        }
    }
    
    
    private Stack<CardData> _pool = new Stack<CardData>();
    
    public CardData Get()
    {
        Debug.Log($"CardDataPool::Get from {_pool.Count}");
        if (_pool.Count > 0)
        {
            var cardData = _pool.Pop();
            cardData.Reset();
            return cardData;
        }
        return new CardData();
    }

    public PooledObject<CardData> Get(out CardData v)
    {
        v = Get();
        return  new PooledObject<CardData>(v, this);
    }

    public void Release(CardData element)
    {
        Debug.Log($"CardDataPool::Release to {_pool.Count}");
        _pool.Push(element);
    }

    public void Clear()
    {
        _pool.Clear();
    }

    public int CountInactive { get { return _pool.Count; } }
}
