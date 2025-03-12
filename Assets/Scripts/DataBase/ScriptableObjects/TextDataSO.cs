using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "TextDataSO")]
public class TextDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.TextData> textDataList = new List<ClassManager.Card.TextData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray textDataArray = JArray.Parse(json);

        textDataList.Clear();

        foreach(JToken token in textDataArray)
        {
            JObject textDataObj = (JObject)token;

            var newTextData = new ClassManager.Card.TextData
            {
                Index = TryParseLong(textDataObj["index"]?.ToString(), 0),
                StringKey = textDataObj["StringKey"]?.ToString() ?? "",
                KOR = textDataObj["KOR"]?.ToString() ?? "",
                EN = textDataObj["EN"]?.ToString() ?? ""
            };

            textDataList.Add(newTextData);
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