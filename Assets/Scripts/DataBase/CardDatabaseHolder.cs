using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System;
using ClassBase.Card;
using CardDataManager;
using Newtonsoft.Json;

public class CardDatabaseHolder : MonoBehaviour
{
    public Dictionary<string, IList> soLists = new Dictionary<string, IList>();
    public Dictionary<string, Dictionary<long, ScriptableObject>> allDataDictionaries = new Dictionary<string, Dictionary<long, ScriptableObject>>();


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
        CardDataHelper helper = new CardDataHelper();
        CardDataHelper.Initialize();
        LoadAllScriptableObjects();
        ConvertAllListsToDictionaries();
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
        string listName = char.ToLower(soType.Name[0]) + soType.Name.Substring(1);

        if (listName.EndsWith("SO"))
        {
            listName = listName.Substring(0, listName.Length - 2);
        }

        listName += "List";

        if (!soLists.ContainsKey(listName))
        {
            Type listType = typeof(List<>).MakeGenericType(soType);
            IList newList = (IList)Activator.CreateInstance(listType);
            soLists.Add(listName, newList);
        }

        soLists[listName].Add(so);
    }

    private void ConvertAllListsToDictionaries()
    {
        allDataDictionaries.Clear();

        foreach (var key in soLists.Keys)
        {
            IList soList = soLists[key];

            if (soList == null || soList.Count == 0)
            {
                Debug.LogWarning($"'{key}' ����Ʈ�� ��� �ֽ��ϴ�.");
                continue;
            }

            Debug.Log($"����Ʈ��: {key}, SO ����: {soList.Count}");

            Dictionary<long, ScriptableObject> dataDictionary = new Dictionary<long, ScriptableObject>();

            foreach (ScriptableObject so in soList)
            {
                if (so == null)
                {
                    Debug.LogError($"'{key}' ����Ʈ�� ��� �� null�� �����մϴ�.");
                    continue;
                }

                Debug.Log($"SO �̸�: {so.name}, Ÿ��: {so.GetType()}");

                string listFieldName = key;
                Debug.Log($"'{so.name}'���� ���� ����Ʈ �ʵ��: {listFieldName}");

                var listField = so.GetType().GetField(listFieldName);

                if (listField == null)
                {
                    Debug.LogWarning($"{so.name}�� '{listFieldName}' �ʵ尡 �������� �ʽ��ϴ�.");
                    continue;
                }

                var listValue = listField.GetValue(so) as IList;

                if (listValue == null || listValue.Count == 0)
                {
                    Debug.LogWarning($"{so.name} ���� ����Ʈ�� ��� �ֽ��ϴ�.");
                    continue;
                }

                Debug.Log($"{so.name} ���� ����Ʈ ����: {listValue.Count}");

                foreach (var item in listValue)
                {
                    if (item == null)
                    {
                        Debug.LogError($"'{so.name}' ���� ����Ʈ�� null ��Ұ� �ֽ��ϴ�.");
                        continue;
                    }

                    var indexField = item.GetType().GetField("Index");

                    if (indexField == null)
                    {
                        Debug.LogError($"'{so.name}' ���� ����Ʈ ��ҿ��� 'Index' �ʵ带 ã�� �� �����ϴ�.");
                        continue;
                    }

                    object indexValue = indexField.GetValue(item);

                    if (indexValue is long index)
                    {
                        if (dataDictionary.ContainsKey(index))
                        {
                            Debug.LogWarning($"'{so.name}'���� �ߺ��� Index �߰�: {index}, ���� ������ ����.");
                        }
                        else
                        {
                            dataDictionary.Add(index, so);
                        }
                    }
                    else
                    {
                        Debug.LogError($"'{so.name}'�� Index ���� long Ÿ���� �ƴմϴ�. ���� ��: {indexValue}, Ÿ��: {indexValue?.GetType()}");
                    }
                }
            }

            allDataDictionaries[key] = dataDictionary;
            Debug.Log($"Dictionary ��ȯ �Ϸ�: {key}, ����� ������ ����: {dataDictionary.Count}");
        }
    }

}
