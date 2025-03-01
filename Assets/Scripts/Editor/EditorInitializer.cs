using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class EditorInitializer
{
    static EditorInitializer()
    {
        Debug.Log("[EditorInitializer] Unity Editor �����! ExcelToJSON ���� ����...");

        ExcelToJSON.ConvertAllExcelsInFolder("Assets/Scripts/DataBase");

        EditorApplication.delayCall += () =>
        {
            Debug.Log("[EditorInitializer] ExcelToJSON �Ϸ� �� JsonToSO ����...");
            JsonToSo.GenerateCardData2SO();
        };
    }
}
