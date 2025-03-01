using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "CardData2SO")]
public class CardData2SO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.CardClass> cardDataList = new List<ClassManager.Card.CardClass>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);

        string json = File.ReadAllText(fullPath);

        CardDataWrapper wrapper = JsonConvert.DeserializeObject<CardDataWrapper>(json);

        Debug.Log($"[CardData2SO] JSON에서 읽어온 카드 개수: {wrapper.cardDataList.Count}");


        cardDataList.Clear();

        foreach (var wrappedCard in wrapper.cardDataList)
        {
            int parsedType = TryParseInt(wrappedCard.CardType, -1);
            int parsedRank = TryParseInt(wrappedCard.CardRank, -1);


            var newCard = new ClassManager.Card.CardClass
            {
                Index = TryParseLong(wrappedCard.Index, 0),
                Name = wrappedCard.Name,

                CardType = Enum.IsDefined(typeof(ClassManager.Card.CardClass.Type), parsedType)
                    ? (ClassManager.Card.CardClass.Type)parsedType
                    : ClassManager.Card.CardClass.Type.ENTER,

                CardRank = Enum.IsDefined(typeof(ClassManager.Card.CardClass.Rank), parsedRank)
                    ? (ClassManager.Card.CardClass.Rank)parsedRank
                    : ClassManager.Card.CardClass.Rank.NORMAL,

                Cost = TryParseInt(wrappedCard.Cost, 0),
                KeyWordIndex = new long[] { TryParseLong(wrappedCard.KeywordIndex, -1) },
                SpritePath = wrappedCard.SpritePath,
                AvailableRoutes = new int[]
                {
            TryParseInt(wrappedCard.AvailableRoutes_0, -1),
            TryParseInt(wrappedCard.AvailableRoutes_1, -1)
                },
                DestroyPayback = TryParseInt(wrappedCard.DestroyPayback, 0),
                CardEffectIndex = TryParseLong(wrappedCard.CardEffectIndex, -1),
                PlaceAnimationIndex = TryParseLong(wrappedCard.PlaceAnimationIndex, -1),
                ActionAnimationIndex = TryParseLong(wrappedCard.ActionAnimationIndex, -1)
            };

            cardDataList.Add(newCard);
        }

        Debug.Log($"[CardData2SO] ScriptableObject 저장 완료! 총 {cardDataList.Count}개");
    }

    private int TryParseInt(string value, int defaultValue)
    {
        if (int.TryParse(value, out int result))
            return result;
        return defaultValue;
    }

    private long TryParseLong(string value, long defaultValue)
    {
        if (long.TryParse(value, out long result))
            return result;
        return defaultValue;
    }

    [System.Serializable]
    public class CardDataWrapper
    {
        public List<CardDataStruct> cardDataList;
    }

    [System.Serializable]
    public struct CardDataStruct
    {
        public string Index;
        public string Name;

        [JsonProperty("type")]
        public string CardType;

        [JsonProperty("rank")]
        public string CardRank;

        public string Cost;
        public string KeywordIndex;
        public string SpritePath;
        public string AvailableRoutes_0;
        public string AvailableRoutes_1;
        public string DestroyPayback;
        public string CardEffectIndex;
        public string PlaceAnimationIndex;
        public string ActionAnimationIndex;
    }
}

