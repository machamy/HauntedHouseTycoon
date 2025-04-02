using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class PackData : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.CardPackData>
{
    [SerializeField]
    public List<ClassBase.Card.CardPackData> packDataList = new();

    public ClassBase.Card.CardPackData FindByIndex(long index)
    {
        return packDataList.FirstOrDefault(cardpack => cardpack.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON 경로가 존재하지 않음: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray packArray = JArray.Parse(json);

        packDataList.Clear();

        foreach (JObject packObj in packArray)
        {
            int[] weightedRatioForEachCards = TypeConverter.ExtractIntArray(packObj, "weightedRatioForEachCards");

            long[] appearingCardIndex = TypeConverter.ExtractLongArray(packObj, "appearingCardIndex");

            string illustFileName = packObj["illustFileName"]?.ToString() ?? "";

            var packData = new ClassBase.Card.CardPackData
            {
                Index = TypeConverter.TryParseLong(packObj["index"]?.ToString(), 0),
                NameIndex = TypeConverter.TryParseLong(packObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TypeConverter.TryParseLong(packObj["explainIndex"]?.ToString(), 0),
                PackPrice = TypeConverter.TryParseInt(packObj["packPrice"]?.ToString(), 0),
                NumberOfCardsAppearingUponOpening = TypeConverter.TryParseInt(packObj["numberOfCardsAppearingUponOpening"]?.ToString(), 0),
                AppearingCardIndex = appearingCardIndex,
                WeightedRatioForEachCards = weightedRatioForEachCards,
                RepurchaseAllowed = TypeConverter.TryParseBool(packObj["repurchaseAllowed"]?.ToString(), false),
                RepurchaseAllowedForOnce = TypeConverter.TryParseBool(packObj["repurchaseAllowedForOnce"]?.ToString(), false),
                MaximumOpenedPackAmount = TypeConverter.TryParseInt(packObj["maximumOpenedPackAmount"]?.ToString(), 0),
                PackOpeningAnimation = TypeConverter.TryParseLong(packObj["PackOpeningAnimation"]?.ToString(), 0),
                IllustFileName = illustFileName
            };

            packDataList.Add(packData);
        }
    }
}