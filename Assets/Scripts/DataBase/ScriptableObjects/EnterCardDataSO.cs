using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(menuName = "EnterCardDataSO")]
public class EnterCardDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.EnterCardData> animtionDataList = new List<ClassManager.Card.EnterCardData>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);
    }
}