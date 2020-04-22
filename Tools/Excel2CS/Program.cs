
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ETTools
{
    public struct CellInfo
    {
        public string Type;
        public string Name;
        public string Desc;
    }
    public class ExcelMD5Info
    {
        public Dictionary<string, string> fileMD5 = new Dictionary<string, string>();

        public string Get(string fileName)
        {
            string md5 = "";
            this.fileMD5.TryGetValue(fileName, out md5);
            return md5;
        }

        public void Add(string fileName, string md5)
        {
            this.fileMD5[fileName] = md5;
        }
    }
    public static class Program
    {
        private const string ExcelPath = "../";
        private const string ServerConfigPath = "../../Config/";
        private const string ServerCSPath = @"../../Server/Model/Games/Common/Config/";
        private static bool isClient = false;
        private static ExcelMD5Info md5Info;
        public static void Main()
        {
            ExportAll(ServerConfigPath);

            ExportAllClass(ServerCSPath, "namespace ETModel\n{\n");
            Console.WriteLine($"导出服务端配置完成!");
            Console.ReadKey();
        }

        private static void ExportAllClass(string exportDir, string csHead)
        {
            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx")
                {
                    continue;
                }
                if (Path.GetFileName(filePath).StartsWith("~"))
                {
                    continue;
                }

                ExportClass(filePath, exportDir, csHead);
                Console.WriteLine($"生成{Path.GetFileName(filePath)}类完成");
            }
        }

        private static void ExportClass(string fileName, string exportDir, string csHead)
        {
            XSSFWorkbook xssfWorkbook = null;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }

            string protoName = Path.GetFileNameWithoutExtension(fileName);

            string exportPath = Path.Combine(exportDir, $"{protoName}.cs");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                StringBuilder sb = new StringBuilder();
                ISheet sheet = xssfWorkbook.GetSheetAt(0);
                sb.Append(csHead);

                sb.Append($"\t[Config((int)({GetCellString(sheet, 0, 0)}))]\n");
                sb.Append($"\tpublic partial class {protoName}Category : ACategory<{protoName}>\n");
                sb.Append("\t{\n");
                sb.Append("\t}\n\n");

                sb.Append($"\tpublic class {protoName}: IConfig\n");
                sb.Append("\t{\n");
                sb.Append("\t\tpublic long Id { get; set; }\n");

                int cellCount = sheet.GetRow(3).LastCellNum;

                for (int i = 0; i < cellCount; i++)
                {
                    string fieldDesc = GetCellString(sheet, 2, i);

                    if (fieldDesc.StartsWith("#"))
                    {
                        continue;
                    }

                    // s开头表示这个字段是服务端专用
                    if (fieldDesc.StartsWith("s") && isClient)
                    {
                        continue;
                    }

                    string fieldName = GetCellString(sheet, 3, i);

                    if (fieldName == "Id" || fieldName == "_id")
                    {
                        continue;
                    }

                    string fieldType = GetCellString(sheet, 4, i);
                    if (fieldType == "" || fieldName == "")
                    {
                        continue;
                    }

                    sb.Append($"\t\tpublic {fieldType} {fieldName};\n");
                }

                sb.Append("\t}\n");
                sb.Append("}\n");

                sw.Write(sb.ToString());
            }
        }

        private static void ExportAll(string exportDir)
        {
            //string md5File = Path.Combine(ExcelPath, "md5.txt");
            //  if (!File.Exists(md5File))
            //{
            //    md5Info = new ExcelMD5Info();
            //}
            //else
            //{
            //    md5Info = MongoHelper.FromJson<ExcelMD5Info>(File.ReadAllText(md5File));
            //}

            foreach (string filePath in Directory.GetFiles(ExcelPath))
            {
                if (Path.GetExtension(filePath) != ".xlsx")
                {
                    continue;
                }
                if (Path.GetFileName(filePath).StartsWith("~"))
                {
                    continue;
                }
                //string fileName = Path.GetFileName(filePath);
                //string oldMD5 = md5Info.Get(fileName);
                //string md5 = MD5Helper.FileMD5(filePath);
                //md5Info.Add(fileName, md5);
                //if (md5 == oldMD5)
                //{
                //    continue;
                //}

                Export(filePath, exportDir);
            }

            //File.WriteAllText(md5File, md5Info.ToJson());

            Console.WriteLine("所有表导出配置文件完成");
        }

        private static void Export(string fileName, string exportDir)
        {
            XSSFWorkbook xssfWorkbook = null;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }
            string protoName = Path.GetFileNameWithoutExtension(fileName);
            Console.WriteLine($"{protoName}导表开始");
            //导出服务器使用配置格式
            string exportPath = Path.Combine(exportDir, $"{protoName}.txt");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                for (int i = 0; i < xssfWorkbook.NumberOfSheets; ++i)
                {
                    ISheet sheet = xssfWorkbook.GetSheetAt(i);
                    ExportSheet(sheet, sw);
                } 
            }
            //导出客户端使用json格式
            exportPath = Path.Combine(exportDir, $"{protoName}.json");
            using (FileStream txt = new FileStream(exportPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(txt))
            {
                StringBuilder jsonSb = new StringBuilder();
                jsonSb.Append("{\"Arr\":[");
                for (int i = 0; i < xssfWorkbook.NumberOfSheets; ++i)
                {
                    ISheet sheet = xssfWorkbook.GetSheetAt(i);
                    ExportSheetToJson(sheet, sw, jsonSb, i == xssfWorkbook.NumberOfSheets -1 );
                }
                sw.WriteLine(jsonSb);
            }
            Console.WriteLine($"{protoName}导表完成");
        }

        private static void ExportSheet(ISheet sheet, StreamWriter sw)
        {
            int cellCount = sheet.GetRow(3).LastCellNum;

            CellInfo[] cellInfos = new CellInfo[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                string fieldDesc = GetCellString(sheet, 2, i);
                string fieldName = GetCellString(sheet, 3, i);
                string fieldType = GetCellString(sheet, 4, i);
                cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Desc = fieldDesc };
            }
            for (int i = 5; i <= sheet.LastRowNum; ++i)
            {
                if (GetCellString(sheet, i, 0) == "")
                {
                    continue;
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                IRow row = sheet.GetRow(i);
                for (int j = 0; j < cellCount; ++j)
                {
                    string desc = cellInfos[j].Desc.ToLower();
                    if (desc.StartsWith("#"))
                    {
                        continue;
                    }

                    // s开头表示这个字段是服务端专用
                    if (desc.StartsWith("s") && isClient)
                    {
                        continue;
                    }

                    // c开头表示这个字段是客户端专用
                    if (desc.StartsWith("c") && !isClient)
                    {
                        continue;
                    }

                    string fieldValue = GetCellString(row, j);
                    if (fieldValue == "")
                    {
                        throw new Exception($"sheet: {sheet.SheetName} 中有空白字段 {i},{j}");
                    }

                    if (j > 0)
                    {
                        sb.Append(",");
                    }

                    string fieldName = cellInfos[j].Name;

                    if (fieldName == "Id" || fieldName == "_id")
                    {
                        if (isClient)
                        {
                            fieldName = "Id";
                        }
                        else
                        {
                            fieldName = "_id";
                        }
                    }

                    string fieldType = cellInfos[j].Type;
                    sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
                }
                sb.Append("}");
                sw.WriteLine(sb.ToString());
            }
        }

        private static void ExportSheetToJson(ISheet sheet, StreamWriter sw , StringBuilder jsonSB , bool lastSheet)
        {
            int cellCount = sheet.GetRow(3).LastCellNum;

            CellInfo[] cellInfos = new CellInfo[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                string fieldDesc = GetCellString(sheet, 2, i);
                string fieldName = GetCellString(sheet, 3, i);
                string fieldType = GetCellString(sheet, 4, i);
                cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Desc = fieldDesc };
            }
            for (int i = 5; i <= sheet.LastRowNum; ++i)
            {
                if (GetCellString(sheet, i, 0) == "")
                {
                    continue;
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                IRow row = sheet.GetRow(i);
                for (int j = 0; j < cellCount; ++j)
                {
                    string desc = cellInfos[j].Desc.ToLower();
                    if (desc.StartsWith("#"))
                    {
                        continue;
                    }

                    // s开头表示这个字段是服务端专用
                    if (desc.StartsWith("s") && isClient)
                    {
                        continue;
                    }

                    // c开头表示这个字段是客户端专用
                    if (desc.StartsWith("c") && !isClient)
                    {
                        continue;
                    }

                    string fieldValue = GetCellString(row, j);
                    if (fieldValue == "")
                    {
                        throw new Exception($"sheet: {sheet.SheetName} 中有空白字段 {i},{j}");
                    }

                    if (j > 0)
                    {
                        sb.Append(",");
                    }

                    string fieldName = cellInfos[j].Name;

                    if (fieldName == "Id" || fieldName == "_id")
                    {
                        if (isClient)
                        {
                            fieldName = "Id";
                        }
                        else
                        {
                            fieldName = "_id";
                        }
                    }

                    string fieldType = cellInfos[j].Type;
                    sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
                }
                sb.Append("}");
                if (i != sheet.LastRowNum || !lastSheet) jsonSB.Append(sb.ToString() + ",");
                else jsonSB.Append(sb.ToString() + "]}");
            }
        }

        private static string Convert(string type, string value)
        {
            switch (type)
            {
                case "int[]":
                case "int32[]":
                case "long[]":
                    return $"[{value}]";
                case "string[]":
                    return $"[{value}]";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    return value;
                case "string":
                    return $"\"{value}\"";
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }

        private static string GetCellString(ISheet sheet, int i, int j)
        {
            return sheet.GetRow(i)?.GetCell(j)?.ToString() ?? "";
        }

        private static string GetCellString(IRow row, int i)
        {
            return row?.GetCell(i)?.ToString() ?? "";
        }

        private static string GetCellString(ICell cell)
        {
            return cell?.ToString() ?? "";
        }

    }
}
