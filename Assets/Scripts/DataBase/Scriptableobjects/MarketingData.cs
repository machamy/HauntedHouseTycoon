using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class MarketingData : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.MarketingData>
{
    [SerializeField]
    public List<ClassBase.Card.MarketingData> marketingDataList = new();

    public ClassBase.Card.MarketingData FindByIndex(long index)
    {
        return marketingDataList.FirstOrDefault(market => market.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON 경로가 존재하지 않음: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray marketArray = JArray.Parse(json);

        marketingDataList.Clear();

        foreach  (JObject marketObj in marketArray)
        {
            var market = new ClassBase.Card.MarketingData
            {
                Index = TypeConverter.TryParseLong(marketObj["index"]?.ToString(), 0)
            };
        }
    }
}