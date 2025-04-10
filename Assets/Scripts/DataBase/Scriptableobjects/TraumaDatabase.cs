using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class TraumaDatabase : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.TraumaData>
{
    [SerializeField]
    public List<ClassBase.Card.TraumaData> traumaDataList = new();

    public ClassBase.Card.TraumaData FindByIndex(long index)
    {
        return traumaDataList.FirstOrDefault(traum => traum.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON ��ΰ� �������� ����: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray traumaArray = JArray.Parse(json);

        traumaDataList.Clear();

        foreach(JObject traumaObj in traumaArray)
        {
            string traumaName = traumaObj["traumaName"]?.ToString() ?? "";
            var trauma = new ClassBase.Card.TraumaData
            {
                Index = TypeConverter.TryParseLong(traumaObj["index"]?.ToString(), 0),
                TraumaName = traumaName
            };

            traumaDataList.Add(trauma);
        }
    }
}