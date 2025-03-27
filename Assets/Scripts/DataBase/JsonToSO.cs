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
}
