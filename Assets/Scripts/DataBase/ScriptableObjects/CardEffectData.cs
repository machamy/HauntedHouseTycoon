using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;



[CreateAssetMenu(menuName = "CardEffect DataSO")]
public class CardEffectDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.Effect> cardEffectDataList = new List<ClassBase.Card.Effect>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray cardEffectDataArray = JArray.Parse(json);

        cardEffectDataList.Clear();

        foreach (JToken token in cardEffectDataArray)
        {
            JObject cardEffectDataobj = (JObject)token;

            var newCardEffectData = new ClassBase.Card.Effect
            {
               Index = TypeConverter.TryParseLong(cardEffectDataobj["index"]?.ToString(), 0),
               condition = (ClassBase.Card.Effect.ConditonType)TypeConverter.TryParseInt(cardEffectDataobj["conditionType"]?.ToString(), -1),
               target = (ClassBase.Card.Effect.TargetType)TypeConverter.TryParseInt(cardEffectDataobj["targetType"]?.ToString(), -1)
            };

            cardEffectDataList.Add(newCardEffectData);
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
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(cardEffectDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}