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
    private static string AnimationDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/AnimationData.asset";
    private static string CardDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/CardData2.asset";
    private static string CardEffectDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/CardEffectData.asset";
    private static string CardpackDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/CardpackData.asset";
    private static string EnterCardDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/EnterCardData.asset";
    private static string KeywordDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/KeywordData.asset";
    private static string MarketingDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/MarketingData.asset";
    private static string TextDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/TextData.asset";
    private static string TraumaDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/TraumaData.asset";
    private static string VisitorDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/VisitorData.asset";

    [MenuItem("Tools/Generate GenerateAllCardSO from JSON")]
    public static void GenerateAllCardSO()
    {
        Debug.Log("[JsonToSO] 실행 시작!");

        AnimationDataSO animationDataSO = AssetDatabase.LoadAssetAtPath<AnimationDataSO>(AnimationDataSOPath);
        CardData2SO cardData2SO = AssetDatabase.LoadAssetAtPath<CardData2SO>(CardDataSOPath);
        CardEffectDataSO cardEffectDataSO = AssetDatabase.LoadAssetAtPath<CardEffectDataSO>(CardDataSOPath);
        CardpackDataSO cardpackDataSO = AssetDatabase.LoadAssetAtPath<CardpackDataSO>(CardpackDataSOPath);
        EnterCardDataSO enterCardDataSO = AssetDatabase.LoadAssetAtPath<EnterCardDataSO>(EnterCardDataSOPath);
        KeywordDataSO keywordDataSO = AssetDatabase.LoadAssetAtPath<KeywordDataSO>(KeywordDataSOPath);
        MarketingDataSO marketingDataSO = AssetDatabase.LoadAssetAtPath<MarketingDataSO>(MarketingDataSOPath);
        TextDataSO textDataSO = AssetDatabase.LoadAssetAtPath<TextDataSO>(TextDataSOPath);
        TraumaDataSO traumaDataSO = AssetDatabase.LoadAssetAtPath<TraumaDataSO>(TraumaDataSOPath);
        VisitorDataSO visitorDataSO = AssetDatabase.LoadAssetAtPath<VisitorDataSO>(VisitorDataSOPath);

        Dictionary<string, ScriptableObject> soDict = new Dictionary<string, ScriptableObject>
        {
            {AnimationDataSOPath, animationDataSO },
            {CardDataSOPath, cardData2SO },
            {CardEffectDataSOPath, cardEffectDataSO},
            {CardpackDataSOPath, cardpackDataSO },
            {EnterCardDataSOPath, enterCardDataSO },
            {KeywordDataSOPath, keywordDataSO },
            {MarketingDataSOPath, marketingDataSO },
            {TextDataSOPath, textDataSO },
            {TraumaDataSOPath, traumaDataSO },
            {VisitorDataSOPath, visitorDataSO }
        };

        foreach(var kvp in soDict)
        {
            Debug.LogWarning($"[JsonToSo] {kvp.Key}가 존재하지 않음. 새로 생성합니다.");

            ScriptableObject newSO = null;
            if(kvp.Key == AnimationDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<AnimationDataSO>();
                animationDataSO = (AnimationDataSO)newSO;
            }
            else if(kvp.Key == CardDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<CardData2SO>();
                cardData2SO = (CardData2SO)newSO;
            }
            else if(kvp.Key == CardEffectDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<CardEffectDataSO>();
                cardEffectDataSO = (CardEffectDataSO)newSO;
            }
            else if (kvp.Key == CardpackDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<CardpackDataSO>();
                cardpackDataSO = (CardpackDataSO)newSO;
            }
            else if (kvp.Key == EnterCardDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<EnterCardDataSO>();
                enterCardDataSO = (EnterCardDataSO)newSO;
            }
            else if (kvp.Key == KeywordDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<KeywordDataSO>();
                keywordDataSO = (KeywordDataSO)newSO;
            }
            else if (kvp.Key == MarketingDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<MarketingDataSO>();
                marketingDataSO = (MarketingDataSO)newSO;
            }
            else if (kvp.Key == TextDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<TextDataSO>();
                textDataSO = (TextDataSO)newSO;
            }
            else if (kvp.Key == TraumaDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<TraumaDataSO>();
                traumaDataSO = (TraumaDataSO)newSO;
            }
            else if (kvp.Key == VisitorDataSOPath)
            {
                newSO = ScriptableObject.CreateInstance<VisitorDataSO>();
                visitorDataSO = (VisitorDataSO)newSO;
            }


            if (newSO !=null)
            {
                AssetDatabase.CreateAsset(newSO,kvp.Key);
                Debug.Log($"[JsonToSO] {kvp.Key} ScriptableObject가 생성되었습니다.");
            }
        }


        Debug.Log("모든 LoadFromJSON 실행...");
        animationDataSO.LoadFromJSON(jsonFilenameAnimationData);
        cardData2SO.LoadFromJSON(jsonFileNameCardData);
        cardEffectDataSO.LoadFromJSON(jsonFileNameCardEffectData);
        cardpackDataSO.LoadFromJSON(jsonFileNameCardpackData);
        enterCardDataSO.LoadFromJSON(jsonFileNameEnterCardData);
        keywordDataSO.LoadFromJSON(jsonFileNameKeywordData);
        marketingDataSO.LoadFromJSON (jsonFileNameMarketingData);
        textDataSO.LoadFromJSON(jsonFileNameTextData);
        traumaDataSO.LoadFromJSON(jsonFileNameTraumaData);
        visitorDataSO.LoadFromJSON(jsonFileNameVisitorData);

        Debug.Log($"JSON 데이터를 SO로 변환 완료! 저장 위치: {CardDataSOPath}");
    }

    public static void Initailze()
    {
        GenerateAllCardSO();
    }
}
