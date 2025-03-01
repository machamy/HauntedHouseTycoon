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
        Debug.Log("[JsonToSO] ���� ����!");

        // ScriptableObject �ҷ�����
        CardData2SO cardData2SO = AssetDatabase.LoadAssetAtPath<CardData2SO>(scriptableObjectPath);

        if (cardData2SO == null)
        {
            Debug.LogWarning("[JsonToSO] CardData2SO�� �������� ����. ���� �����մϴ�.");
            cardData2SO = ScriptableObject.CreateInstance<CardData2SO>();
            AssetDatabase.CreateAsset(cardData2SO, scriptableObjectPath);
        }
        Debug.Log("[JsonToSO] CardData2SO�� LoadFromJSON ����...");
        cardData2SO.LoadFromJSON(jsonFileName);  // JSON ������ �ε�

        // ScriptableObject ������ ������� ����
        EditorUtility.SetDirty(cardData2SO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"JSON �����͸� CardData2SO�� ��ȯ �Ϸ�! ���� ��ġ: {scriptableObjectPath}");
    }
}
