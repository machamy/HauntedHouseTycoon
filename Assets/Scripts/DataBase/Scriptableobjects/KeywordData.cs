using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class KeywordData : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.KeywordData>
{
    [SerializeField]
    public List<ClassBase.Card.KeywordData> keywordDataList = new();

    public ClassBase.Card.KeywordData FindByIndex(long index)
    {
        return keywordDataList.FirstOrDefault(keyword => keyword.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON 경로가 존재하지 않음: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray keywordArray = JArray.Parse(json);

        keywordDataList.Clear();

        foreach(JObject keywordObj in keywordArray)
        {
            var keyword = new ClassBase.Card.KeywordData
            {
                Index = TypeConverter.TryParseLong(keywordObj["index"]?.ToString(), 0),
                NameIndex = TypeConverter.TryParseLong(keywordObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TypeConverter.TryParseLong(keywordObj["explainIndex"]?.ToString(), 0)
            };
        }
    }
}