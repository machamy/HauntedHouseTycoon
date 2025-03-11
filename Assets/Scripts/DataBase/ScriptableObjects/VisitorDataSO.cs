using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(menuName = "VisitorDataSO")]
public class VisitorDataSO : ScriptableObject
{
    [SerializeField] public List<ClassManager.Card.Visitor> animtionDataList = new List<ClassManager.Card.Visitor>();

    public void LoadFromJSON(string jsonFilePath)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", jsonFilePath);
        string json = File.ReadAllText(fullPath);
    }
}