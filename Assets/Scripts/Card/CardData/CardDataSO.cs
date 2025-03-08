
using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData")]
public class CardDataSO : ScriptableObject
{
    [SerializeField] private CardData cardData;
    
    public CardData OriginalCardData => cardData;

    public CardData GetCardData()
    {
        return cardData.Clone();
    }
    
    public void LoadFromJSON(string jsonCardData)
    {
        if(File.Exists(jsonCardData))
        {
            string json = File.ReadAllText(jsonCardData);
            cardData = JsonUtility.FromJson<cardDataWrapper>(json).cardData;
        }
    }

    public void OnValidate()
    {
        cardData?.OnValidate();
    }

    [System.Serializable]
    private class cardDataWrapper
    {
        public CardData cardData;
    }
}
