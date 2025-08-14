using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using mah_boi.Tools.Extensions;
using mah_boi.Tools.StringTableFormats.Exceptions;

namespace mah_boi.Tools.StringTableFormats;

/// <summary>
/// Class for parsing <u>.str</u> file format.<br/>
/// Supported games: GZH, TW, KW, RA3.<br/><br/>
/// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
/// </summary>
public class StrFile : StringTable
{
    private enum LineType
    {
        Label = 0,
        Value = 1,
        End = 2
    }

    #region Contsructors
    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile() : base()
    {
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName) : base(fileName)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName, Encoding encoding) : base(fileName, encoding)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(StrFile strFile) : base(strFile)
    {
        this.Format = strFile.Format;
        this.CategorySeparator = strFile.CategorySeparator;
        this.FileEncoding = strFile.FileEncoding;
        this.FileName = strFile.FileName;
        this.Table = strFile.Table;
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName, List<StringTableEntry> stStringsList) : base(fileName, stStringsList)
    {
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName, Encoding encoding, List<StringTableEntry> stStringsList) : base(fileName, encoding, stStringsList)
    {
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(StringTable stSample) : base(stSample)
    {
    }
    #endregion

    #region Parsing
    public override void Parse()
    {
        string stringName   = string.Empty;
        string stringValue  = string.Empty;

        LineType searchStatus = LineType.Label;

        // For each cycle through all lines in text file.
        uint currentLineNumber = 0;
        foreach (var currentLine in new StreamReader(FileName, FileEncoding).ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
        {
            currentLineNumber++;

            // Skip line if it is a commentary or empty line
            if 
            (
                currentLine.StartsWith("//") 
                || currentLine.StartsWith(";") 
                || currentLine.Trim() == string.Empty
            )
                continue;

            // If read string has an error (multiple value)
            if
            (
                searchStatus == LineType.End 
                && !currentLine.Trim().Equals("end", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                parsingErrorsAndWarnings.AddMessage($"Ошибка форматирования в строке ({currentLineNumber}): \"{currentLine}\" | "
                                                   + "После значения лейбла идёт другая строка со значением", StringTableParseException.MessageType.Error);
                searchStatus = LineType.Label;
            }

            // считанная строка содержит ошибку, т.к. нет значения
            else if (currentLine.Trim().ToLower() == "end" && searchStatus == LineType.Value)
            {
                parsingErrorsAndWarnings.AddMessage($"Ошибка форматирования в строке ({currentLineNumber}): \"{currentLine}\" | "
                                                   + "После названия лейбла идёт закрытие строки, а не значение. "
                                                   + "Воспользуйтесь ковычками \"\" для обозначения пустой строк", StringTableParseException.MessageType.Error);

                searchStatus = LineType.Label;
            }

            // считанная строка - лейбл а-ля название строки
            else if
            (
                searchStatus == (int)LineType.Label     // анализируемая строка является лейблом
                && !currentLine.Trim().StartsWith("\"") // строка не является значением
            )
            {
                if (currentLine.IsACII()) // символы в значении исключительно в кодировке ASCII
                {
                    stringName   = currentLine.Trim();
                    searchStatus = LineType.Value;
                }
                else                                   // символы в значении не в кодировке ASCII
                {
                    parsingErrorsAndWarnings.AddMessage($"Ошибка форматирования в строке ({currentLineNumber}): \"{currentLine}\" | "
                                                       + "В названии строки содержатся не ASCII символы, что не является "
                                                       + "допустимым. Замените их, чтобы строку можно было считать.",
                                                        StringTableParseException.MessageType.Error);
                }
            }

            // считанная строка - полное значение
            else if
            (
                searchStatus == LineType.Value         // анализируемая строка - значение
                && currentLine.Trim().StartsWith("\"") // значения всегда начинаются с таба+ковычка
                && currentLine.Trim().EndsWith("\"")   // и оканчиваются ковычкой
            )
            {
                stringValue  = currentLine.Trim().Replace("\"", string.Empty);
                searchStatus = LineType.End;
            }

            // считанная строка - это частичное значение
            else if
            (
                searchStatus == LineType.Value         // анализируемая строка - значение
                && currentLine.Trim().StartsWith("\"") // значение частичное, т.к. в строке только 1 ковычка, и стоит она в начале
                && !currentLine.Trim().EndsWith("\"")  // и нет ковычки в конце
            )
            {
                stringValue += currentLine.Trim().Replace("\"", string.Empty);
            }

            else if
            (
                searchStatus == LineType.Value          // анализируемая строка - значение
                && currentLine.Trim().StartsWith("\\n") // значение частичное, т.к. в начале строки \n
                && !currentLine.Trim().EndsWith("\"")   // и нет ковычки в конце
            )
            {
                stringValue += currentLine.Trim();
            }

            else if
            (
                searchStatus == LineType.Value          // анализируемая строка - значение
                && currentLine.Trim().StartsWith("\\n") // значение частичное, т.к. в начале строки \n
                && currentLine.Trim().EndsWith("\"")    // и в строке только 1 ковычка, и стоит она в конце
            )
            {
                stringValue += currentLine.Trim().Replace("\"", string.Empty);
                searchStatus = LineType.End;
            }

            else if
            (
                searchStatus == LineType.Value           // если мы ищем значение
                && !currentLine.Trim().StartsWith("\\n") // а текущая строка не начинается с \n, когда мы ищем значение
            )
            {
                parsingErrorsAndWarnings.AddMessage($"Ошибка форматирования в строке ({currentLineNumber}): \"{currentLine}\" | "
                                                   + "Отсутствие символов \"\\n\" в начале составной строки.", StringTableParseException.MessageType.Error);
            }

            // считанная строка - окончание строки
            else if (searchStatus == LineType.End && currentLine.Trim().ToLower() == "end")
            {
                Table.Add(new StringTableEntry(stringName, stringValue));
                searchStatus = (int)LineType.Label;
            }

            // на случай не предвиденных проблем
            else
            {
                parsingErrorsAndWarnings.AddMessage($"Ошибка форматирования в строке ({currentLineNumber}): \"{currentLine}\" | "
                                                   + "Неизвестная ошибка", StringTableParseException.MessageType.Error);
            }
        }
    }

    /// <summary>
    /// Save .str file format data to the source file.
    /// </summary>
    public override void Save()
    {
        using (StreamWriter sw = new(File.OpenWrite(FileName), FileEncoding))
            sw.WriteLine(ToString());
    }

    /// <summary>
    /// Save .str file format data to the destination file.
    /// </summary>
    public override void SaveAs(string fileName)
    {
        using (StreamWriter sw = new(File.OpenWrite(fileName), FileEncoding))
            sw.WriteLine(ToString());
    }
    #endregion

    #region Other methods

    public static bool operator == (StrFile a, StrFile b) => (StringTable)a == (StringTable)b;

    public static bool operator != (StrFile a, StrFile b) => !(a == b);

    public override bool Equals(object obj) => (StrFile)obj == this;

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => base.ToString();
    #endregion
}
