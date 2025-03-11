using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(menuName = "MarketingDataSO")]
public class MarketingDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.MarketingData> animtionDataList = new List<ClassManager.Card.MarketingData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);
    }
}