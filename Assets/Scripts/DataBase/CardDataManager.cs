using UnityEngine;
using UnityEditor;

[InitializeOnLoad]

public class CardDataManager
{
    static CardDataManager()
    {
        InitializeAllSystems();
    }

    private static void InitializeAllSystems()
    {
        ExcelToJSON.Initailze();
        JsonToSo.Initailze();
        AssetDatabase.Refresh();
    }
}
