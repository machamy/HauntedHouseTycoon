using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "TraumaDataSO")]
public class TraumaDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.TraumaData> traumaDataList = new List<ClassBase.Card.TraumaData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray traumaDataArray = JArray.Parse(json);

        traumaDataList.Clear();

        foreach (JToken token in traumaDataArray)
        {
            JObject traumaDataObj = (JObject)token;

            var newTraumaData = new ClassBase.Card.TraumaData
            {
                Index = TypeConverter.TryParseLong(traumaDataObj["index"]?.ToString(), 0),
                TraumaName = traumaDataObj["traumaName"]?.ToString() ?? ""
            };
            traumaDataList.Add(newTraumaData);
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
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(traumaDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}

