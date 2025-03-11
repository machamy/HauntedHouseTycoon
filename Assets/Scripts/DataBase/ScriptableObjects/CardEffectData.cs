using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(menuName = "CardEffect DataSO")]
public class CardEffectDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.Effect> animtionDataList = new List<ClassManager.Card.Effect>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);
    }
}