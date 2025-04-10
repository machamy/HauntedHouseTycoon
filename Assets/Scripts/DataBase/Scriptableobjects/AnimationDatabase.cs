using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonFunction.TypeConversion;
using System.Linq;

public class AnimationDatabase : ScriptableObject, JsonToSO.ILoadFromJson, JsonToSO.IIndexedData<ClassBase.Card.AnimationData>
{
    [SerializeField]
    public List<ClassBase.Card.AnimationData> animationDataList = new();

    public ClassBase.Card.AnimationData FindByIndex(long index)
    {
        return animationDataList.FirstOrDefault(aniData => aniData.Index == index);
    }

    public void LoadFromJson(string jsonPath)
    {
        if(!File.Exists(jsonPath))
        {
            Debug.LogError("JSON 경로가 존재하지 않음: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        JArray aniDataArray = JArray.Parse(json);

        animationDataList.Clear();

        foreach (JObject aniObj in aniDataArray)
        {
            var aniData = new ClassBase.Card.AnimationData
            {
                Index = TypeConverter.TryParseLong(aniObj["index"]?.ToString(), 0)
            };

            animationDataList.Add(aniData);
        }
    }
}