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
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            if (so == null)
            {
                so = ScriptableObject.CreateInstance(kvp.Value);
                AssetDatabase.CreateAsset(so, assetPath);
                Debug.Log($"[JsonToSO] {kvp.Key} SO가 생성되었습니다: {assetPath}");
            }

            soDict[kvp.Key] = so;

            if (jsonFiles.ContainsKey(kvp.Key))
            {
                LoadJsonData(so, jsonFiles[kvp.Key]);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[JsonToSO] 모든 데이터가 업데이트되었습니다.");
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
