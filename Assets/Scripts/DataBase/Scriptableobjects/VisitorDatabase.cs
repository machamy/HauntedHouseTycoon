using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;
using JetBrains.Annotations;

public class VisitorDatabase : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.GameObject.Visitor>
{
    [SerializeField]
    public List<ClassBase.GameObject.Visitor> visitorDataList = new();

    public ClassBase.GameObject.Visitor FindByIndex(long index)
    {
        return visitorDataList.FirstOrDefault(visitor => visitor.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON ��ΰ� �������� ����: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray visitorArray = JArray.Parse(json);

        visitorDataList.Clear();

        foreach (JObject visitorObj in visitorArray)
        {
            long[] traumaIndex = TypeConverter.ExtractLongArray(visitorObj, "traumaIndex");
            float[] traumaRatio = TypeConverter.ExtractFloatArray(visitorObj, "traumaRatio");
            int[] requiredHorrorAmount = TypeConverter.ExtractIntArray(visitorObj, "requiredHorrorAmount");
            int[] panicWeightedAmount = TypeConverter.ExtractIntArray(visitorObj, "panicWeightedAmount");
            long[] enterCardIndex = TypeConverter.ExtractLongArray(visitorObj, "enterCardIndex");

            var visitor = new ClassBase.GameObject.Visitor
            {
                Index = TypeConverter.TryParseLong(visitorObj["index"]?.ToString(), 0),
                sex = (ClassBase.GameObject.Visitor.Sex)TypeConverter.TryParseInt(visitorObj["sex"]?.ToString(), 0),
                Age = TypeConverter.TryParseInt(visitorObj["age"]?.ToString(), 0),
                TraumaIndex = traumaIndex,
                TraumaRatio = traumaRatio,
                VisualHorrorTolerance = TypeConverter.TryParseFloat(visitorObj["visualHorrorTolerance"]?.ToString(), 0f),
                AuditoryHorrorTolerance = TypeConverter.TryParseFloat(visitorObj["auditoryHorrorTolerance"]?.ToString(), 0f),
                ScentHorrorTolerance = TypeConverter.TryParseFloat(visitorObj["scentHorrorTolerance"]?.ToString(), 0f),
                TouchHorrorTolerance = TypeConverter.TryParseFloat(visitorObj["touchHorrorTolerance"]?.ToString(), 0f),
                RequiredHorrorAmount = TypeConverter.ExtractIntArray(visitorObj, "requiredHorrorAmount"),
                PanicAmount = TypeConverter.TryParseInt(visitorObj["panicAmount"]?.ToString(), 0),
                AmountOfTiredInTurn = TypeConverter.TryParseFloat(visitorObj["amountOfTiredInTurn"]?.ToString(), 0f),
                panicResponse = (ClassBase.GameObject.Visitor.PanicResponse)TypeConverter.TryParseInt(visitorObj["panicResponse"]?.ToString(), 0),
                PanicWeightedAmount = panicWeightedAmount,
                ExitGetScreamAmount = TypeConverter.TryParseInt(visitorObj["exitGetScreamAmount"]?.ToString(), 0),
                EnterCardIndex = enterCardIndex
            };

            visitorDataList.Add(visitor);
        }
    }
}