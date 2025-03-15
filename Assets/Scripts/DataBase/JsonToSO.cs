using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class JsonToSo
{
    private static string jsonFilenameAnimationData = "Assets/Scripts/DataBase/CardDataBase_JSON/AnimationData.json";
    private static string jsonFileNameCardData = "Assets/Scripts/DataBase/CardDataBase_JSON/CardData.json";
    private static string jsonFileNameCardEffectData = "Assets/Scripts/DataBase/CardDataBase_JSON/CardEffectData.json";
    private static string jsonFileNameCardpackData = "Assets/Scripts/DataBase/CardDataBase_JSON/CardpackData.json";
    private static string jsonFileNameEnterCardData = "Assets/Scripts/DataBase/CardDataBase_JSON/EnterCardData.json";
    private static string jsonFileNameKeywordData = "Assets/Scripts/DataBase/CardDataBase_JSON/KeyWordData.json";
    private static string jsonFileNameMarketingData = "Assets/Scripts/DataBase/CardDataBase_JSON/MarketingData.json";
    private static string jsonFileNameTextData = "Assets/Scripts/DataBase/CardDataBase_JSON/TextData.json";
    private static string jsonFileNameTraumaData = "Assets/Scripts/DataBase/CardDataBase_JSON/TraumaData.json";
    private static string jsonFileNameVisitorData = "Assets/Scripts/DataBase/CardDataBase_JSON/VisitorData.json";

    private static Dictionary<string, ScriptableObject> soDict = new Dictionary<string, ScriptableObject>();

    [MenuItem("Tools/Generate GenerateAllCardSO from JSON")]
    public static void GenerateAllCardSO()
    {
        GenerateAllCardSOInternal();
    }

    public static void GenerateAllCardSOForAPI()
    {
        GenerateAllCardSOInternal();
        SaveAllAssetsForAPI();
    }

    private static void GenerateAllCardSOInternal()
    {
        Debug.Log("[JsonToSO] 실행 시작!");

        Dictionary<string, Type> scriptableObjectTypes = new Dictionary<string, Type>
        {
            { "AnimationData", typeof(AnimationDataSO) },
            { "CardDataBase", typeof(CardDataBaseSO) },
            { "CardEffectData", typeof(CardEffectDataSO) },
            { "CardpackData", typeof(CardpackDataSO) },
            { "EnterCardData", typeof(EnterCardDataSO) },
            { "KeywordData", typeof(KeywordDataSO) },
            { "MarketingData", typeof(MarketingDataSO) },
            { "TextData", typeof(TextDataSO) },
            { "TraumaData", typeof(TraumaDataSO) },
            { "VisitorData", typeof(VisitorDataSO) }
        };

        Dictionary<string, string> jsonFiles = new Dictionary<string, string>
        {
            { "AnimationData", jsonFilenameAnimationData },
            { "CardDataBase", jsonFileNameCardData },
            { "CardEffectData", jsonFileNameCardEffectData },
            { "CardpackData", jsonFileNameCardpackData },
            { "EnterCardData", jsonFileNameEnterCardData },
            { "KeywordData", jsonFileNameKeywordData },
            { "MarketingData", jsonFileNameMarketingData },
            { "TextData", jsonFileNameTextData },
            { "TraumaData", jsonFileNameTraumaData },
            { "VisitorData", jsonFileNameVisitorData }
        };

        foreach (var kvp in scriptableObjectTypes)
        {
            string assetPath = $"Assets/Scripts/DataBase/ScriptableObjects/{kvp.Key}.asset";
            ScriptableObject so = null;

#if UNITY_EDITOR
            so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if (so == null)
            {
                so = ScriptableObject.CreateInstance(kvp.Value);
                AssetDatabase.CreateAsset(so, assetPath);
                Debug.Log($"[JsonToSO] {kvp.Key} SO가 생성되었습니다: {assetPath}");
            }
#else
        so = LoadOrCreateScriptableObject(kvp.Key, kvp.Value);
#endif

            soDict[kvp.Key] = so;

            if (jsonFiles.ContainsKey(kvp.Key))
            {
                LoadJsonData(so, jsonFiles[kvp.Key]);
            }
        }

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[JsonToSO] 모든 데이터가 업데이트되었습니다.");
#endif
    }

    private static void SaveAllAssetsForAPI()
    {
#if !UNITY_EDITOR
    Debug.Log("[JsonToSO API] API 환경에서 ScriptableObject 저장");

    foreach (var kvp in soDict)
    {
        string savePath = Path.Combine(Application.persistentDataPath, $"{kvp.Key}.json");
        string jsonData = JsonUtility.ToJson(kvp.Value, true);
        
        File.WriteAllText(savePath, jsonData);

        Debug.Log($"[JsonToSO API] {kvp.Key} ScriptableObject 데이터가 {savePath}에 저장되었습니다.");
    }

    Debug.Log("[JsonToSO API] 모든 ScriptableObject가 저장되었습니다.");
#endif
    }

    private static ScriptableObject LoadOrCreateScriptableObject(string key, Type type)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, $"{key}.json");

        if (File.Exists(loadPath))
        {
            string jsonData = File.ReadAllText(loadPath);
            ScriptableObject so = ScriptableObject.CreateInstance(type);
            JsonUtility.FromJsonOverwrite(jsonData, so);
            Debug.Log($"[JsonToSO API] {key} ScriptableObject가 로드되었습니다.");
            return so;
        }
        else
        {
            Debug.Log($"[JsonToSO API] {key} ScriptableObject가 존재하지 않아 새로 생성됩니다.");
            return ScriptableObject.CreateInstance(type);
        }
    }

    private static void LoadJsonData(ScriptableObject so, string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            var method = so.GetType().GetMethod("LoadFromJSON");
            if (method != null)
            {
                method.Invoke(so, new object[] { jsonFilePath });
                Debug.Log($"[JsonToSO] {jsonFilePath} 데이터를 {so.GetType().Name}에 적용했습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"[JsonToSO] {jsonFilePath} 파일이 존재하지 않습니다.");
        }
    }
}
