using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Text;
using System.Xml.Linq;

namespace ModelTrainer
{
    public static class ExcelHelper
    {
        public static void SaveToExcel(string filePath, List<JJMInfo> records)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
            {
                // 1. [Content_Types].xml
                WriteZipEntry(archive, "[Content_Types].xml",
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Types xmlns=""http://schemas.openxmlformats.org/package/2006/content-types"">
  <Default Extension=""rels"" ContentType=""application/vnd.openxmlformats-package.relationships+xml""/>
  <Default Extension=""xml"" ContentType=""application/xml""/>
  <Override PartName=""/xl/workbook.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml""/>
  <Override PartName=""/xl/worksheets/sheet1.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml""/>
</Types>");

                // 2. _rels/.rels
                WriteZipEntry(archive, "_rels/.rels",
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""xl/workbook.xml""/>
</Relationships>");

                // 3. xl/workbook.xml
                WriteZipEntry(archive, "xl/workbook.xml",
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"">
  <sheets>
    <sheet name=""TrainingData"" sheetId=""1"" r:id=""rId1""/>
  </sheets>
</workbook>");

                // 4. xl/_rels/workbook.xml.rels
                WriteZipEntry(archive, "xl/_rels/workbook.xml.rels",
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"" Target=""worksheets/sheet1.xml""/>
</Relationships>");

                // 5. xl/worksheets/sheet1.xml
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>");
                sb.AppendLine(@"<worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">");
                sb.AppendLine(@"  <sheetData>");

                // Header Row
                sb.AppendLine(@"    <row r=""1"">");
                sb.AppendLine(@"      <c r=""A1"" t=""inlineStr""><is><t>User</t></is></c>");
                sb.AppendLine(@"      <c r=""B1"" t=""inlineStr""><is><t>Assistant</t></is></c>");
                sb.AppendLine(@"    </row>");

                // Data Rows
                for (int i = 0; i < records.Count; i++)
                {
                    int rowNum = i + 2;
                    string userEscaped = SecurityElement.Escape(records[i].User ?? string.Empty) ?? string.Empty;
                    string assistantEscaped = SecurityElement.Escape(records[i].Assistant ?? string.Empty) ?? string.Empty;

                    sb.AppendLine($@"    <row r=""{rowNum}"">");
                    sb.AppendLine($@"      <c r=""A{rowNum}"" t=""inlineStr""><is><t>{userEscaped}</t></is></c>");
                    sb.AppendLine($@"      <c r=""B{rowNum}"" t=""inlineStr""><is><t>{assistantEscaped}</t></is></c>");
                    sb.AppendLine(@"    </row>");
                }

                sb.AppendLine(@"  </sheetData>");
                sb.AppendLine(@"</worksheet>");

                WriteZipEntry(archive, "xl/worksheets/sheet1.xml", sb.ToString());
            }
        }

        public static void SaveToCsv(string filePath, List<JJMInfo> records)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            writer.WriteLine("\"User\",\"Assistant\"");

            foreach (var rec in records)
            {
                string u = (rec.User ?? string.Empty).Replace("\"", "\"\"");
                string a = (rec.Assistant ?? string.Empty).Replace("\"", "\"\"");
                writer.WriteLine($"\"{u}\",\"{a}\"");
            }
        }

        public static List<JJMInfo> LoadFromExcel(string filePath)
        {
            var result = new List<JJMInfo>();
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Excel file not found at: {filePath}");
            }

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read);

            // Optional shared strings (used if opened/saved by Microsoft Excel)
            var sharedStrings = new List<string>();
            var sharedEntry = archive.GetEntry("xl/sharedStrings.xml");
            if (sharedEntry != null)
            {
                using var stream = sharedEntry.Open();
                var doc = XDocument.Load(stream);
                XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
                foreach (var si in doc.Descendants(ns + "si"))
                {
                    var textNodes = si.Descendants(ns + "t");
                    var fullText = string.Concat(textNodes.Select(t => t.Value));
                    sharedStrings.Add(fullText);
                }
            }

            var sheetEntry = archive.GetEntry("xl/worksheets/sheet1.xml");
            if (sheetEntry == null)
            {
                throw new InvalidOperationException("Could not find xl/worksheets/sheet1.xml in Excel archive.");
            }

            using (var stream = sheetEntry.Open())
            {
                var doc = XDocument.Load(stream);
                XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
                var rows = doc.Descendants(ns + "row");

                bool isFirstRow = true;
                foreach (var row in rows)
                {
                    var cells = row.Elements(ns + "c").ToList();
                    string userVal = string.Empty;
                    string assistantVal = string.Empty;

                    foreach (var c in cells)
                    {
                        string r = c.Attribute("r")?.Value ?? string.Empty;
                        string t = c.Attribute("t")?.Value ?? string.Empty;
                        string cellText = ExtractCellValue(c, t, ns, sharedStrings);

                        if (r.StartsWith("A"))
                        {
                            userVal = cellText;
                        }
                        else if (r.StartsWith("B"))
                        {
                            assistantVal = cellText;
                        }
                    }

                    if (isFirstRow)
                    {
                        // Check if header row
                        isFirstRow = false;
                        if (userVal.Equals("User", StringComparison.OrdinalIgnoreCase) &&
                            assistantVal.Equals("Assistant", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(userVal) && !string.IsNullOrWhiteSpace(assistantVal))
                    {
                        result.Add(new JJMInfo
                        {
                            User = userVal.Trim(),
                            Assistant = assistantVal.Trim()
                        });
                    }
                }
            }

            return result;
        }

        public static List<JJMInfo> LoadFromCsv(string filePath)
        {
            var result = new List<JJMInfo>();
            if (!File.Exists(filePath)) return result;

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            string? line = reader.ReadLine(); // header
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                // Parse CSV with quotes
                var tokens = ParseCsvLine(line);
                if (tokens.Count >= 2)
                {
                    result.Add(new JJMInfo
                    {
                        User = tokens[0].Trim(),
                        Assistant = tokens[1].Trim()
                    });
                }
            }
            return result;
        }

        private static string ExtractCellValue(XElement cell, string cellType, XNamespace ns, List<string> sharedStrings)
        {
            if (cellType == "inlineStr")
            {
                var isEl = cell.Element(ns + "is");
                if (isEl != null)
                {
                    return string.Concat(isEl.Descendants(ns + "t").Select(t => t.Value));
                }
            }
            else if (cellType == "s")
            {
                var vEl = cell.Element(ns + "v");
                if (vEl != null && int.TryParse(vEl.Value, out int idx) && idx >= 0 && idx < sharedStrings.Count)
                {
                    return sharedStrings[idx];
                }
            }
            else
            {
                var vEl = cell.Element(ns + "v");
                if (vEl != null) return vEl.Value;
            }
            return string.Empty;
        }

        private static void WriteZipEntry(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
            using var entryStream = entry.Open();
            using var streamWriter = new StreamWriter(entryStream, new UTF8Encoding(false));
            streamWriter.Write(content);
        }

        private static List<string> ParseCsvLine(string line)
        {
            var list = new List<string>();
            bool inQuotes = false;
            var sb = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '\"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '\"')
                    {
                        sb.Append('\"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            list.Add(sb.ToString());
            return list;
        }
    }
}
