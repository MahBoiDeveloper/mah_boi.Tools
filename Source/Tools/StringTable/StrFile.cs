﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using mah_boi.Tools.StringTable.Exceptions;

namespace mah_boi.Tools.StringTable
{
    /// <summary>
    ///     Класс для парсинга <u>.str</u> файлов<br/>
    ///     Поддерживаются форматы игр: GZH, TWKW, RA3.<br/><br/>
    ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
    ///     Подробнее про особенности парсинга 
    ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L16">здесь</see>
    /// </summary>
    #region Нюансы парсинга STR
    //     Это не войдёт в описание класса, но это необходимо упомянуть хотя бы в виде комментариев.
    //     
    //     Используемый формат .str файлов основывается в основном на формате mod.str
    //     модов на игру Red Alert 3. Тем не менее, он поддерживает и не только формат .str Red Alert 3, 
    //     но форматы и TW, и KW, и GZH. Применяемая при написании кода терминалогия немного отличается от 
    //     общепринятой с сайта-вики modenc.renegadeprojects.com, т.к. она более структурированная и расширена.
    //     
    //     .str файл состоит из лейблов aka строк, которые имеют значение.
    //     Лейблы регистронезависимы, т.к. писать можно их в любом регистре. Лейблы могут повторяться.
    //     Названия лейблов состоят исключительно из символов ASCII и могут содержать пробелы.
    //     
    //     Пример лейбла
    //     
    //                          Лейбл
    //                            ^
    //            *---------------^----------------*
    //            |HOTKEYNAME_SIDEBARWATERCRAFTPAGE|
    //            *----^-----^-----------v---------*
    //                 ^     ^           v
    //             Категория ^           v
    //                       ^         Строка
    //                  Разделитель (условный; по факту разделителем может быть что угодно, но WW и EA использвали в основном двоеточие)
    //     
    //     У лейблов ВСЕГДА имеется значение, которое указывается между ковычками "".
    //     Пустое значение обозначается в друг за другом идущих ковычках.
    //     
    //     Значение может быть многострочным, т.е. его можно расписать на несколько строк.
    //     каждая строка после первой обязана начинаться \n, а последняя строка
    //     должна начинаться с \n и заканчиваться ".
    //     
    //     Заканчиваются лейблы всегда словом "end". Регистр слова не важен.
    //     
    //     Примеры правильных лейблов:
    //     SCRIPT:SCRIPT_EXAMPLE
    //     	"Значение строки, котороые будет выведено"
    //     END
    //     
    //     SCRIPT:SCRIPT_EXAMPLE
    //     "Значение строки, котороые будет выведено.
    //     \n Однако разделено на несколько строк.
    //     \n И в этой строке их 3"
    //     End
    //     
    //     Some text for string
    //     ""
    //     end
    #endregion
    
    public class StrFile : StringTable
    {
        private enum LineType
        {
            Label = 0,
            Value = 1,
            End = 2
        }

        #region Конструкторы
        /// <summary>
        ///     Класс для парсинга <u>.str</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StrFile() : base()
        {
        }

        /// <summary>
        ///     Класс для парсинга <u>.str</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StrFile(string fileName) : base(fileName)
        {
            Parse();
        }

        /// <summary>
        ///     Класс для парсинга <u>.str</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StrFile(string fileName, Encoding encoding) : base(fileName, encoding)
        {
            Parse();
        }

        /// <summary>
        ///     Класс для парсинга <u>.str</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StrFile(StrFile strFile) : base(strFile)
        {
        }

        /// <summary>
        ///     Класс для парсинга <u>.str</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StrFile(string fileName, List<StringTableString> stStringsList) : base(fileName, stStringsList)
        {
        }

        /// <summary>
        ///     Класс для парсинга <u>.str</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StrFile(string fileName, Encoding encoding, List<StringTableString> stStringsList) : base(fileName, encoding, stStringsList)
        {
        }

        /// <summary>
        ///     Класс для парсинга <u>.str</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StrFile(string fileName, List<StringTableString> stStringsList, List<StringTableExtraString> stExtraStringList) : base(fileName, stStringsList, stExtraStringList)
        {
        }
        #endregion

