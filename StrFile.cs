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
    ///     Подробнее про особенности парсинга <see href="https://github.com/MahBoiDeveloper">здесь</see>
    /// </summary>
    ///     Это не войдёт в описание класса, но это необходимо упомянуть хотя бы здесь.
    ///     Описанный здесь формат .str файлов основывается в основном на формате mod.str
    ///     модов на игру Red Alert 3. Тем не менее, он поддерживает и не только Red Alert 3, 
    ///     но и TW, и KW, и GZH. Применяемая при написании кода терминалогия немного отличается от 
    ///     общепринятой, т.к. она более структурированная.
    ///     
    ///     Всего имеется 2 типа объектов в .str файле:
    ///     1. Лейблы
    ///     2. Значения лейблов
    ///     
    ///     Лейблы в свою очередь являются сочетанием категорий и строк, разделяемыми через двоеточие
    ///     Для лейблов не имеет значения в каком регистре они написаны.
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
    ///     У лейблов всегда имеется значение, которое указывается между ковычками "".
    ///     
    ///     Значение можно расписать на несколько строк, но тогда значение станет многострочным.
    ///     И каждая строка после первой обязана начинаться \n, а последняя строка
    ///     должна начинаться с \n и заканчиваться ".
    ///     
    ///     Заканчиваются лейблы всегда словом "end" не важно в каком регистре
    ///     
    ///     Примеры правильных лейблов:
    ///     SCRIPT:SCRIPT_EXAMPLE
    ///     "Значение строки, котороые будет выведено"
    ///     END
    ///     
    ///     SCRIPT:SCRIPT_EXAMPLE
    ///     "Значение строки, котороые будет выведено.
    ///     \n Однако разделено на несколько строк.
    ///     \n И в этой строке их 3"
    ///     END
    ///     
    ///     Some text for string
    ///     ""
    ///     END
    class StrFile : IStrFile
    {
        public const string NOCATEGORYSTRINGS = ".NOCATEGORYSTRINGS";
        private string FileName { get; set; }
        private List<StrCategory> categoriesOfFile;

        #region Конструкторы
        public StrFile() { }

        public StrFile(string fileName)
        {
            FileName = fileName;

            Parse();
        }

        public StrFile(StrFile strFile)
        {
            FileName = strFile.FileName;
            categoriesOfFile = strFile.categoriesOfFile;
        }
        #endregion

        #region Работа с парсингом и выводом
        /// <summary>
        ///     Парсинг .str файла.
        /// </summary>
        public void Parse()
        {
            string categoryName = string.Empty;
            string stringName   = string.Empty;
            string stringValue  = string.Empty;

            var tmpListOfCategory = new List<StrCategory>();

            // Согласно https://modenc.renegadeprojects.com/CSF_File_Format название строки состоит исключительно из ASCII символов
            // дополнительно к этому стоит отметить, что формат поддерживает комментарии, которые подобны комментариям из FORTRAN 66
            // т.е. комментрии должны начинаться с 1-ой буквы строки. Комметрий создаётся за счёт символов //, как в C-подобных языках
            int searchStatus = (int)LineType.Label;

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
                    throw new StrParseException("Ошибка форматирования: указано несколько двоеточий в названии лейбла"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");

                // считанная строка содержит ошибку, т.к. несколько значений
                else if
                (
                    searchStatus == (int)LineType.End && currentLine.Trim().ToLower() != "end"
                )
                    throw new StrParseException("Ошибка форматирования: после значения лейбла идёт другая строка со значением"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");

                // считанная строка содержит ошибку, т.к. нет окончания
                else if (currentLine.Trim().ToLower() == "end" && searchStatus == (int)LineType.Value)
                    throw new StrParseException("Ошибка форматирования: после названия лейбла идёт закрытие строки"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + "Воспользуйтесь ковычками \"\" для обозначения пустой строки"
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");

                // считанная строка - лейбл
                else if
                (
                    searchStatus == (int)LineType.Label      // анализируемая строка является лейблом
                    && !currentLine.Trim().StartsWith("\"")  // строка не является значением
                    && IsACIIString(currentLine)             // символы исключительно в кодировке ASCII
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

                    if (stringName == string.Empty) // если не было двоеточия, то всё название лейбла - это название строки
                    {
                        stringName = categoryName;
                        categoryName = NOCATEGORYSTRINGS;
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
                    throw new StrParseException("Ошибка форматирования: отсутствие символов \"\\n\" в начале составной строки."
                                                + Environment.NewLine
                                                + Environment.NewLine
                                                + $"Ошибка в строке: \"{currentLine}\"");
                }

                // считанная строка - окончание строки
                else if (searchStatus == (int)LineType.End && currentLine.Trim().ToLower() == "end")
                {
                    var tmpCategory = new StrCategory(categoryName);
                    tmpCategory.AddString(stringName, stringValue);
                    tmpListOfCategory.Add(tmpCategory);

                    searchStatus = (int)LineType.Label;
                }

                // на случай непредвиденных проблем
                else
                {
                    throw new StrParseException("Неизвестная ошибка форматирования."
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
        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(FileName))
                sw.WriteLine(ToString());
        }

        /// <summary>
        ///     Сохранение данных класса в .str файл с указанным именем.
        /// </summary>
        public void Save(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
                sw.WriteLine(ToString());
        }

        /// <summary>
        ///     Метод формирует строку, равносильную .str файлу.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var category in categoriesOfFile)
                foreach (var _string in category.stringsOfCategory)
                {
                    if (category.CategoryName != NOCATEGORYSTRINGS)
                    {
                        sb.AppendLine(category.CategoryName + ":" + _string.StringName);
                    }
                    else
                    {
                        sb.AppendLine(_string.StringName);
                    }
                    sb.AppendLine("\t\"" + _string.StringValue + "\"")
                      .AppendLine("END")
                      .AppendLine(string.Empty);
                }

            return sb.ToString();
        }
        #endregion

        #region Конверторы
        /// <summary>
        ///     Конвертор из .str в .csf из текущего отпарсенного файла.
        /// </summary>
        public CsfFile ConvertToCsf()
        {
            return new CsfFile();
        }

        /// <summary>
        ///     Конвертор из .str в .csf из файла с именем fileName.
        /// </summary>
        public CsfFile ConvertToCsf(string fileName)
        {
            return new CsfFile();
        }

        /// <summary>
        ///     Конвертор из .str в .csf на основе указанного отпарсенного файла fileSample.
        /// </summary>
        public CsfFile ConvertToCsf(StrFile fileSample)
        {
            return new CsfFile();
        }
        #endregion

        #region Методы работы с категориями и строками

        /// <summary>
        ///     Метод выдаёт список названий всех строк.
        /// </summary>
        public List<string> GetCategoriesNames()
        {
            var tmp = new List<string>();

            categoriesOfFile.ForEach(category => tmp.Add(category.CategoryName));

            return tmp;
        }

        /// <summary>
        ///     Поиск категории по указанному названию. Возвращает первое вхождение.
        /// </summary>
        public StrCategory GetCategory(string categoryName)
        {
            foreach (var tmp in categoriesOfFile)
                if (tmp.CategoryName == categoryName) return tmp;

            return null;
        }

        /// <summary>
        ///     Поиск категории по указанному названию. Возвращает все вхождения.
        /// </summary>
        public List<StrCategory> GetAllCategories(string categoryName) =>
            categoriesOfFile.Where(category => category.CategoryName == categoryName).ToList();

        /// <summary>
        ///     Поиск значения по указанному названию категории и строки. Возвращает первое вхождение.
        /// </summary>
        public string GetStringValue(string categoryName, string stringName)
        {
            foreach (var category in categoriesOfFile)
                if (category.CategoryName == categoryName)
                    return category.GetStringValue(stringName);

            return null;
        }

        /// <summary>
        ///     Возвращает все строки первой найденной категории.
        /// </summary>
        public List<StrString> GetCategoryStrings(string categoryName)
        {
            foreach (var category in categoriesOfFile)
                if (category.CategoryName == categoryName)
                    return category.stringsOfCategory;

            return null;
        }

        public bool CategoryExist(string categoryName)
        {
            foreach (var category in categoriesOfFile)
                if (category.CategoryName == categoryName)
                    return true;

            return false;
        }

        public bool CategoryExist(StrCategory categorySample)
        {
            foreach (var category in categoriesOfFile)
                if (category == categorySample)
                    return true;

            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .str файле.
        /// </summary>
        public bool StringExist(string stringName)
        {
            foreach (var category in categoriesOfFile)
                foreach (var str in category.stringsOfCategory)
                    if (str.StringName == stringName)
                        return true;

            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .str файле.<br/>
        ///     При нахождении первого вхождения выходит, <br/>
        ///     выдавая положительный результат.
        /// </summary>
        public bool StringExist(string categoryName, string stringName)
        {
            foreach (var category in categoriesOfFile)
                if (category.CategoryName == categoryName)
                    foreach (var str in category.stringsOfCategory)
                        if (str.StringName == stringName) 
                            return true;
            
            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .str файле.<br/>
        ///     При нахождении первого вхождения выходит, <br/>
        ///     выдавая положительный результат.
        /// </summary>
        public bool StringExist(string categoryName, StrString stringSample)
        {
            foreach (var category in categoriesOfFile)
                if (category.CategoryName == categoryName)
                    foreach (var str in category.stringsOfCategory)
                        if (str.StringName == stringSample.StringName)
                            return true;

            return false;
        }

        public void RemoveCategoryWithStrings(string categoryName)
        {
            foreach (var category in categoriesOfFile)
                if (category.CategoryName == categoryName)
                    categoriesOfFile.Remove(new StrCategory(GetCategory(categoryName)));
        }

        public void RemoveCategoryWithStrings(StrCategory categorySample) 
            =>
                categoriesOfFile.Remove(categorySample);

        public void RemoveCategoryWithoutStrings(string categoryName)
        {
            if (!CategoryExist(categoryName)) return;

            StrCategory NoCategoryStrings = GetCategory(NOCATEGORYSTRINGS);

            foreach (var category in categoriesOfFile)
                if (category.CategoryName == categoryName)
                {
                    category.stringsOfCategory.ForEach(elem => NoCategoryStrings.AddString(elem));
                    categoriesOfFile.Remove(category);
                    categoriesOfFile.Remove(new StrCategory(GetCategory(NOCATEGORYSTRINGS)));
                    categoriesOfFile.Add(NoCategoryStrings);
                }
        }

        /// <summary>
        ///     Перемещает все подходящие строки из одной категори в другую.
        /// </summary>
        public void MoveToCategory(string stringName, string oldParentCategoryName, string newParentCategoryName)
        {
            if (!StringExist(oldParentCategoryName, stringName)) return;
            if (!CategoryExist(newParentCategoryName)) return;

            List<StrCategory> list = categoriesOfFile.Where(elem => elem.CategoryName == oldParentCategoryName
                                                                 || elem.CategoryName == newParentCategoryName).ToList();

            foreach (var category in list)
            {
                if(category.CategoryName == oldParentCategoryName)
                {
                    foreach(var str in category.stringsOfCategory)
                    {
                        if(str.StringName == stringName)
                        {
                            foreach(var _category in list)
                            {
                                _category.AddString(str.StringName, str.StringValue);
                                category.RemoveString(str.StringName, str.StringValue);
                            }
                        }
                    }
                }
            }
        }

        public static bool operator ==(StrFile firstFile, StrFile secondFile)
        {
            if (firstFile.FileName != secondFile.FileName) return false;

            if (firstFile.categoriesOfFile.Count != secondFile.categoriesOfFile.Count) return false;

            for (int i = 0; i < firstFile.categoriesOfFile.Count; i++)
                if (firstFile.categoriesOfFile[i] != secondFile.categoriesOfFile[i])
                    return false;

            return true;
        }
        public static bool operator !=(StrFile firstFile, StrFile secondFile)
        {
            return !(firstFile == secondFile);
        }
        public override bool Equals(object obj)
            =>
                obj == (object)this;

        public override int GetHashCode()
            =>
                base.GetHashCode();
        #endregion

        #region Вспомогательные методы и данные
        /// <summary>
        ///     Проверка на то, состоит ли строка только из ASCII символов.
        /// </summary>
        private bool IsACIIString(string str)
        {
            // Создание байтового массива на основе символов строки
            byte[] bytesArray = Encoding.UTF8.GetBytes(str);

            // Если символ имеет номер больше 126, то он не является символом ASCII
            foreach (var tmp in bytesArray)
                if (tmp >= 127) return false;

            return true;
        }
        
        /// <summary>
        ///     Вынесенная логика из метода <see cref="Parse"></see>. Метод комбинирует список<br/>
        ///     с категориями, где у категорий имеется только по 1 строке, <br/>
        ///     в полноценные категории с множеством строк внутри себя.
        /// </summary>
        private void CombineStringsIntoCategories(List<StrCategory> list)
        {
            List<StrCategory> bufferList = new List<StrCategory>();
            StrCategory bufferCategory;

            for( ; list.Count != 0 ; )
            {
                string categoryName = list[0].CategoryName;
                // создаём категорию с названием, как у первого элемента списка, т.к. список отсортирован
                bufferCategory = new StrCategory(categoryName);

                // выделяем из списка все категории с одним именем, и затем записываем значения из них в буфер
                list.Where(elem => elem.CategoryName == categoryName).ToList()
                    .ForEach(elem => bufferCategory.AddString(elem.stringsOfCategory[0].StringName,
                                                              elem.stringsOfCategory[0].StringValue));

                bufferList.Add(bufferCategory); // получив категорию с собранными вместе строками, записываем в буферный список

                list.RemoveAll(elem => elem.CategoryName == categoryName); // очищаем строку от уже скомбинированных строк
            }

            categoriesOfFile = bufferList;
        }

        private enum LineType
        {
            Label  = 0,
            Value  = 1,
            End    = 2
        }
        #endregion
    }
}
