using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "VisitorDataSO")]
public class VisitorDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.GameObject.Visitor> visitorDataList = new List<ClassBase.GameObject.Visitor>();


    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray visitorDataArray = JArray.Parse(json);

        visitorDataList.Clear();
        foreach(JToken token in visitorDataArray)
        {
            JObject visitorDataObj = (JObject)token;

            long[] traumaIndex = TypeConverter.ExtractLongArray(visitorDataObj, "traumaIndex");
            long[] enterCardIndex = TypeConverter.ExtractLongArray(visitorDataObj, "enterCardIndex");

            float[] traumaRatio = TypeConverter.ExtractFloatArray(visitorDataObj, "traumaRatio");

            int[] requiredHorrorAmount = TypeConverter.ExtractIntArray(visitorDataObj, "requiredHorrorAmount");
            int[] panicWeightedAmount = TypeConverter.ExtractIntArray(visitorDataObj, "panicWeightedAmount");

            var newVisitorData = new ClassBase.GameObject.Visitor
            {
                Index = TypeConverter.TryParseLong(visitorDataObj["index"]?.ToString(), 0),

                sex = (ClassBase.GameObject.Visitor.Sex)TypeConverter.TryParseInt(visitorDataObj["sex"]?.ToString(), -1),

                Age = TypeConverter.TryParseInt(visitorDataObj["age"]?.ToString(), 0),
                TraumaIndex = traumaIndex,
                TraumaRatio = traumaRatio,
                VisualHorrorTolerance = TypeConverter.TryParseFloat(visitorDataObj["visualHorrorTolerance"]?.ToString(), 0),
                AuditoryHorrorTolerance = TypeConverter.TryParseFloat(visitorDataObj["auditoryHorrorTolerance"]?.ToString(), 0),
                ScentHorrorTolerance = TypeConverter.TryParseFloat(visitorDataObj["scentHorrorTolerance"]?.ToString(), 0),
                TouchHorrorTolerance = TypeConverter.TryParseFloat(visitorDataObj["touchHorrorTolerance"]?.ToString(), 0),
                RequiredHorrorAmount = requiredHorrorAmount,
                PanicAmount = TypeConverter.TryParseInt(visitorDataObj["panicAmount"]?.ToString(), 0),
                AmountOfTiredInTurn = TypeConverter.TryParseFloat(visitorDataObj["amountOfTiredInTurn"]?.ToString(), 0),

                panicResponse = (ClassBase.GameObject.Visitor.PanicResponse)TypeConverter.TryParseInt(visitorDataObj["panicResponse"]?.ToString(), -1),

                PanicWeightedAmount = panicWeightedAmount,
                ExitGetScreamAmount = TypeConverter.TryParseInt(visitorDataObj["exitGetScreamAmount"]?.ToString(), 0),
                EnterCardIndex = enterCardIndex
            };

            visitorDataList.Add(newVisitorData);
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
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(visitorDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}