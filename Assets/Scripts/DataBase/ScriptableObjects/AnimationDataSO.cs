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
    [SerializeField] public List<ClassManager.Card.AnimationData> animationDataList = new List<ClassManager.Card.AnimationData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray animationDataArray = JArray.Parse(json);

        animationDataList.Clear();

        foreach (JToken token in animationDataArray)
        {
            JObject animationDataobj = (JObject)token;

            var newAnimationData = new ClassManager.Card.AnimationData
            {
                Index = TryParseLong(animationDataobj["index"]?.ToString(), 0),
                EffectSound = animationDataobj["effectSound"]?.ToString() ?? ""
            };
            animationDataList.Add(newAnimationData);
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