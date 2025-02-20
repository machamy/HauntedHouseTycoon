using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using Excel = Microsoft.Office.Interop.Excel;

class ExcleToJSON
{
    static void main(string[] args)
    {
        string excelPath = @"C:\Unity 2022.3.10f1\HauntedHouseTycoon\Assets\Scripts\DataBase\CardDataBase.xlsx";
        string jsonFolderPath = @"C:\Unity 2022.3.10f1\HauntedHouseTycoon\Assets\Scripts\DataBase\CardDataBase.JSONs";

        try
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(excelPath);

            for(int i = 0; i <= workbook.Sheets.Count; i++)
            {
                Excel._Worksheet worksheet = workbook.Sheets[i];
                Excel.Range range = worksheet.UsedRange;

                List<Dictionary<string, string>> sheetData = ReadSheet(range);
                string jsonString = JsonSerializer.Serialize(sheetData, new JsonSerializerOptions { WriteIndented = true });

                string sheetName = worksheet.Name;
                string jsonPath = Path.Combine(jsonFolderPath, $"{sheetName}.json");
                File.WriteAllText(jsonPath, jsonString);

                Console.WriteLine($"시트 '{sheetName}' 데이터를 JSON으로 저장 완료.");

                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(worksheet);
            }

            workbook.Close(false);
            excelApp.Quit();
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
        }

        catch(Exception ex)
        {
            Console.WriteLine("오류 발생" + ex.Message);
        }
    }

    static List<Dictionary<string, string>> ReadSheet(Excel.Range range)
    {
        var dataList = new List<Dictionary<string, string>>();

        int rowCount = range.Rows.Count;
        int colnCount = range.Columns.Count;

        int headerRow = 2;
        int startCol = 2;
        int dataStartRow = headerRow + 2;

        List<string> headers = new List<string>();
        for (int c = startCol; c <= colnCount; c++)
        {
            var headerCell = (range.Cells[headerRow, c] as Excel.Range).Value2;
            headers.Add(headerCell != null ? headerCell.ToString() : $"Column{c}");
        }

        for(int r =startCol; r <= colnCount; r++)
        {
            var rowDict = new Dictionary<string, string>();
            for (int c = startCol; c <= colnCount; c++)
            {
                string header = headers[c - startCol];
                var cell = (range.Cells[r, c] as Excel.Range).Value2;
                string cellValue = cell != null ? cell.ToString() : "";
                rowDict[header] = cellValue;
            }
            dataList.Add(rowDict);
        }
        return dataList;
    }
}