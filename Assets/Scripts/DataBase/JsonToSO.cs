using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class JsonToSo
{
    private static string jsonFileName = "Assets/Scripts/DataBase/CardDataBase_JSON/CardData.json";
    private static string scriptableObjectPath = "Assets/Scripts/DataBase/ScriptableObjects/CardData2.asset";

    [MenuItem("Tools/Generate CardData2SO from JSON")]
    public static void GenerateCardData2SO()
    {
        Debug.Log("[JsonToSO] 실행 시작!");

        // ScriptableObject 불러오기
        CardData2SO cardData2SO = AssetDatabase.LoadAssetAtPath<CardData2SO>(scriptableObjectPath);

        if (cardData2SO == null)
        {
            Debug.LogWarning("[JsonToSO] CardData2SO가 존재하지 않음. 새로 생성합니다.");
            cardData2SO = ScriptableObject.CreateInstance<CardData2SO>();
            AssetDatabase.CreateAsset(cardData2SO, scriptableObjectPath);
        }
        Debug.Log("[JsonToSO] CardData2SO의 LoadFromJSON 실행...");
        cardData2SO.LoadFromJSON(jsonFileName);  // JSON 데이터 로드

        // ScriptableObject 데이터 변경사항 저장
        EditorUtility.SetDirty(cardData2SO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"JSON 데이터를 CardData2SO로 변환 완료! 저장 위치: {scriptableObjectPath}");
    }
}
