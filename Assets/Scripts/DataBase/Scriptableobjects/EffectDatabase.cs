using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class EffectDatabase : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.Effect>
{
    [SerializeField]
    public List<ClassBase.Card.Effect> effectDataList = new();

    public ClassBase.Card.Effect FindByIndex(long index)
    {
        return effectDataList.FirstOrDefault(effect => effect.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if(!File.Exists(jsonPath))
        {
            Debug.LogError("JSON ��ΰ� �������� ����: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray effectArray = JArray.Parse(json);

        effectDataList.Clear();

        foreach(JObject effectObj in effectArray)
        {
            var effect = new ClassBase.Card.Effect
            {
                Index = TypeConverter.TryParseLong(effectObj["index"]?.ToString(), 0),
                condition = (ClassBase.Card.Effect.ConditonType)TypeConverter.TryParseInt(effectObj["conditionType"]?.ToString(), 0),
                target = (ClassBase.Card.Effect.TargetType)TypeConverter.TryParseInt(effectObj["targetType"]?.ToString(), 0),
            };

            effectDataList.Add(effect);
        }
    }
}