using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime;

[CreateAssetMenu(menuName = "CardData2SO")]
public class CardData2SO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.CardClass> cardDataList = new List<ClassManager.Card.CardClass>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray cardArray = JArray.Parse(json);

        cardDataList.Clear();


        foreach (JToken token in cardArray)
        {
            JObject cardObj = (JObject)token;

            int[] availableRoutes = ExtractIntArray(cardObj, "availableRoutes");
            int[] input1 = ExtractIntArray(cardObj, "input1");
            int[] input2 = ExtractIntArray(cardObj, "input2");
            int[] input3 = ExtractIntArray(cardObj, "input3");
            int[] input4 = ExtractIntArray(cardObj, "input4");

            long[] keywordIndex = ExtractLongArray(cardObj, "keywordIndex");
            long[] cardEffectIndex = ExtractLongArray(cardObj, "cardEffectIndex");
            long[] placeAnimationIndex = ExtractLongArray(cardObj, "placeAnimationIndex");
            long[] actionAnimationIndex = ExtractLongArray(cardObj, "actionAnimationIndex");

            var newCard = new ClassManager.Card.CardClass
            {
                Index = TryParseLong(cardObj["index"]?.ToString(), 0),
                NameIndex = TryParseLong(cardObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TryParseLong(cardObj["explainIndex"]?.ToString(), 0),

                CardType = (ClassManager.Card.CardClass.Type)TryParseInt(cardObj["type"]?.ToString(), -1),
                CardRank = (ClassManager.Card.CardClass.Rank)TryParseInt(cardObj["rank"]?.ToString(), -1),

                Cost = TryParseInt(cardObj["cost"]?.ToString(), 0),
                KeyWordIndex = keywordIndex,
                SpritePath = cardObj["spritePath"]?.ToString() ?? "",

                AvailableRoutes = availableRoutes,
                DestroyPayback = TryParseInt(cardObj["destroyPayback"]?.ToString(), 0),
                CardEffectIndex = cardEffectIndex,
                PlaceAnimationIndex = placeAnimationIndex,
                ActionAnimationIndex = actionAnimationIndex,

                input1 = input1,
                input2 = input2,
                input3 = input3,
                input4 = input4
            };

            cardDataList.Add(newCard);
        }

        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
    }
    private int[] ExtractIntArray(JObject obj, string baseKey)
    {
        List<int> values = new List<int>();

        if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
        {
            if (int.TryParse(singleValue.ToString(), out int parsedValue))
            {
                values.Add(parsedValue);
            }
        }

        foreach (var property in obj.Properties())
        {
            if (property.Name.StartsWith(baseKey + "_"))
            {
                if (int.TryParse(property.Value.ToString(), out int parsedValue))
                {
                    values.Add(parsedValue);
                }
            }
        }

        return values.ToArray();
    }

    private long[] ExtractLongArray(JObject obj, string baseKey)
    {
        List<long> values = new List<long>();

        if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
        {
            if (long.TryParse(singleValue.ToString(), out long parsedValue))
            {
                values.Add(parsedValue);
            }
        }

        foreach (var property in obj.Properties())
        {
            if (property.Name.StartsWith(baseKey + "_"))
            {
                if (long.TryParse(property.Value.ToString(), out long parsedValue))
                {
                    values.Add(parsedValue);
                }
            }
        }

        return values.ToArray();
    }


    private int TryParseInt(string value, int defaultValue)
    {
        return int.TryParse(value, out int result) ? result : defaultValue;
    }

    private long TryParseLong(string value, long defaultValue)
    {
        return long.TryParse(value, out long result) ? result : defaultValue;
    }
}
