using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class EditorInitializer
{
    static EditorInitializer()
    {
        Debug.Log("[EditorInitializer] Unity Editor 실행됨! ExcelToJSON 실행 시작...");

        ExcelToJSON.ConvertAllExcelsInFolder("Assets/Scripts/DataBase");

        EditorApplication.delayCall += () =>
        {
            Debug.Log("[EditorInitializer] ExcelToJSON 완료 후 JsonToSO 실행...");
            JsonToSo.GenerateCardData2SO();
        };
    }
}
