using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "KeywordDataSO")]
public class KeywordDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.KeywordData> keywordDataList = new List<ClassBase.Card.KeywordData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray keywordDataArray = JArray.Parse(json);

        keywordDataList.Clear();

        foreach(JToken token in keywordDataArray)
        {
            JObject keywordDataObj = (JObject)token;

            var newKeywordData = new ClassBase.Card.KeywordData
            {
                Index = TypeConverter.TryParseLong(keywordDataObj["index"]?.ToString(), 0),
                NameIndex = TypeConverter.TryParseLong(keywordDataObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TypeConverter.TryParseLong(keywordDataObj["explainIndex"]?.ToString(), 0)
            };

            keywordDataList.Add(newKeywordData);
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
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(keywordDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}