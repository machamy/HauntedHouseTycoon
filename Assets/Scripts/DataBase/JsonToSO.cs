using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;
using System.Reflection;
using Common.Interfaces;
using System.Collections;


public class JsonToSO
{
    public interface ILoadFromJson
    {
        void LoadFromJson(string jsonPath);
    }

    public interface IIndexedData<T> where T : IBaseData
    {
        T FindByIndex(long index);
    }


    public ScriptableObject CreateSO(string className, string jsonPath)
    {
        Type soType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => typeof(ScriptableObject).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract && t.Name == className);

        if (soType == null)
            throw new InvalidOperationException($"ScriptableObject 클래스 '{className}'을 찾을 수 없습니다.");

        ScriptableObject soInstance = ScriptableObject.CreateInstance(soType);

        if (soInstance is ILoadFromJson loader)
        {
            loader.LoadFromJson(jsonPath);
        }

        Type dataType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => typeof(Common.Interfaces.IBaseData).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract && t.Name == className);

        if (dataType == null)
            throw new InvalidOperationException($"IBaseData를 상속한 '{className}' 이름의 클래스를 찾을 수 없습니다.");

        FieldInfo listField = soType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .FirstOrDefault(f => f.FieldType.IsGenericType
                                 && f.FieldType.GetGenericTypeDefinition() == typeof(List<>)
                                 && f.FieldType.GetGenericArguments()[0] == dataType);

        if (listField == null)
            throw new InvalidOperationException($"'{className}'에 List<{dataType.Name}> 필드가 없습니다.");

        var originalList = listField.GetValue(soInstance) as IEnumerable;
        var tempList = new List<Common.Interfaces.IBaseData>();

        if (originalList != null)
        {
            foreach (var item in originalList)
            {
                if (item is Common.Interfaces.IBaseData baseData)
                    tempList.Add(baseData);
            }
        }

        var createdList = Activator.CreateInstance(listField.FieldType);
        var addMethod = listField.FieldType.GetMethod("Add");

        foreach (var item in tempList)
        {
            addMethod.Invoke(createdList, new object[] { item });
        }

        listField.SetValue(soInstance, createdList);

        return soInstance;
    }
}
