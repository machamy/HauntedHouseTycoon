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

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RuntimeInitialize()
    {
        Initialize();
    }*/

    private static void Initialize()
    {
        ExcelToJSON.Initailze();
#if UNITY_EDITOR
        JsonToSo.GenerateAllCardSO();
#else
        //JsonToSo.GenerateAllCardSOForAPI();
#endif

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
