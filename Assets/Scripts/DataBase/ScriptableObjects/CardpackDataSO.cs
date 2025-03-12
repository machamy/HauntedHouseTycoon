using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "CardpackDataSO")]
public class CardpackDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.CardPackData> cardpackDataList = new List<ClassManager.Card.CardPackData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray cardpackDataArray = JArray.Parse(json);

        cardpackDataList.Clear();

        foreach(JToken token in cardpackDataArray)
        {
            JObject cardpackDataObj = (JObject)token;

            long[] appearingCardIndex = ExtractLongArray(cardpackDataObj, "appearingCardIndex");

            int[] weightedRatioForEachCards = ExtractIntArray(cardpackDataObj, "weightedRatioForEachCards");

            var newcardpackData = new ClassManager.Card.CardPackData
            {
                Index = TryParseLong(cardpackDataObj["index"]?.ToString(), 0),
                NameIndex = TryParseLong(cardpackDataObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TryParseLong(cardpackDataObj["explainIndex"]?.ToString(), 0),
                PackPrice = TryParseInt(cardpackDataObj["packPrice"]?.ToString(), 0),
                NumberOfCardsAppearingUponOpening = TryParseInt(cardpackDataObj["numberOfCardsaAppearingUponOning"]?.ToString(), 0),
                AppearingCardIndex = appearingCardIndex,
                WeightedRatioForEachCards = weightedRatioForEachCards,
                RepurchaseAllowed = TryParseBool(cardpackDataObj["repurchaseAllowed"]?.ToString(), false),
                RepurchaseAllowedForOnce = TryParseBool(cardpackDataObj["repurchaseAllowedForOnce"]?.ToString(), false),
                MaximumOpenedPackAmount = TryParseInt(cardpackDataObj["maximumOpenedPackAmount"]?.ToString(), 0),
                PackOpeningAnimation = TryParseLong(cardpackDataObj["packOpeningAnimation"]?.ToString(), 0),
                IllustFileName = cardpackDataObj["illustFileName"]?.ToString() ?? ""
            };

            cardpackDataList.Add(newcardpackData);
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

    private bool TryParseBool(string value, bool defaultValue)
    {
        return bool.TryParse(value, out bool result) ? result : defaultValue;
    }
}