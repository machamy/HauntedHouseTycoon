using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;

[CreateAssetMenu(menuName = "AnimationSO")]
public class AnimationDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.AnimationData> animtionDataList = new List<ClassManager.Card.AnimationData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray animationDataArray = JArray.Parse(json);

        animtionDataList.Clear();

        foreach (JToken token in animationDataArray)
        {
            JObject animationDataobj = (JObject)token;

            var newAnimationData = new ClassManager.Card.AnimationData
            {
                Index = TryParseLong(animationDataobj["index"]?.ToString(), 0),
                EffectSound = animationDataobj["effectSound"]?.ToString() ?? ""
            };
            animtionDataList.Add(newAnimationData);
            Debug.Log($"[AnimationDataSO] 애니메이션 데이터 추가됨 - Index: {newAnimationData.Index}, EffectSound: {newAnimationData.EffectSound}");
        }

        EditorApplication.delayCall += () =>
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };

        EditorUtility.SetDirty(this);
    }

    private long TryParseLong(string value, long defaultValue)
    {
        return long.TryParse(value, out long result) ? result : defaultValue;

    }
}