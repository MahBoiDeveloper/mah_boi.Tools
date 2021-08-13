using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace mah_boi.Tools
{
    class CsfFile : StringTable
    {
        #region Хедеры, лейблы и строки формата .csf
        public class CsfFileHeader
        {
            public const UInt32 CNC_CSF_VERSION = 3;
            public static readonly string CSF_REVERSED = " FSC";

            public enum LanguagesCodes : UInt32
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
            };

            public char[] Csf { get; set; }
            public UInt32 CsfVersion { get; set; }
            public UInt32 NumberOfLabels { get; set; }
            public UInt32 NumberOfStrings { get; set; }
            public UInt32 UnusedBytes { get; set; }
            public UInt32 LanguageCode { get; set; }

            public CsfFileHeader()
            {
                Csf             = " FSC".ToCharArray();
                CsfVersion      = 3;
                NumberOfLabels  = 0;
                NumberOfStrings = 0;
                UnusedBytes     = 0;
                LanguageCode    = 0;
            }

            public CsfFileHeader
                (
                    char[] csf,
                    UInt32 csfVersion,
                    UInt32 numberOfLabels,
                    UInt32 numberOfStrings,
                    UInt32 unusedBytes,
                    UInt32 languageCode
                )
            {
                Csf             = csf;
                CsfVersion      = csfVersion;
                NumberOfLabels  = numberOfLabels;
                NumberOfStrings = numberOfStrings;
                UnusedBytes     = unusedBytes;
                LanguageCode    = languageCode;
            }

            public CsfFileHeader(CsfFileHeader header)
            {
                Csf             = header.Csf;
                CsfVersion      = header.CsfVersion;
                NumberOfLabels  = header.NumberOfLabels;
                NumberOfStrings = header.NumberOfStrings;
                UnusedBytes     = header.UnusedBytes;
                LanguageCode    = header.LanguageCode;
            }

            public static bool operator ==(CsfFileHeader firstHeader, CsfFileHeader secondHeader)
            {
                if
                (
                       firstHeader.Csf             == secondHeader.Csf
                    && firstHeader.CsfVersion      == secondHeader.CsfVersion
                    && firstHeader.NumberOfLabels  == secondHeader.NumberOfLabels
                    && firstHeader.NumberOfStrings == secondHeader.NumberOfStrings
                    && firstHeader.UnusedBytes     == secondHeader.UnusedBytes
                    && firstHeader.LanguageCode    == secondHeader.LanguageCode
                )
                    return true;

                return false;
            }

            public static bool operator !=(CsfFileHeader firstHeader, CsfFileHeader secondHeader)
                =>
                    !(firstHeader == secondHeader);

            public override bool Equals(object header)
                =>
                    (CsfFileHeader)header == this;

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public class CsfLabelHeader
        {
            public static readonly string LBL = " LBL";

            public char[] Lbl { get; set; }
            public UInt32 NumberOfStringPairs { get; set; }
            public UInt32 LabelNameLength { get; set; }
            public char[] LabelName { get; set; }

            public CsfLabelHeader()
            {
                Lbl                 = "".ToCharArray();
                NumberOfStringPairs = 0;
                LabelNameLength     = 0;
                LabelName           = "".ToCharArray();
            }
        }

        public class CsfLabelValue
        {
            public static readonly string STR_REVERSED  = " RTS";
            public static readonly string STRW_REVERSED = "WRTS";
            public char[] RtsOrWrts { get; set; }
            public UInt32 ValueLength { get; set; }
            public byte[] Value { get; set; }
            public UInt32 ExtraValueLength { get; set; }
            public char[] ExtraValue { get; set; }

            public CsfLabelValue()
            {
                char[] RtsOrWrts        =  "".ToCharArray();
                UInt32 ValueLength      = 0;
                UInt32 ExtraValueLength = 0;
                char[] ExtraValue       = "".ToCharArray();
            }
        }
        #endregion

        private CsfFileHeader Header { get; set; }

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
            if (!IsConvertable(stCategories))
                throw new StringTableParseException("Невозможно перевести указанную строковую таблицу в .csf формат, " +
                                                    "т.к. имеются пробелы в названии строки");

            FileName = fileName;
            categoriesOfTable = stCategories;

            Header = new CsfFileHeader();
            UInt32 countOfString = 0;

            stCategories.ForEach(category => countOfString += Convert.ToUInt32(category.stringsOfCategory.Count));

            Header.NumberOfLabels = Header.NumberOfStrings = countOfString;
        }

        /// <summary>
        ///     Класс для парсинга <u>.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(string fileName, CsfFileHeader header, List<StringTableCategory> stCategories) : base(fileName, stCategories)
        {
            if (!IsConvertable(stCategories))
                throw new StringTableParseException("Невозможно перевести указанную строковую таблицу в .csf формат, " +
                                                    "т.к. имеются пробелы в названии строки");
            
            FileName = fileName;
            Header = header;
            categoriesOfTable = stCategories;
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
                Header = new CsfFileHeader();

                // читаем заголовок файла, который нам укажет количество считываний далее
                Header.Csf             = br.ReadChars(4); // по факту эта строка обязана всегда быть " FSC"
                Header.CsfVersion      = br.ReadUInt32(); // у игр серии ЦНЦ это число всегда равно 3
                Header.NumberOfLabels  = br.ReadUInt32(); // количество лейблов
                Header.NumberOfStrings = br.ReadUInt32(); // количество строк
                Header.UnusedBytes     = br.ReadUInt32(); // никто не знает, что это за байты, и никто их не использует
                Header.LanguageCode    = br.ReadUInt32(); // код языка (подробнее в CsfFileHeader.LanguagesCodes)

                if (CharArrayToString(Header.Csf) != CsfFileHeader.CSF_REVERSED)
                    throw new StringTableParseException("Ошибка чтения .csf файла: заголовок не содержит строку ' FSC'");

                for (UInt32 i = 0; i < Header.NumberOfLabels || br.PeekChar() > -1; i++)
                {
                    CsfLabelHeader LabelHeader = new CsfLabelHeader();
                    CsfLabelValue  LabelValue  = new CsfLabelValue();

                    // чтение лейбла
                    //try
                    //{
                        LabelHeader.Lbl = br.ReadChars(4); // записывает строку ' LBL'
                    //}
                    //catch
                    //{ }
                    // если у нас строка не является ' LBL', то движок игры считает сл-щие 4 бита для поиска слова ' LBL'
                    if (CharArrayToString(LabelHeader.Lbl) != CsfLabelHeader.LBL)
                    {
                        //i--;
                        continue;
                    }
                            
                    LabelHeader.NumberOfStringPairs = br.ReadUInt32(); // количество строк в значении. почти всегда равно 1
                    LabelHeader.LabelNameLength     = br.ReadUInt32(); // длина названия лейбла
                    LabelHeader.LabelName           = br.ReadChars((int)LabelHeader.LabelNameLength); // само название лейбла

                    // чтение значения лейбла
                    LabelValue.RtsOrWrts   = br.ReadChars(4);
                    LabelValue.ValueLength = br.ReadUInt32();
                    LabelValue.Value       = br.ReadBytes((int)(LabelValue.ValueLength * 2));

                    // зачатки кода по чтению дополнительных значений
                    //if(CharArrayToString(LabelValue.RtsOrWrts) == CsfLabelValue.STRW_REVERSED)
                    //{
                    //    LabelValue.ExtraValueLength = br.ReadUInt32();
                    //    LabelValue.ExtraValue       = br.ReadChars((int)LabelValue.ExtraValueLength);
                    //}

                    InvertAllBytesInArray(LabelValue.Value);

                    string categoryName = string.Empty;
                    string stringName = string.Empty;

                    StringBuilder sb = new StringBuilder();
                    int j = 0;
                    foreach(var str in CharArrayToString(LabelHeader.LabelName).Split(':'))
                    {
                        j++;
                        switch(j)
                        {
                            case 1:
                                categoryName = str;
                                break;
                            case 2:
                                sb.Append(str);
                                break;
                            default:
                                sb.Append(":" + str);
                                break;
                        }
                    }

                    stringName = sb.ToString();

                    if (j == 1)
                    {
                        stringName = categoryName;
                        categoryName = NO_CATEGORY_STRINGS;
                    }

                    StringTableCategory category = new StringTableCategory(categoryName);
                    category.AddString(stringName, new string(FileEncoding.GetChars(LabelValue.Value)));

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
                // записываем хедер файла
                bw.Write(Header.Csf);
                bw.Write(Header.CsfVersion);
                bw.Write(Header.NumberOfLabels);
                bw.Write(Header.NumberOfStrings);
                bw.Write(Header.UnusedBytes);
                bw.Write(Header.LanguageCode);

                // записываем построчно значения из строковой таблицы в файл
                for (UInt32 counterOfLables = 0; counterOfLables < Header.NumberOfLabels; counterOfLables++)
                {
                    foreach (var category in categoriesOfTable)
                    {
                        foreach (var str in category.stringsOfCategory)
                        {
                            string labelName = category.CategoryName + ":" + str.StringName;

                            bw.Write(CsfLabelHeader.LBL.ToCharArray()); // строка со значением ' LBL'
                            bw.Write((uint)1);                          // количество строк для дополнительного значения
                            bw.Write(labelName.Length);                 // длина названия
                            bw.Write(labelName.ToCharArray());          // само название

                            bw.Write(CsfLabelValue.STR_REVERSED.ToCharArray()); // проигнорируем на данный момент расширенное форматирование строк

                            // получение значения длины строки символов в байтах
                            UInt32 length = Convert.ToUInt32(Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(str.StringValue)).Length);
                            // если у нас кодировка - это реализация Unicode, то количество символов должно быть сокращено в двое
                            if (FileEncoding != Encoding.Unicode && FileEncoding != Encoding.UTF32)
                                length /= 2;

                            bw.Write(length); // запись длины значения

                            byte[] byteValue = FileEncoding.GetBytes(str.StringValue); // получения байтового массива на основе строки
                            InvertAllBytesInArray(byteValue); // инвертирование полученного массива

                            bw.Write(byteValue); // запись в файл инвертированных байтов значения строки

                            counterOfLables++; // т.к. мы прошли лейбл, то мы обязаны увеличить счётчик на 1
                        }
                    }
                }
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

        private string CharArrayToString(char[] array)
        {
            StringBuilder sb = new StringBuilder();

            if (array == null) return null;

            foreach (var ch in array)
                sb.Append(ch);

            return sb.ToString();
        }

        private string CharArrayToString(byte[] array)
        {
            StringBuilder sb = new StringBuilder();

            if (array == null) return null;

            foreach (var ch in array)
                sb.Append((char)ch);

            return sb.ToString();
        }

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
