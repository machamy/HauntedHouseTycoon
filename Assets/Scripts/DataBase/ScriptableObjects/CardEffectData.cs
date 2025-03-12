using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "CardEffect DataSO")]
public class CardEffectDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.Effect> cardEffectDataList = new List<ClassManager.Card.Effect>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray cardEffectDataArray = JArray.Parse(json);

        cardEffectDataList.Clear();

        foreach (JToken token in cardEffectDataArray)
        {
            JObject cardEffectDataobj = (JObject)token;

            var newCardEffectData = new ClassManager.Card.Effect
            {
               Index = TryParseLong(cardEffectDataobj["index"]?.ToString(), 0),
               condition = (ClassManager.Card.Effect.ConditonType)TryParseInt(cardEffectDataobj["conditionType"]?.ToString(), -1),
               target = (ClassManager.Card.Effect.TargetType)TryParseInt(cardEffectDataobj["targetType"]?.ToString(), -1)
            };

            cardEffectDataList.Add(newCardEffectData);
        }

        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
    }

    private int TryParseInt(string value, int defaultValue)
    {
        return int.TryParse(value, out int result) ? result : defaultValue;
    }

    private long TryParseLong(string value, long defaultValue)
    {
        return long.TryParse(value, out long result) ? result : defaultValue;

    }
}