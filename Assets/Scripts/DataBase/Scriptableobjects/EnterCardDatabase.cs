using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class EnterCardDatabase : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.EnterCardData>
{
    [SerializeField]
    public List<ClassBase.Card.EnterCardData> enterCardDataList = new();

    public ClassBase.Card.EnterCardData FindByIndex(long index)
    {
        return enterCardDataList.FirstOrDefault(enterCard => enterCard.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON ��ΰ� �������� ����: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray enterCardArray = JArray.Parse(json);

        enterCardDataList.Clear();

        foreach (JObject enterCardObj in enterCardArray)
        {
            long[] spawningVisitorIndex = TypeConverter.ExtractLongArray(enterCardObj, "spawningVisitorIndex");

            var enterCard = new ClassBase.Card.EnterCardData
            {
                Index = TypeConverter.TryParseLong(enterCardObj["index"]?.ToString(), 0),
                EffectLastsTurn = TypeConverter.TryParseInt(enterCardObj["effectLastsTrun"]?.ToString(), 0),
                SpawningVisitorIndex = spawningVisitorIndex
            };

            enterCardDataList.Add(enterCard);
        }
    }
}