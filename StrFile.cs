using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace mah_boi.Tools
{
    /// <summary>
    ///     Класс для парсинга <u>.str</u> файлов<br/>
    ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
    ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
    ///     Подробнее про особенности парсинга 
    ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
    /// </summary>
    /// 
    ///     Это не войдёт в описание класса, но это необходимо упомянуть хотя бы в виде комментариев.
    ///     
    ///     Используемый формат .str файлов основывается в основном на формате mod.str
    ///     модов на игру Red Alert 3. Тем не менее, он поддерживает и не только формат .str Red Alert 3, 
    ///     но и TW, и KW, и GZH. Применяемая при написании кода терминалогия немного отличается от 
    ///     общепринятой с сайта-вики modenc.renegadeprojects.com, т.к. она более структурированная.
    ///     
    ///     .str файл состоит из лейблов, которые имеют значение.
    ///     Лейблы являются сочетанием категорий и строк, разделяемыми через двоеточие.
    ///     Лейблы регистронезависимы, т.к. писать можно их в любом регистре.
    ///     Лейблы могут повторяться.
    ///     Названия лейблов состоят исключительно из символов ASCII
    ///     
    ///     Приведу пример более подробно то, что описал текстом:
    ///     
    ///                          Лейбл
    ///                            ^
    ///            |---------------^----------------|
    ///            |HOTKEYNAME:SIDEBARWATERCRAFTPAGE|
    ///            |----^-----^-----------v---------|
    ///                 ^     ^           v
    ///             Категория ^           v
    ///                       ^         Строка
    ///                  Разделитель
    ///     
    ///     У лейблов ВСЕГДА имеется значение, которое указывается между ковычками "".
    ///     Пустое значение обозначается в друг за другом идущих ковычках.
    ///     
    ///     Значение может быть многострочным, т.е. его можно расписать на несколько строк.
    ///     каждая строка после первой обязана начинаться \n, а последняя строка
    ///     должна начинаться с \n и заканчиваться ".
    ///     
    ///     Заканчиваются лейблы всегда словом "end". Регистр слова не важен.
    ///     
    ///     Примеры правильных лейблов:
    ///     SCRIPT:SCRIPT_EXAMPLE
    ///     	"Значение строки, котороые будет выведено"
    ///     END
    ///     
    ///     SCRIPT:SCRIPT_EXAMPLE
    ///     "Значение строки, котороые будет выведено.
    ///     \n Однако разделено на несколько строк.
    ///     \n И в этой строке их 3"
    ///     End
    ///     
    ///     Some text for string
    ///     ""
    ///     end
    class StrFile : StringTable
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
        public StrFile(string fileName, List<StringTableCategory> stCategories) : base(fileName, stCategories)
        {
        }
        #endregion

        #region Парсинг
        /// <summary>
        ///     Парсинг .str файла.
        /// </summary>
        public override void Parse()
        {
            string categoryName = string.Empty;
            string stringName   = string.Empty;
            string stringValue  = string.Empty;

            var tmpListOfCategory = new List<StringTableCategory>();

            // Согласно https://modenc.renegadeprojects.com/CSF_File_Format
            // название строки состоит исключительно из ASCII символов
            // дополнительно к этому стоит отметить, что формат поддерживает
            // комментарии, которые подобны комментариям из FORTRAN 66
            // т.е. комментрии должны начинаться с 1-ой буквы строки.
            // Комметрий создаётся за счёт символов //, как в C-подобных языках
            int searchStatus = (int)LineType.Label;

            // красиво-ленивый способ пробежаться по всем строкам файла. Имхо, но это лучше, чем просто считывать
            foreach (var currentLine in new StreamReader(FileName).ReadToEnd().Split(Environment.NewLine))
            {
                // считанная строка - комментарий или пустая строка
                if (currentLine.StartsWith("//") || currentLine.Trim() == string.Empty)
                    continue;

                // считанная строка содержит ошибку, т.к. несколько двоеточий
                else if
                (
                    searchStatus == (int)LineType.Label
                    && currentLine.IndexOf(':') > -1
                    && currentLine.IndexOf(':', currentLine.IndexOf(':') + 1) > -1
                )
                    throw new StringTableParseException("Ошибка форматирования: указано несколько двоеточий в названии лейбла"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");

                // считанная строка содержит ошибку, т.к. несколько значений
                else if
                (
                    searchStatus == (int)LineType.End && currentLine.Trim().ToLower() != "end"
                )
                    throw new StringTableParseException("Ошибка форматирования: после значения лейбла идёт другая строка со значением"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");

                // считанная строка содержит ошибку, т.к. нет значения
                else if (currentLine.Trim().ToLower() == "end" && searchStatus == (int)LineType.Value)
                    throw new StringTableParseException("Ошибка форматирования: после названия лейбла идёт закрытие строки, а не значение"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + "Воспользуйтесь ковычками \"\" для обозначения пустой строки"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");

                // считанная строка - лейбл
                else if
                (
                    searchStatus == (int)LineType.Label            // анализируемая строка является лейблом
                    && !currentLine.Trim().StartsWith("\"")        // строка не является значением
                    && StringTableString.IsACIIString(currentLine) // символы исключительно в кодировке ASCII
                )
                {
                    // если у нас не закрытая строка, то мы очищаем все заполненные поля
                    categoryName = string.Empty;
                    stringName   = string.Empty;
                    stringValue  = string.Empty;

                    int i = 0;
                    foreach (var str in currentLine.Split(':'))
                    {
                        i++;
                        switch (i)
                        {
                            case 1: // первая часть строки до ":" - [SOMETHING:]
                                categoryName = str.ToUpper();
                                break;
                            case 2: // вторая часть строки после ":" - [:SOMETHING]
                                stringName = str.ToUpper();
                                break;
                        }
                    }

                    // если не было двоеточия, то всё название лейбла - это название строки
                    if (stringName == string.Empty)
                    {
                        stringName = categoryName;
                        categoryName = NO_CATEGORY_STRINGS;
                    }

                    searchStatus = (int)LineType.Value;
                }

                // считанная строка - полное значение
                else if
                (
                    searchStatus == (int)LineType.Value    // анализируемая строка - значение
                    && currentLine.Trim().StartsWith("\"") // значения всегда начинаются с таба+ковычка
                    && currentLine.Trim().EndsWith("\"")   // и оканчиваются ковычкой
                )
                {
                    stringValue = currentLine.Trim().Replace("\"", string.Empty);
                    searchStatus = (int)LineType.End;
                }

                // считанная строка - это частичное значение
                else if
                (
                    searchStatus == (int)LineType.Value     // анализируемая строка - значение
                    && currentLine.Trim().StartsWith("\"")  // значение частичное, т.к. в строке только 1 ковычка, и стоит она в начале
                    && !currentLine.Trim().EndsWith("\"")   // и нет ковычки в конце
                )
                {
                    stringValue += currentLine.Trim().Replace("\"", string.Empty);
                }

                else if
                (
                    searchStatus == (int)LineType.Value     // анализируемая строка - значение
                    && currentLine.Trim().StartsWith("\\n") // значение частичное, т.к. в начале строки \n
                    && !currentLine.Trim().EndsWith("\"")   // и нет ковычки в конце
                )
                {
                    stringValue += currentLine.Trim();
                }

                else if
                (
                    searchStatus == (int)LineType.Value     // анализируемая строка - значение
                    && currentLine.Trim().StartsWith("\\n") // значение частичное, т.к. в начале строки \n
                    && currentLine.Trim().EndsWith("\"")    // и в строке только 1 ковычка, и стоит она в конце
                )
                {
                    stringValue += currentLine.Trim().Replace("\"", string.Empty);
                    searchStatus = (int)LineType.End;
                }

                else if
                (
                    searchStatus == (int)LineType.Value
                    && !currentLine.Trim().StartsWith("\\n")
                )
                {
                    throw new StringTableParseException("Ошибка форматирования: отсутствие символов \"\\n\" в начале составной строки."
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");
                }

                // считанная строка - окончание строки
                else if (searchStatus == (int)LineType.End && currentLine.Trim().ToLower() == "end")
                {
                    var tmpCategory = new StringTableCategory(categoryName);
                    tmpCategory.AddString(stringName, stringValue);
                    tmpListOfCategory.Add(tmpCategory);

                    searchStatus = (int)LineType.Label;
                }

                // на случай непредвиденных проблем
                else
                {
                    throw new StringTableParseException("Неизвестная ошибка форматирования."
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");
                }
            }
            CombineStringsIntoCategories(tmpListOfCategory);
        }

        /// <summary>
        ///     Сохранение данных класса в .str файл.
        /// </summary>
        public override void Save()
        {
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(FileName)))
                sw.WriteLine(ToString());
        }

        /// <summary>
        ///     Сохранение данных класса в .str файл с указанным именем.
        /// </summary>
        public override void Save(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(fileName)))
                sw.WriteLine(ToString());
        }

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        #region Конверторы
        /// <summary>
        ///     Конвертор из <u>.str</u> в <u>.csf</u> из текущего отпарсенного файла.
        /// </summary>
        public CsfFile ToCsf()
        {
            if (!StringTable.IsConvertableTo((Object)this, StringTableFormats.str))
                throw new StringTableParseException("Указанный экземпляр .str файла не конвертируем в формат .csf");

            // в csf нет символов \n, т.к. они заменяются на символ перевода строки и каретки
            List<StringTableCategory> tmp = categoriesOfTable;
            foreach(var category in tmp)
                foreach(var str in category.stringsOfCategory)
                    if (str.StringValue.IndexOf("\\n") > -1)
                        str.StringValue.Replace("\\n", "\n");

            return new CsfFile(FileName, tmp);
        }

        /// <summary>
        ///     Конвертор из <u>.str</u> в <u>.csf</u> на основе указанного отпарсенного файла fileSample.
        /// </summary>
        public static CsfFile ToCsf(StrFile fileSample)
        {
            if (!fileSample.IsConvertable())
                throw new StringTableParseException("Указанный экземпляр .str файла не конвертируем в формат .csf");

            // в csf нет символов \n, т.к. они заменяются на символ перевода строки и каретки
            List<StringTableCategory> tmp = fileSample.categoriesOfTable;
            foreach (var category in tmp)
                foreach (var str in category.stringsOfCategory)
                    if (str.StringValue.IndexOf("\\n") > -1)
                        str.StringValue.Replace("\\n", "\n");

            return new CsfFile(fileSample.FileName, tmp);
        }
        #endregion

        #region Вспомогательные методы
        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой в <u>.csf</u>.
        /// </summary>
        public override bool IsConvertable()
            =>
                StringTable.IsConvertableTo((Object)this, StringTableFormats.csf);

        public override bool IsConvertable(List<StringTableCategory> stCategories)
            =>
                StringTable.IsConvertableTo((Object)(new CsfFile(string.Empty, stCategories)), StringTableFormats.str);

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
        #endregion
    }
}
