using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "VisitorDataSO")]
public class VisitorDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.Visitor> visitorDataList = new List<ClassManager.Card.Visitor>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray visitorDataArray = JArray.Parse(json);

        visitorDataList.Clear();
        foreach(JToken token in visitorDataArray)
        {
            JObject visitorDataObj = (JObject)token;

            long[] traumaIndex = ExtractLongArray(visitorDataObj, "traumaIndex");
            long[] enterCardIndex = ExtractLongArray(visitorDataObj, "enterCardIndex");

            float[] traumaRatio = ExtractFloatArray(visitorDataObj, "traumaRatio");

            int[] requiredHorrorAmount = ExtractIntArray(visitorDataObj, "requiredHorrorAmount");
            int[] panicWeightedAmount = ExtractIntArray(visitorDataObj, "panicWeightedAmount");

            var newVisitorData = new ClassManager.Card.Visitor
            {
                Index = TryParseLong(visitorDataObj["index"]?.ToString(), 0),

                sex = (ClassManager.Card.Visitor.Sex)TryParseInt(visitorDataObj["sex"]?.ToString(), -1),

                Age = TryParseInt(visitorDataObj["age"]?.ToString(), 0),
                TraumaIndex = traumaIndex,
                TraumaRatio = traumaRatio,
                VisualHorrorTolerance = TryParseFloat(visitorDataObj["visualHorrorTolerance"]?.ToString(), 0),
                AuditoryHorrorTolerance = TryParseFloat(visitorDataObj["auditoryHorrorTolerance"]?.ToString(), 0),
                ScentHorrorTolerance = TryParseFloat(visitorDataObj["scentHorrorTolerance"]?.ToString(), 0),
                TouchHorrorTolerance = TryParseFloat(visitorDataObj["touchHorrorTolerance"]?.ToString(), 0),
                RequiredHorrorAmount = requiredHorrorAmount,
                PanicAmount = TryParseInt(visitorDataObj["panicAmount"]?.ToString(), 0),
                AmountOfTiredInTurn = TryParseFloat(visitorDataObj["amountOfTiredInTurn"]?.ToString(), 0),

                panicResponse = (ClassManager.Card.Visitor.PanicResponse)TryParseInt(visitorDataObj["panicResponse"]?.ToString(), -1),

                PanicWeightedAmount = panicWeightedAmount,
                ExitGetScreamAmount = TryParseInt(visitorDataObj["exitGetScreamAmount"]?.ToString(), 0),
                EnterCardIndex = enterCardIndex
            };

            visitorDataList.Add(newVisitorData);
        }

        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
    }

    private int[] ExtractIntArray(JObject obj, string baseKey)
    {
        List<int> values = new List<int>();

        if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
        {
            if (int.TryParse(singleValue.ToString(), out int parsedValue))
            {
                values.Add(parsedValue);
            }
        }

        foreach (var property in obj.Properties())
        {
            if (property.Name.StartsWith(baseKey + "_"))
            {
                if (int.TryParse(property.Value.ToString(), out int parsedValue))
                {
                    values.Add(parsedValue);
                }
            }
        }

        return values.ToArray();
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

    private float[] ExtractFloatArray(JObject obj, string baseKey)
    {
        List<float> values = new List<float>();

        if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
        {
            if (float.TryParse(singleValue.ToString(), out float parsedValue))
            {
                values.Add(parsedValue);
            }
        }

        foreach (var property in obj.Properties())
        {
            if (property.Name.StartsWith(baseKey + "_"))
            {
                if (float.TryParse(property.Value.ToString(), out float parsedValue))
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

    private float TryParseFloat(string value, float defaultValue)
    {
        return float.TryParse(value, out float result) ? result : defaultValue;
    }
}