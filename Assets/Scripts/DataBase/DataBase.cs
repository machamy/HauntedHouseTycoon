
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : SingletonBehaviour<DataBase>
{
    [SerializeField] private List<CardDataSO> cardDataSOList;
    public List<CardData> cardDataList;
    
    private void OnValidate()
    {
        cardDataList = new List<CardData>();
        foreach (var cardDataSO in cardDataSOList)
        {
            if(!cardDataSO || cardDataSO.cardData == null)
                continue;
            cardDataList.Add(cardDataSO.cardData);
        }
    }
}
