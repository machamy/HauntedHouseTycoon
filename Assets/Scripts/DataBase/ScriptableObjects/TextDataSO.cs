using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "TextDataSO")]
public class TextDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.TextData> textDataList = new List<ClassBase.Card.TextData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray textDataArray = JArray.Parse(json);

        textDataList.Clear();

        foreach(JToken token in textDataArray)
        {
            JObject textDataObj = (JObject)token;

            var newTextData = new ClassBase.Card.TextData
            {
                Index = TypeConverter.TryParseLong(textDataObj["index"]?.ToString(), 0),
                StringKey = textDataObj["StringKey"]?.ToString() ?? "",
                KOR = textDataObj["KOR"]?.ToString() ?? "",
                EN = textDataObj["EN"]?.ToString() ?? ""
            };

            textDataList.Add(newTextData);
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
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(textDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}