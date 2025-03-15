using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using CommonFunction.TypeConversion;

[CreateAssetMenu(menuName = "AnimationSO")]
public class AnimationDataSO : ScriptableObject
{
    [SerializeField] public List<ClassBase.Card.AnimationData> animationDataList = new List<ClassBase.Card.AnimationData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);

        JArray animationDataArray = JArray.Parse(json);

        animationDataList.Clear();

        foreach (JToken token in animationDataArray)
        {
            JObject animationDataobj = (JObject)token;

            var newAnimationData = new ClassBase.Card.AnimationData
            {
                Index = TypeConverter.TryParseLong(animationDataobj["index"]?.ToString(), 0),
                EffectSound = animationDataobj["effectSound"]?.ToString() ?? ""
            };
            animationDataList.Add(newAnimationData);
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
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(animationDataList, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }
}