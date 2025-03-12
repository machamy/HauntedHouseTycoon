using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "MarketingDataSO")]
public class MarketingDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.MarketingData> marketingDataList = new List<ClassManager.Card.MarketingData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray marketingDataArray = JArray.Parse(json);

        marketingDataList.Clear();

        foreach(JToken token in marketingDataArray )
        {
            JObject marketingDataObj = (JObject)token;

            var newMarketingData = new ClassManager.Card.MarketingData
            {
                Index = TryParseLong(marketingDataObj["index"]?.ToString(), 0),
            };

            marketingDataList.Add(newMarketingData);
        }

        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
    }

    private long TryParseLong(string value, long defaultValue)
    {
        return long.TryParse(value, out long result) ? result : defaultValue;
    }
}