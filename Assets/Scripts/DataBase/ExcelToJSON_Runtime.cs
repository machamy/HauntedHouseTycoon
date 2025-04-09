using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using ExcelDataReader;
using Newtonsoft.Json;

public class ExcelToJSON_Runtime
{
    public static void ConvertExcelAtRuntime(string fileName)
    {
        string inputPath = Path.Combine(Application.streamingAssetsPath, fileName);
        string outputFolder = Path.Combine(Application.streamingAssetsPath,"AssetBundles","CardDataBase_JSON");

        if (!File.Exists(inputPath))
        {
            Debug.LogError("Excel 파일을 찾을 수 없습니다: " + inputPath);
            return;
        }

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        string markerPath = Path.Combine(outputFolder, "__ExcelLastModified.txt");
        DateTime excelLastModified = File.GetLastWriteTimeUtc(inputPath);
        bool isModified = true;

        if (File.Exists(markerPath))
        {
            string timestamp = File.ReadAllText(markerPath);
            if (DateTime.TryParse(timestamp, out var prevModified))
            {
                if (prevModified >= excelLastModified)
                {
                    Debug.Log("Excel 파일 변경 없음. JSON 생성 생략.");
                    return;
                }
            }
        }

        if (Directory.Exists(outputFolder))
        {
            Directory.Delete(outputFolder, true);
            Debug.Log("기존 JSON 폴더 삭제 완료");
        }

        Directory.CreateDirectory(outputFolder);

        using (var stream = File.Open(inputPath, FileMode.Open, FileAccess.Read))
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
            var result = reader.AsDataSet();

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            foreach (DataTable table in result.Tables)
            {
                ProcessSheet(table, outputFolder);
            }

            Debug.Log($"런타임 엑셀 → JSON 변환 완료: {inputPath}");
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

        for (int r = 3; r < table.Rows.Count; r++)
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
                        if (Enum.TryParse(typeof(ClassBase.Card.CardDatabase.Type), cellValue, out var typeEnum))
                        {
                            rowDict[header] = ((int)(ClassBase.Card.CardDatabase.Type)typeEnum).ToString();
                        }
                        else
                        {
                            rowDict[header] = "-1";
                        }
                    }

                    else if (header.Equals("Rank", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Enum.TryParse(typeof(ClassBase.Card.CardDatabase.Rank), cellValue, out var rankEnum))
                        {
                            rowDict[header] = ((int)(ClassBase.Card.CardDatabase.Rank)rankEnum).ToString();
                        }
                        else
                        {
                            rowDict[header] = "-1";
                        }
                    }

                    else if (header.Equals("ConditionType", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Enum.TryParse(typeof(ClassBase.Card.Effect.ConditonType), cellValue, out var conditonTypeEnum))
                        {
                            rowDict[header] = ((int)(ClassBase.Card.Effect.ConditonType)conditonTypeEnum).ToString();
                        }
                        else
                        {
                            rowDict[header] = "-1";
                        }
                    }

                    else if (header.Equals("TargetType", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Enum.TryParse(typeof(ClassBase.Card.Effect.TargetType), cellValue, out var targetTypeEnum))
                        {
                            rowDict[header] = ((int)(ClassBase.Card.Effect.TargetType)targetTypeEnum).ToString();
                        }
                        else
                        {
                            rowDict[header] = "-1";
                        }
                    }

                    else if (header.Equals("Sex", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Enum.TryParse(typeof(ClassBase.GameObject.Visitor.Sex), cellValue, out var sexEnum))
                        {
                            rowDict[header] = ((int)(ClassBase.GameObject.Visitor.Sex)sexEnum).ToString();
                        }
                        else
                        {
                            rowDict[header] = "-1";
                        }
                    }

                    else if (header.Equals("PanicResponse", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Enum.TryParse(typeof(ClassBase.GameObject.Visitor.PanicResponse), cellValue, out var panicResponseEnum))
                        {
                            rowDict[header] = ((int)(ClassBase.GameObject.Visitor.PanicResponse)panicResponseEnum).ToString();
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
        string jsonString = JsonConvert.SerializeObject(excelData, Formatting.Indented);
        File.WriteAllText(jsonFilePath, jsonString, System.Text.Encoding.UTF8);

        Debug.Log($"JSON 저장 완료: {jsonFilePath}");
    }
}