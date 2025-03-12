using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "TraumaDataSO")]
public class TraumaDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.TraumaData> traumaDataList = new List<ClassManager.Card.TraumaData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray traumaDataArray = JArray.Parse(json);

        traumaDataList.Clear();

        foreach (JToken token in traumaDataArray)
        {
            JObject traumaDataObj = (JObject)token;

            var newTraumaData = new ClassManager.Card.TraumaData
            {
                Index = TryParseLong(traumaDataObj["index"]?.ToString(), 0),
                TraumaName = traumaDataObj["traumaName"]?.ToString() ?? ""
            };
            traumaDataList.Add(newTraumaData);
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

