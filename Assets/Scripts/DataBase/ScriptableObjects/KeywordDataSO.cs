using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "KeywordDataSO")]
public class KeywordDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.KeywordData> keywordDataList = new List<ClassManager.Card.KeywordData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray keywordDataArray = JArray.Parse(json);

        keywordDataList.Clear();

        foreach(JToken token in keywordDataArray)
        {
            JObject keywordDataObj = (JObject)token;

            var newKeywordData = new ClassManager.Card.KeywordData
            {
                Index = TryParseLong(keywordDataObj["index"]?.ToString(), 0),
                NameIndex = TryParseLong(keywordDataObj["nameIndex"]?.ToString(), 0),
                ExplainIndex = TryParseLong(keywordDataObj["explainIndex"]?.ToString(), 0)
            };

            keywordDataList.Add(newKeywordData);
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