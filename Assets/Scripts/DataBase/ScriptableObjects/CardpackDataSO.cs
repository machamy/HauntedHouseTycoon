using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(menuName = "CardpackDataSO")]
public class CardPackDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.CardPackData> animtionDataList = new List<ClassManager.Card.CardPackData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);
    }
}