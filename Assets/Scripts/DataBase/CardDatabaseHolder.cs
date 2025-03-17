using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System;

public class CardDatabaseHolder : MonoBehaviour
{
    public Dictionary<string, IList> soLists = new Dictionary<string, IList>();

    private static CardDatabaseHolder instance;
    public static CardDatabaseHolder Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<CardDatabaseHolder>();
            }
            return instance;
        }
    }

    private void Start()
    {
        LoadAllScriptableObjects();
    }

    private void LoadAllScriptableObjects()
    {
        soLists.Clear();

        string path = Path.Combine(Application.dataPath, "Resources/AllDataBase");

        Debug.Log($"SO 파일을 검색할 경로: {path}");

        if (!Directory.Exists(path))
        {
            Debug.LogError($"경로를 찾을 수 없습니다: {path}");
            return;
        }

        string[] files = Directory.GetFiles(path, "*.asset", SearchOption.AllDirectories);
        Debug.Log($"발견된 ScriptableObject 파일 개수: {files.Length}");

        foreach (string file in files)
        {
            ScriptableObject so = LoadScriptableObject(file);
            if (so != null)
            {
                AddToSpecificList(so);
            }
        }

        Debug.Log($"총 {soLists.Count}개의 ScriptableObject 리스트가 생성되었습니다.");
    }

    private ScriptableObject LoadScriptableObject(string filePath)
    {
        try
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            ScriptableObject so = Resources.Load<ScriptableObject>($"AllDataBase/{fileName}");

            if (so == null)
            {
                Debug.LogError($"'{fileName}'에 해당하는 ScriptableObject를 Resources/AllDataBase에서 찾을 수 없습니다.");
                return null;
            }

            Debug.Log($"ScriptableObject 로드 성공: {so.name}");
            return so;
        }
        catch (Exception e)
        {
            Debug.LogError($"ScriptableObject 로드 중 예외 발생: {e.Message}");
            return null;
        }
    }

    private void AddToSpecificList(ScriptableObject so)
    {
        Type soType = so.GetType();
        string listName = char.ToLower(soType.Name[0]) + soType.Name.Substring(1) + "List";

        if (!soLists.ContainsKey(listName))
        {
            Type listType = typeof(List<>).MakeGenericType(soType);
            IList newList = (IList)Activator.CreateInstance(listType);
            soLists.Add(listName, newList);
        }

        soLists[listName].Add(so);
    }
}
