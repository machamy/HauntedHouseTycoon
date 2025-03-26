using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[InitializeOnLoad]
public class CardDataInitialize
{
    static CardDataInitialize()
    {
        Initialize();
    }

    private static void Initialize()
    {
        ExcelToJSON.Initailze();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
