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
                Debug.LogWarning($"'{key}' 리스트가 비어 있습니다.");
                continue;
            }

            Debug.Log($"리스트명: {key}, SO 개수: {soList.Count}");

            Dictionary<long, ScriptableObject> dataDictionary = new Dictionary<long, ScriptableObject>();

            foreach (ScriptableObject so in soList)
            {
                if (so == null)
                {
                    Debug.LogError($"'{key}' 리스트의 요소 중 null이 존재합니다.");
                    continue;
                }

                Debug.Log($"SO 이름: {so.name}, 타입: {so.GetType()}");

                string listFieldName = key;
                Debug.Log($"'{so.name}'에서 내부 리스트 필드명: {listFieldName}");

                var listField = so.GetType().GetField(listFieldName);

                if (listField == null)
                {
                    Debug.LogWarning($"{so.name}에 '{listFieldName}' 필드가 존재하지 않습니다.");
                    continue;
                }

                var listValue = listField.GetValue(so) as IList;

                if (listValue == null || listValue.Count == 0)
                {
                    Debug.LogWarning($"{so.name} 내부 리스트가 비어 있습니다.");
                    continue;
                }

                Debug.Log($"{so.name} 내부 리스트 개수: {listValue.Count}");

                foreach (var item in listValue)
                {
                    if (item == null)
                    {
                        Debug.LogError($"'{so.name}' 내부 리스트에 null 요소가 있습니다.");
                        continue;
                    }

                    var indexField = item.GetType().GetField("Index");

                    if (indexField == null)
                    {
                        Debug.LogError($"'{so.name}' 내부 리스트 요소에서 'Index' 필드를 찾을 수 없습니다.");
                        continue;
                    }

                    object indexValue = indexField.GetValue(item);

                    if (indexValue is long index)
                    {
                        if (dataDictionary.ContainsKey(index))
                        {
                            Debug.LogWarning($"'{so.name}'에서 중복된 Index 발견: {index}, 기존 데이터 유지.");
                        }
                        else
                        {
                            dataDictionary.Add(index, so);
                        }
                    }
                    else
                    {
                        Debug.LogError($"'{so.name}'의 Index 값이 long 타입이 아닙니다. 실제 값: {indexValue}, 타입: {indexValue?.GetType()}");
                    }
                }
            }

            allDataDictionaries[key] = dataDictionary;
            Debug.Log($"Dictionary 변환 완료: {key}, 저장된 데이터 개수: {dataDictionary.Count}");
        }
    }

}
