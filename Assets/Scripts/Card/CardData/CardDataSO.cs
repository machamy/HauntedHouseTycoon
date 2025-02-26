
using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData")]
public class CardDataSO : ScriptableObject
{
    [SerializeField] public CardData cardData;
    
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
