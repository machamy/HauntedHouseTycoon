using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class TextDatabase : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.TextData>
{
    [SerializeField]
    public List<ClassBase.Card.TextData> textDataList = new();

    public ClassBase.Card.TextData FindByIndex(long index)
    {
        return textDataList.FirstOrDefault(text => text.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON ��ΰ� �������� ����: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray textArray = JArray.Parse(json);

        textDataList.Clear();

        foreach(JObject textObj in textArray)
        {
            string stringKey = textObj["stringKey"]?.ToString() ?? "";
            string kor = textObj["KOR"]?.ToString() ?? "";
            string en = textObj["EN"]?.ToString() ?? "";
            var text = new ClassBase.Card.TextData
            {
                StringKey = stringKey,
                KOR = kor,
                EN = en
            };

            textDataList.Add(text);
        }
    }
}