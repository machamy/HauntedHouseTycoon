using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using ExcelDataReader;
using UnityEngine;
using UnityEditor;
using ClassManager;
using static ClassManager.Card.CardClass;

[InitializeOnLoad]
public class ExcelToJSON
{
    static ExcelToJSON()
    {
        string folderPath = Application.dataPath + "/Scripts/DataBase";
        ConvertAllExcelsInFolder(folderPath);
    }

    public static void ConvertAllExcelsInFolder(string folderPath)
    {
        if(!Directory.Exists(folderPath))
        {
            Debug.LogWarning("폴더를 찾을 수 없습니다.");
        }

        string[] excelFiles = Directory.GetFiles(folderPath, "*xlsx",SearchOption.AllDirectories);

        foreach(string excelFilePath in excelFiles)
        {
            ConvertExcelToJson(excelFilePath, "");
        }

        AssetDatabase.Refresh();
    }

    public static void ConvertExcelToJson(string excelFilePath, string sheetName = "")
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                string excelFileName = Path.GetFileNameWithoutExtension(excelFilePath);
                string outputFolder = Path.Combine(Path.GetDirectoryName(excelFilePath), excelFileName + "_JSON");

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                foreach (DataTable table in result.Tables)
                {
                    ProcessSheet(table, outputFolder);
                }
            }
            Debug.Log($"엑셀 → JSON 변환 완료: {excelFilePath}");
        }
    }

    public static void ProcessSheet(DataTable table, string outputFolder)
    {
        Debug.Log($"시트 이름: {table.TableName}");

        var excelData = new List<Dictionary<string, string>>();

        List<string> headers = new List<string>();
        for (int c = 2; c < table.Columns.Count; c++)
        {
            headers.Add(table.Rows[1][c].ToString());
        }

        for (int r = 2; r < table.Rows.Count; r++)
        {
            var rowDict = new Dictionary<string, string>();
            for (int c = 2; c < table.Columns.Count; c++)
            {
                string header = headers[c - 2];
                var cellValue = table.Rows[r][c]?.ToString() ?? "";

                if (cellValue.Contains(","))
                {
                    string[] splitArray = cellValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < splitArray.Length; i++)
                    {
                        rowDict[$"{header}_{i}"] = splitArray[i].Trim();
                    }
                }
                else
                {
                    if (header.Equals("Type", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Enum.TryParse(typeof(ClassManager.Card.CardClass.Type), cellValue, out var typeEnum))
                        {
                            rowDict[header] = ((int)(ClassManager.Card.CardClass.Type)typeEnum).ToString();
                        }
                        else
                        {
                            rowDict[header] = "-1";
                        }
                    }
                    else if (header.Equals("Rank", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Enum.TryParse(typeof(ClassManager.Card.CardClass.Rank), cellValue, out var rankEnum))
                        {
                            rowDict[header] = ((int)(ClassManager.Card.CardClass.Rank)rankEnum).ToString();
                        }
                        else
                        {
                            rowDict[header] = "-1";
                        }
                    }
                    else
                    {
                        rowDict[header] = cellValue;
                    }
                }

            }
            excelData.Add(rowDict);
        }

        string jsonFilePath = Path.Combine(outputFolder, table.TableName + ".json");

        var wrapper = new CardDataWrapper { cardDataList = excelData };
        var jsonString = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

        File.WriteAllText(jsonFilePath, jsonString, System.Text.Encoding.UTF8);

        Debug.Log($"시트 '{table.TableName}' → JSON 변환 완료: {jsonFilePath}");
    }

    [System.Serializable]
    public class CardDataWrapper
    {
        public List<Dictionary<string, string>> cardDataList;
    }

}