using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;
 
public class CardDatabase : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.CardData>
{
    [SerializeField]
    public List<ClassBase.Card.CardData> cardDatabaseList = new();

    public ClassBase.Card.CardData FindByIndex(long index)
    {
        return cardDatabaseList.FirstOrDefault(card => card.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON 경로가 존재하지 않음: " + jsonPath);
            return;
        }

        string assetBundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles", "CardSprite");
        AssetBundle cardSpriteBundle = AssetBundle.LoadFromFile(assetBundlePath);
        if (cardSpriteBundle == null)
        {
            Debug.LogError("CardSprite AssetBundle 로드 실패: " + assetBundlePath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray cardArray = JArray.Parse(json);

        cardDatabaseList.Clear();

        foreach (JObject cardObj in cardArray)
        {
            int[] availableRoutes = TypeConverter.ExtractIntArray(cardObj, "availableRoutes");
            int[] input1 = TypeConverter.ExtractIntArray(cardObj, "input1");
            int[] input2 = TypeConverter.ExtractIntArray(cardObj, "input2");
            int[] input3 = TypeConverter.ExtractIntArray(cardObj, "input3");
            int[] input4 = TypeConverter.ExtractIntArray(cardObj, "input4");

            long[] keywordIndex = TypeConverter.ExtractLongArray(cardObj, "keywordIndex");
            long[] cardEffectIndex = TypeConverter.ExtractLongArray(cardObj, "cardEffectIndex");
            long[] placeAnimationIndex = TypeConverter.ExtractLongArray(cardObj, "placeAnimationIndex");
            long[] actionAnimationIndex = TypeConverter.ExtractLongArray(cardObj, "actionAnimationIndex");

            string spritePath = cardObj["spritePath"]?.ToString() ?? "";
            Sprite sprite = null;
            if (!string.IsNullOrEmpty(spritePath))
            {
                sprite = cardSpriteBundle.LoadAsset<Sprite>(spritePath);
                if (sprite == null)
                {
                    Debug.LogWarning("AssetBundle에서 해당 Sprite를 찾을 수 없음: " + spritePath);
                }
            }

            var card = new ClassBase.Card.CardData
            {
                Index = TypeConverter.TryParseLong(cardObj["index"]?.ToString(), 0),
                NameIndex = TypeConverter.TryParseLong(cardObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TypeConverter.TryParseLong(cardObj["explainIndex"]?.ToString(), 0),

                CardType = (ClassBase.Card.CardData.Type)TypeConverter.TryParseInt(cardObj["type"]?.ToString(), 0),
                CardRank = (ClassBase.Card.CardData.Rank)TypeConverter.TryParseInt(cardObj["rank"]?.ToString(), 0),
                Cost = TypeConverter.TryParseInt(cardObj["cost"]?.ToString(), 0),
                DestroyPayback = TypeConverter.TryParseInt(cardObj["destroyPayback"]?.ToString(), 0),

                KeyWordIndex = keywordIndex,
                SpritePath = spritePath,
                Sprite = sprite,
                AvailableRoutes = availableRoutes,
                CardEffectIndex = cardEffectIndex,
                PlaceAnimationIndex = placeAnimationIndex,
                ActionAnimationIndex = actionAnimationIndex,
                input1 = input1,
                input2 = input2,
                input3 = input3,
                input4 = input4
            };

            cardDatabaseList.Add(card);
        }
    }
}
