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
    private static string AnimationDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/AnimaiotnData.asset";
    private static string CardDataSOPath = "Assets/Scripts/DataBase/ScriptableObjects/CardData2.asset";

    [MenuItem("Tools/Generate GenerateAllCardSO from JSON")]
    public static void GenerateAllCardSO()
    {
        Debug.Log("[JsonToSO] 실행 시작!");

        AnimationDataSO animationDataSO = AssetDatabase.LoadAssetAtPath<AnimationDataSO>(AnimationDataSOPath);
        CardData2SO cardData2SO = AssetDatabase.LoadAssetAtPath<CardData2SO>(CardDataSOPath);

        if (cardData2SO == null)
        {
            Debug.LogWarning("[JsonToSO] CardData2SO가 존재하지 않음. 새로 생성합니다.");
            cardData2SO = ScriptableObject.CreateInstance<CardData2SO>();
            AssetDatabase.CreateAsset(cardData2SO, CardDataSOPath);
        }

        else if(animationDataSO == null)
        {
            Debug.LogWarning("[JsonToSO] AnimationDataSO가 존재하지 않음. 새로 생성합니다.");
            animationDataSO = ScriptableObject.CreateInstance<AnimationDataSO>();
            AssetDatabase.CreateAsset(animationDataSO, AnimationDataSOPath);
        }

        Debug.Log("모든 LoadFromJSON 실행...");
        cardData2SO.LoadFromJSON(jsonFileNameCardData);
        animationDataSO.LoadFromJSON(jsonFilenameAnimationData);


        Debug.Log($"JSON 데이터를 SO로 변환 완료! 저장 위치: {CardDataSOPath}");
    }

    public static void Initailze()
    {
        GenerateAllCardSO();
    }
}
