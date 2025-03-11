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
        Debug.Log("[JsonToSO] ���� ����!");

        AnimationDataSO animationDataSO = AssetDatabase.LoadAssetAtPath<AnimationDataSO>(AnimationDataSOPath);
        CardData2SO cardData2SO = AssetDatabase.LoadAssetAtPath<CardData2SO>(CardDataSOPath);

        if (cardData2SO == null)
        {
            Debug.LogWarning("[JsonToSO] CardData2SO�� �������� ����. ���� �����մϴ�.");
            cardData2SO = ScriptableObject.CreateInstance<CardData2SO>();
            AssetDatabase.CreateAsset(cardData2SO, CardDataSOPath);
        }

        else if(animationDataSO == null)
        {
            Debug.LogWarning("[JsonToSO] AnimationDataSO�� �������� ����. ���� �����մϴ�.");
            animationDataSO = ScriptableObject.CreateInstance<AnimationDataSO>();
            AssetDatabase.CreateAsset(animationDataSO, AnimationDataSOPath);
        }

        Debug.Log("��� LoadFromJSON ����...");
        cardData2SO.LoadFromJSON(jsonFileNameCardData);
        animationDataSO.LoadFromJSON(jsonFilenameAnimationData);


        Debug.Log($"JSON �����͸� SO�� ��ȯ �Ϸ�! ���� ��ġ: {CardDataSOPath}");
    }

    public static void Initailze()
    {
        GenerateAllCardSO();
    }
}
