using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class CsfFile : StringTable
    {
        private static readonly char[] FSC  = {' ', 'F', 'S', 'C' }; // с этих символов начинается любой CSF файл, иначе его игра не прилинкует файл
        private static readonly char[] LBL  = {' ', 'L', 'B', 'L' }; // с этих символов начинается название лейбла
        private static readonly char[] RTS  = {' ', 'R', 'T', 'S' }; // с этих символов начинается значение лейбла
        private static readonly char[] WRTS = {'W', 'R', 'T', 'S' }; // с этих символов начинается значение лейбла, и у него имеется доп. значение
        private static UInt32 CNC_CSF_VERSION = 3;                   // стандартная версия формата CSF во всех играх C&C
        public enum CSFLanguageCodes : UInt32
        {
            US           = 0,
            UK           = 1,
            German       = 2,
            French       = 3,
            Spanish      = 4,
            Italian      = 5,
            Japanese     = 6,
            Jabberwockie = 7,
            Korean       = 8,
            Chinese      = 9,
            Unknown      = 10
        }

        #region Конструкторы
        /// <summary>
        ///     Класс для парсинга <u>.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(string fileName) : base(fileName)
        {
            Parse();
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(string fileName, Encoding encoding) : base(fileName, encoding)
        {
            Parse();
        }

        /// <summary>
        ///     Класс для парсинга <u>.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(CsfFile csfFile) : base(csfFile)
        {
        }

        /// <summary>
        ///     Класс для парсинга <u>.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(string fileName, List<StringTableCategory> stCategories) : base(fileName, stCategories)
        {
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(string fileName, Encoding encoding, List<StringTableCategory> stCategoties) : base(fileName, encoding, stCategoties)
        {
        }
        #endregion

        #region Парсинг
        public override void Parse()
        {
            using (BinaryReader br = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                List<StringTableCategory> bufferList = new List<StringTableCategory>();

                // читаем заголовок файла, который нам укажет количество считываний далее
                char[] csf = br.ReadChars(4); // по факту эта строка обязана всегда быть " FSC"

                if (new string(csf) != new string(FSC))
                    ParsingErrorsAndWarnings.AddMessage("Ошибка чтения .csf заголовка файла: заголовок не содержит строку ' FSC'. "
                                                      + "Игра не прилинкует указанный .csf файл.", StringTableParseException.MessageType.Error);

                UInt32 csfFormatVersion = br.ReadUInt32(); // у игр серии ЦНЦ это число всегда равно 3. по факту ни на что не влияет

                if(csfFormatVersion != 3)
                {
                    ParsingErrorsAndWarnings.AddMessage("Версия формата, указанная в заголовке, не соответствует версии, "
                                                      + "используемой в играх серии C&C. При следующем сохранении изменений "
                                                      + "версия формата будет выставлена в соответствии с версией формата в "
                                                      + "играх серии C&C.", StringTableParseException.MessageType.Warning);
                }

                UInt32 numberOfLabels   = br.ReadUInt32(); // количество лейблов
                UInt32 numberOfStrings  = br.ReadUInt32(); // количество строк
                if(numberOfLabels > numberOfStrings)
                {
                    ParsingErrorsAndWarnings.AddMessage("В файле используются строки с 0 значений. При следующем сохранении "
                                                      + "эти строки будут изменены так, чтобы они содержали значение."
                                                      , StringTableParseException.MessageType.Warning);
                }
                else if(numberOfLabels < numberOfStrings)
                {
                    ParsingErrorsAndWarnings.AddMessage("В файле используются строки с дополнительным значением. "
                                                      + "Конвертирование данного .csf файла в .str файл невозможно! "
                                                      + "Советуем удалить все дополнительные значения."
                                                      , StringTableParseException.MessageType.Warning);
                }


                br.ReadUInt32(); // никто не знает, что это за байты, и никто их не использует (и этот класс пока что тоже не использует)
                br.ReadUInt32(); // код языка (подробнее в LanguagesCodes)

                for (UInt32 i = 0; i < numberOfLabels || br.PeekChar() > -1; i++)
                {
                    // чтение лейбла
                    char[] lbl = br.ReadChars(4); // записывает строку ' LBL'

                    // если у нас строка не является ' LBL', то движок игры считает сл-щие 4 бита для поиска слова ' LBL'
                    if (new string(lbl) != new string(LBL))
                    {
                        if (i > 0) i--;
                        continue;
                    }

                    UInt32 countOfStrings = br.ReadUInt32();                     // количество строк в значении. почти всегда равно 1
                                                                                 // а если больше 1, то имеется дополнительные значения у строки,
                                                                                 // которые на данный момент класс не умеет обрабатывать.
                                                                                 // если 0, то значения нет

                    UInt32 labelNameLength = br.ReadUInt32();                    // длина названия лейбла
                    char[] labelName       = br.ReadChars((int)labelNameLength); // само название лейбла

                    byte[] stringValue = FileEncoding.GetBytes(string.Empty);
                    char[] extraStringValue = new string("").ToCharArray();

                    if(countOfStrings != 0)
                    {
                        // чтение значения лейбла
                        char[] rtsOrWrts = br.ReadChars(4);                 // ' RTS' - доп. значения нет. 'WRTS' - доп. значение есть.
                        UInt32 valueLength = br.ReadUInt32();               // длина строки юникода, укороченная вдвое
                        stringValue = br.ReadBytes(Convert.ToInt32(valueLength * 2)); // строка, конвертированная в интертированные байты

                        InvertAllBytesInArray(stringValue);

                        // чтение дополнительного значения лейбла
                        if (new string(rtsOrWrts) == new string(WRTS))
                        {
                            UInt32 extraValueLength = br.ReadUInt32();              // длина доп. значения
                            extraStringValue = br.ReadChars(Convert.ToInt32(extraValueLength)); // само доп значение (проблема поддержки кодировки отличной от Unicode)
                        }
                    }

                    // преобразование считанных данных во внутренний формат представления стоковой таблицы
                    string categoryName = string.Empty;
                    string stringName = string.Empty;

                    StringBuilder sb = new StringBuilder();
                    int j = 0;
                    foreach (var str in new string(labelName).Split(':'))
                    {
                        j++;
                        switch (j)
                        {
                            case 1: // если двоеточие было одно, и текст перед двоеточием, то это текст является названием категории
                                categoryName = str;
                                break;
                            case 2: // если текст после двоеточия - название строки
                                sb.Append(str);
                                break;
                            default: // если больше 1 двоеточия - прибавляем текст к названию строки
                                sb.Append(":" + str);
                                break;
                        }
                    }

                    stringName = sb.ToString();

                    if (j == 1) // если двоеточий не было вообще
                    {
                        stringName = categoryName;
                        categoryName = NO_CATEGORY_STRINGS;
                    }

                    StringTableCategory category = new StringTableCategory(categoryName);

                    if(new string(extraStringValue) != string.Empty)
                        category.AddString(stringName, new string(FileEncoding.GetChars(stringValue)), new string(extraStringValue));
                    else
                        category.AddString(stringName, new string(FileEncoding.GetChars(stringValue)));

                    bufferList.Add(category);
                }

                CombineStringsIntoCategories(bufferList);
            }
        }

        public override void Save()
        {
            // если файл существует, то стираем его
            if (File.Exists(FileName)) File.Delete(FileName);

            using (BinaryWriter bw = new BinaryWriter(File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                UInt32 countOfLables = Convert.ToUInt32(Count());

                // записываем хедер файла
                bw.Write(FSC);
                bw.Write(CNC_CSF_VERSION);
                bw.Write(countOfLables);
                bw.Write(countOfLables);
                bw.Write((UInt32)0);
                bw.Write((UInt32)0);

                // записываем построчно значения из строковой таблицы в файл
                UInt32 labelCounter = 0;
                do
                {
                    foreach (var category in categoriesOfTable)
                    {
                        foreach (var str in category.stringsOfCategory)
                        {
                            string labelName = category.CategoryName + ":" + str.StringName;

                            bw.Write(LBL);                     // строка со значением ' LBL'
                            bw.Write((uint)1);                 // количество строк для дополнительного значения
                            bw.Write(labelName.Length);        // длина названия
                            bw.Write(labelName.ToCharArray()); // само название

                            if (str.ExtraStringValue == string.Empty)
                                bw.Write(RTS); // строка со значением ' RTS'
                            else
                                bw.Write(WRTS); // строка со значением 'WRTS'

                            bw.Write(Convert.ToUInt32(str.StringValue.Length)); // запись длины значения

                            byte[] byteValue = FileEncoding.GetBytes(str.StringValue); // получения байтового массива на основе строки
                            InvertAllBytesInArray(byteValue); // инвертирование полученного массива

                            bw.Write(byteValue); // запись в файл инвертированных байтов значения строки

                            if (str.ExtraStringValue != string.Empty)
                            {
                                bw.Write(Convert.ToUInt32(str.ExtraStringValue.Length));
                                bw.Write(str.ExtraStringValue.ToCharArray());
                            }

                            labelCounter++; // т.к. мы прошли лейбл, то мы обязаны увеличить счётчик на 1
                        }
                    }
                } while (labelCounter < countOfLables);
            }
        }

        public override void Save(string fileName)
        {
            string tmp = FileName;
            FileName = fileName;
            Save();
            FileName = tmp;
        }

        /// <summary>
        ///     Метод формирует строку, равносильную .str/.csf файлу.
        /// </summary>
        public override string ToString()
            =>
                base.ToString();
        #endregion

        #region Конверторы
        /// <summary>
        ///     Конвертор из <u>.csf</u> в <u>.str</u> из текущего отпарсенного файла.
        /// </summary>
        public StrFile ToStr()
        {
            if (!StringTable.IsConvertableTo((Object)this, StringTableFormats.str))
                throw new StringTableParseException("Указанный экземпляр .csf файла не конвертируем в формат .str");

            // в str нет символов переводы на новую строку заменяются на \n
            List<StringTableCategory> tmp = categoriesOfTable;
            foreach (var category in tmp)
                foreach (var str in category.stringsOfCategory)
                    if (str.StringValue.IndexOf("\n") > -1)
                        str.StringValue.Replace("\n", "\\n");

            return new StrFile(FileName, tmp);
        }

        /// <summary>
        ///     Конвертор из <u>.csf</u> в <u>.str</u> на основе указанного отпарсенного файла fileSample.
        /// </summary>
        public static StrFile ToStr(CsfFile fileSample)
        {
            if (!fileSample.IsConvertable())
                throw new StringTableParseException("Указанный экземпляр .csf файла не конвертируем в формат.str");

            // в str нет символов переводы на новую строку заменяются на \n
            List<StringTableCategory> tmp = fileSample.categoriesOfTable;
            foreach (var category in tmp)
                foreach (var str in category.stringsOfCategory)
                    if (str.StringValue.IndexOf("\n") > -1)
                        str.StringValue.Replace("\n", "\\n");

            return new StrFile(fileSample.FileName, tmp);
        }
        #endregion

        #region Вспомогательные методы
        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой в <u>.str</u>.
        /// </summary>
        public override bool IsConvertable()
            =>
                StringTable.IsConvertableTo((Object)this, StringTableFormats.str);

        public override bool IsConvertable(List<StringTableCategory> stCategories)
            =>
                StringTable.IsConvertableTo((Object)(new StrFile(string.Empty, stCategories)), StringTableFormats.csf);

        /// <summary>
        ///     Согласно описанию формата <u>.csf</u> на <a href="https://modenc.renegadeprojects.com/CSF_File_Format">modenc</a>, 
        ///     все байты строки-значения необходимо<br/>
        ///     инвертировать перед тем, как записывать данные в переменные.<br/>
        ///     Отдельное спасибо человеку с никнеймом <b>pd</b> за подсказку.
        /// </summary>
        private void InvertAllBytesInArray(byte[] array)
        {
            if (array == null) return;

            for (int i = 0; i < array.Length; i++)
                array[i] = (byte)~(array[i]);
        }

        public static bool operator ==(CsfFile firstFile, CsfFile secondFile)
            =>
                (StringTable)firstFile == (StringTable)secondFile;

        public static bool operator !=(CsfFile firstFile, CsfFile secondFile)
            =>
                !(firstFile == secondFile);

        public override bool Equals(object obj)
            =>
                (CsfFile)obj == this;

        public override int GetHashCode()
            =>
                base.GetHashCode();
        #endregion
    }
}