        #region Парсинг
        /// <summary>
        ///     Парсинг .str файла.
        /// </summary>
        public override void Parse()
        {
            string stringName   = string.Empty;
            string stringValue  = string.Empty;

            // Согласно https://modenc.renegadeprojects.com/CSF_File_Format
            // название строки состоит исключительно из ASCII символов
            // дополнительно к этому стоит отметить, что формат поддерживает
            // комментарии, которые подобны комментариям из FORTRAN 66
            // т.е. комментрии должны начинаться с 1-ой буквы строки.
            // Комметрий создаётся за счёт символов //, как в C-подобных языках
            LineType searchStatus = LineType.Label;

            // красиво-ленивый способ пробежаться по всем строкам файла.
            uint currentLineNumber = 0;
            foreach (var currentLine in new StreamReader(FileName, FileEncoding).ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                currentLineNumber++;
                // считанная строка - комментарий или пустая строка
                if (currentLine.StartsWith("//") || currentLine.Trim() == string.Empty)
                    continue;

                // считанная строка содержит ошибку, т.к. несколько значений
                else if
                (
                    searchStatus == LineType.End && currentLine.Trim().ToLower() != "end"
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
                    searchStatus == (int)LineType.Label              // анализируемая строка является лейблом
                    && !currentLine.Trim().StartsWith("\"")          // строка не является значением
                )
                {
                    if (StringTableString.IsACIIString(currentLine)) // символы в значении исключительно в кодировке ASCII
                    {
                        stringName   = currentLine.Trim();
                        searchStatus = LineType.Value;
                    }
                    else                                             // символы в значении не в кодировке ASCII
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
                    Table.Add(new StringTableString(stringName, stringValue));
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
        ///     Сохранение данных класса в .str файл.
        /// </summary>
        public override void Save()
        {
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(FileName), FileEncoding))
                sw.WriteLine(ToString());
        }

        /// <summary>
        ///     Сохранение данных класса в .str файл с указанным именем.
        /// </summary>
        public override void SaveAs(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(fileName), FileEncoding))
                sw.WriteLine(ToString());
        }
        #endregion

        #region Конверторы
        /// <summary>
        ///     Конвертор из <u>.str</u> в <u>.csf</u> из текущего отпарсенного файла.
        /// </summary>
        public CsfFile ToCsf()
        {
            if (!StringTable.IsConvertableTo((StringTable)this, StringTableFormats.csf))
                throw new StringTableParseException("Указанный экземпляр .str файла не конвертируем в формат .csf");

            // в csf нет символов \n, т.к. они заменяются на символ перевода строки и каретки
            List<StringTableString> tmp = Table;
            foreach (var str in tmp)
                str.StringValue = str.StringValue.Replace("\\n", "\n");

            return new CsfFile(FileName, tmp, ExtraTable);
        }

        /// <summary>
        ///     Конвертор, аналогичный ToCsf, только безопасный.
        /// </summary>
        public bool Safe_ToCsf(out CsfFile returnParam)
        {
            try
            {
                returnParam = ToCsf();
            }
            catch(StringTableParseException)
            {
                returnParam = new CsfFile();
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Конвертор из <u>.str</u> в <u>.csf</u> на основе указанного отпарсенного файла fileSample.
        /// </summary>
        public static CsfFile ToCsf(StrFile fileSample)
        {
            if (!fileSample.IsConvertable())
                throw new StringTableParseException("Указанный экземпляр .str файла не конвертируем в формат .csf");

            // в csf нет символов \n, т.к. они заменяются на символ перевода строки и каретки
            List<StringTableString> tmp = fileSample.Table;
            foreach (var str in tmp)
                if (str.StringValue.IndexOf("\\n") > -1)
                    str.StringValue.Replace("\\n", "\n");

            return new CsfFile(fileSample.FileName, tmp, fileSample.ExtraTable);
        }

        /// <summary>
        ///     Безопасный конвертор для конвертации .str файла без исключений.
        /// </summary>
        public static bool Safe_ToCsf(StrFile fileSample, out CsfFile returnParam)
        {
            try
            {
                returnParam = fileSample.ToCsf();
            }
            catch (StringTableParseException)
            {
                returnParam = new CsfFile();
                return false;
            }
            return true;
        }
        #endregion

        #region Вспомогательные методы
        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой в <u>.csf</u>.
        /// </summary>
        public override bool IsConvertable()
            =>
                StringTable.IsConvertableTo((StringTable)this, StringTableFormats.csf);

        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой в <u>.csf</u>.
        /// </summary>
        public override bool IsConvertable(List<StringTableString> strings)
            =>
                StringTable.IsConvertableTo((StringTable)new CsfFile(string.Empty, strings), StringTableFormats.str);

        public static bool operator ==(StrFile firstFile, StrFile secondFile)
            =>
                (StringTable)firstFile == (StringTable)secondFile;

        public static bool operator !=(StrFile firstFile, StrFile secondFile)
            =>
                !(firstFile == secondFile);

        public override bool Equals(object obj)
            =>
                (StrFile)obj == this;

        public override int GetHashCode()
            =>
                base.GetHashCode();

        /// <summary>
        ///     Метод формирует строку, равносильную .str/.csf файлу.
        /// </summary>
        public override string ToString()
            =>
                base.ToString();
        #endregion
    }
}
