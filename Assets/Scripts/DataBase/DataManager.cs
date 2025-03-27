using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using ExcelDataReader.Log;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    private readonly Dictionary<Type, ScriptableObject> databaseMap = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ExcelToJSON_Runtime.ConvertExcelAtRuntime("ExcelFiles/CardDataBase.xlsx");
        LoadAllDatabases();
    }

    private void Register<T>(string jsonPath)
        where T : ScriptableObject, JsonToSO.ILoadFromJson
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, jsonPath);
        T so = ScriptableObject.CreateInstance<T>();
        so.LoadFromJson(fullPath);
        databaseMap[typeof(T)] = so;
    }

    private void LoadAllDatabases()
    {
        Register<AnimationData>("AssetBundles/CardDataBase_JSON/AnimationData.json");
        Register<CardDatabase>("AssetBundles/CardDataBase_JSON/CardData.json");
        Register<CardEffectData>("AssetBundles/CardDataBase_JSON/CardEffectData.json");
        Register<CardpackData>("AssetBundles/CardDataBase_JSON/CardpackData.json");
        Register<EnterCardData>("AssetBundles/CardDataBase_JSON/EnterCardData.json");
        Register<KeywordData>("AssetBundles/CardDataBase_JSON/KeywordData.json");
        Register<MarketingData>("AssetBundles/CardDataBase_JSON/MarketingData.json");
        Register<TextData>("AssetBundles/CardDataBase_JSON/TextData.json");
        Register<TraumaData>("AssetBundles/CardDataBase_JSON/TraumaData.json");
        Register<VisitorData>("AssetBundles/CardDataBase_JSON/VisitorData.json");
    }

    public T Get<T>() where T : ScriptableObject
    {
        if (databaseMap.TryGetValue(typeof(T), out var so))
            return so as T;

        Debug.LogError($"등록되지 않은 SO 요청: {typeof(T).Name}");
        return null;
    }
}