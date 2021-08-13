using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            if (!IsConvertable(stCategories))
                throw new StringTableParseException("Невозможно перевести указанную строковую таблицу в .csf формат, " +
                                                    "т.к. имеются пробелы в названии строки");

            FileName = fileName;
            categoriesOfTable = stCategories;

            UInt32 countOfString = 0;

            stCategories.ForEach(category => countOfString += Convert.ToUInt32(category.stringsOfCategory.Count));
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
                char[] csf             = br.ReadChars(4); // по факту эта строка обязана всегда быть " FSC"

                if (new string(csf) != new string(FSC))
                    throw new StringTableParseException("Ошибка чтения .csf файла: заголовок не содержит строку ' FSC'");

                br.ReadUInt32(); // у игр серии ЦНЦ это число всегда равно 3. по факту ни на что не влияет
                UInt32 numberOfLabels  = br.ReadUInt32(); // количество лейблов
                UInt32 numberOfStrings = br.ReadUInt32(); // количество строк

                if (numberOfLabels != numberOfStrings)
                    throw new StringTableParseException("Ошибка чтения .csf файла: имеются не пустые поля дополнительных значений строк (класс CsfFile не умеет обрабатывать)");

                br.ReadUInt32(); // никто не знает, что это за байты, и никто их не использует (и этот класс пока что тоже не использует)
                br.ReadUInt32(); // код языка (подробнее в LanguagesCodes)

                for (UInt32 i = 0; i < numberOfLabels || br.PeekChar() > -1; i++)
                {
                    // чтение лейбла

                    // чтение иногда даёт ошибку при попадании на некоторые пустые строки. чтение generals.csf уж точно даст исключение
                    //try
                    //{
                        char[] lbl = br.ReadChars(4); // записывает строку ' LBL'
                    //}
                    //catch
                    //{ }

                    // если у нас строка не является ' LBL', то движок игры считает сл-щие 4 бита для поиска слова ' LBL'
                    if (new string(lbl) != new string(LBL))
                    {
                        i--;
                        continue;
                    }
                            
                    br.ReadUInt32();                                             // количество строк в значении. почти всегда равно 1
                                                                                 // а если не равно 1, то имеется дополнительные значения у строки,
                                                                                 // которые на данный момент класс не умеет обрабатывать
                    UInt32 labelNameLength = br.ReadUInt32();                    // длина названия лейбла
                    char[] labelName       = br.ReadChars((int)labelNameLength); // само название лейбла

                    // чтение значения лейбла
                    char[] rtsOrWrts   = br.ReadChars(4);                      // ' RTS' - доп. значения нет. 'WRTS' - доп. значение есть.
                    UInt32 valueLength = br.ReadUInt32();                      // длина строки юникода, укороченная вдвое
                    byte[] stringValue = br.ReadBytes((int)(valueLength * 2)); // строка, конвертированная в интертированные байты

                    // зачатки кода по чтению дополнительных значений
                    //if(CharArrayToString(LabelValue.RtsOrWrts) == CsfLabelValue.STRW_REVERSED)
                    //{
                    //    LabelValue.ExtraValueLength = br.ReadUInt32();
                    //    LabelValue.ExtraValue       = br.ReadChars((int)LabelValue.ExtraValueLength);
                    //}

                    InvertAllBytesInArray(stringValue);

                    // преобразование считанных данных во внутренний формат представления стоковой таблицы
                    string categoryName = string.Empty;
                    string stringName = string.Empty;

                    StringBuilder sb = new StringBuilder();
                    int j = 0;
                    foreach(var str in new string(labelName).Split(':'))
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
                UInt32 countOfLables = (UInt32)Count();

                // записываем хедер файла
                bw.Write(FSC);
                bw.Write(CNC_CSF_VERSION);
                bw.Write(countOfLables);
                bw.Write(countOfLables);
                bw.Write((UInt32)0);
                bw.Write((UInt32)0);

                // записываем построчно значения из строковой таблицы в файл
                UInt32 counterOfLables = 0;
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

                            // проигнорируем на данный момент расширенное форматирование строк
                            bw.Write(RTS); // строка со значением ' RTS'

                            // получение значения длины строки символов в байтах
                            UInt32 labelValueLength = Convert.ToUInt32(Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(str.StringValue)).Length);
                            // если у нас кодировка - это реализация Unicode, то количество символов должно быть сокращено в двое
                            if (FileEncoding != Encoding.Unicode && FileEncoding != Encoding.UTF32)
                                labelValueLength /= 2;

                            bw.Write(labelValueLength); // запись длины значения

                            byte[] byteValue = FileEncoding.GetBytes(str.StringValue); // получения байтового массива на основе строки
                            InvertAllBytesInArray(byteValue); // инвертирование полученного массива

                            bw.Write(byteValue); // запись в файл инвертированных байтов значения строки

                            counterOfLables++; // т.к. мы прошли лейбл, то мы обязаны увеличить счётчик на 1
                        }
                    }
                } while (counterOfLables < countOfLables);
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
