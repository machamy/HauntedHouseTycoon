using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(menuName = "KeywordDataSO")]
public class KeywordDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.KeywordData> animtionDataList = new List<ClassManager.Card.KeywordData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);
    }
}