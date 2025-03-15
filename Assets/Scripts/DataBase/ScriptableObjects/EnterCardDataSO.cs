using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "EnterCardDataSO")]
public class EnterCardDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.EnterCardData> enterCardDataList = new List<ClassBase.Card.EnterCardData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray enterCardDataArray = JArray.Parse(json);

        enterCardDataList.Clear();

        foreach(JToken token in enterCardDataArray)
        {
            JObject enterCardDataObj = (JObject)token;

            long[] spawningVisitorIndex = TypeConverter.ExtractLongArray(enterCardDataObj, "spawningVisitorIndex");

            var newEnterCardData = new ClassBase.Card.EnterCardData
            {
                Index = TypeConverter.TryParseLong(enterCardDataObj["index"]?.ToString(), 0),
                EffectLastsTurn = TypeConverter.TryParseInt(enterCardDataObj["effectLastsTurn"]?.ToString(), 0),
                SpawningVisitorIndex = spawningVisitorIndex
            };

            enterCardDataList.Add(newEnterCardData);
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
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(enterCardDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}