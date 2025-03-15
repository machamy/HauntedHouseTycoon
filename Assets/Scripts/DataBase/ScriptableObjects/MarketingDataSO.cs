using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "MarketingDataSO")]
public class MarketingDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.MarketingData> marketingDataList = new List<ClassBase.Card.MarketingData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray marketingDataArray = JArray.Parse(json);

        marketingDataList.Clear();

        foreach(JToken token in marketingDataArray )
        {
            JObject marketingDataObj = (JObject)token;

            var newMarketingData = new ClassBase.Card.MarketingData
            {
                Index = TypeConverter.TryParseLong(marketingDataObj["index"]?.ToString(), 0),
            };

            marketingDataList.Add(newMarketingData);
        }
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
#else
        SaveForAPI();
#endif
    }
    private void SaveForAPI()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "SavedAnimationData.json");
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(marketingDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}