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

        Debug.Log($"SO ������ �˻��� ���: {path}");

        if (!Directory.Exists(path))
        {
            Debug.LogError($"��θ� ã�� �� �����ϴ�: {path}");
            return;
        }

        string[] files = Directory.GetFiles(path, "*.asset", SearchOption.AllDirectories);
        Debug.Log($"�߰ߵ� ScriptableObject ���� ����: {files.Length}");

        foreach (string file in files)
        {
            ScriptableObject so = LoadScriptableObject(file);
            if (so != null)
            {
                AddToSpecificList(so);
            }
        }

        Debug.Log($"�� {soLists.Count}���� ScriptableObject ����Ʈ�� �����Ǿ����ϴ�.");
    }

    private ScriptableObject LoadScriptableObject(string filePath)
    {
        try
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            ScriptableObject so = Resources.Load<ScriptableObject>($"AllDataBase/{fileName}");

            if (so == null)
            {
                Debug.LogError($"'{fileName}'�� �ش��ϴ� ScriptableObject�� Resources/AllDataBase���� ã�� �� �����ϴ�.");
                return null;
            }

            Debug.Log($"ScriptableObject �ε� ����: {so.name}");
            return so;
        }
        catch (Exception e)
        {
            Debug.LogError($"ScriptableObject �ε� �� ���� �߻�: {e.Message}");
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
