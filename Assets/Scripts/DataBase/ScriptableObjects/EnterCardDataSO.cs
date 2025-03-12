using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "EnterCardDataSO")]
public class EnterCardDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.EnterCardData> enterCardDataList = new List<ClassManager.Card.EnterCardData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray enterCardDataArray = JArray.Parse(json);

        enterCardDataList.Clear();

        foreach(JToken token in enterCardDataArray)
        {
            JObject enterCardDataObj = (JObject)token;

            long[] spawningVisitorIndex = ExtractLongArray(enterCardDataObj, "spawningVisitorIndex");

            var newEnterCardData = new ClassManager.Card.EnterCardData
            {
                Index = TryParseLong(enterCardDataObj["index"]?.ToString(), 0),
                EffectLastsTurn = TryParseInt(enterCardDataObj["effectLastsTurn"]?.ToString(), 0),
                SpawningVisitorIndex = spawningVisitorIndex
            };

            enterCardDataList.Add(newEnterCardData);
        }

        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
    }

    private long[] ExtractLongArray(JObject obj, string baseKey)
    {
        List<long> values = new List<long>();

        if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
        {
            if (long.TryParse(singleValue.ToString(), out long parsedValue))
            {
                values.Add(parsedValue);
            }
        }

        foreach (var property in obj.Properties())
        {
            if (property.Name.StartsWith(baseKey + "_"))
            {
                if (long.TryParse(property.Value.ToString(), out long parsedValue))
                {
                    values.Add(parsedValue);
                }
            }
        }

        return values.ToArray();
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