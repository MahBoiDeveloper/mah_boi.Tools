using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace mah_boi.Tools
{
    class Program
    {
        // Кодировки
        static Encoding cp1251;
        static Encoding unicode = Encoding.Unicode;
        static Encoding uft8 = Encoding.UTF8;
        static Encoding ascii = Encoding.ASCII;

        // Переменные
        static string path_str = @"..\..\..\DataSamples\test.str";
        static string path_csf = @"..\..\..\DataSamples\clean_zh.csf";
        static string tmp = @"..\..\..\DataSamples\_tmp.csf";
        //string path = @"..\..\..\DataSamples\gamestrings.csf";
        //string path = @"..\..\..\DataSamples\generals.csf";
        //string path = @"..\..\..\DataSamples\test.csf";

        static string pathSaveTest_SourceCsf = "";
        static string pathSaveTest_ResultCsf = "";
        static string pathSaveTest_SourceStr = "";
        static string pathSaveTest_ResultStr = "";

        static string pathConvertTest_SourceCsf = "";
        static string pathConvertTest_ResultStr = "";
        static string pathConvertTest_SourceStr = "";
        static string pathConvertTest_ResultCsf = "";

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            cp1251 = Encoding.GetEncoding("windows-1251");

            //StrFile str = new StrFile(path_str, uft8);
            //Console.WriteLine(str.GetParsingMessages());
            //Console.WriteLine("===========================");
            //Console.WriteLine(str.ToString());

            ReadTest();
            SaveTest();
            ConvertTest();
            AddTest();
        }

        static void ReadTest()
        {
        }

        static void AddTest()
        {
        }

        static void SaveTest()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("НАЧАЛО ТЕСТИРОВАНИЯ СОХРАНЕНИЙ");
            Console.WriteLine("***************************************************************");
            Console.WriteLine();

            Console.WriteLine($"Создание CSF файла по пути {pathSaveTest_SourceCsf}");
                CsfFile csf = new CsfFile(pathSaveTest_SourceCsf);
            Console.WriteLine("");
            Console.WriteLine("Ошибки, найденные во время парсинга");
            Console.WriteLine("===============================================================");
            Console.WriteLine(csf.GetParsingMessages());
            Console.WriteLine("===============================================================");
            Console.WriteLine();
            Console.WriteLine($"Сохранение файла по пути {pathConvertTest_ResultCsf}");
                csf.Save(pathConvertTest_ResultCsf);
            Console.WriteLine("");
            Console.Write($"Сравнение файлов {pathSaveTest_SourceCsf} и {pathConvertTest_ResultCsf}. Результат: ");
            if (csf == new CsfFile(pathConvertTest_ResultCsf))
                Console.WriteLine("==");
            else 
                Console.WriteLine("!=");

            Console.WriteLine();

            Console.WriteLine($"Создание STR файла по пути {pathSaveTest_SourceStr}");
                StrFile str = new StrFile(pathSaveTest_SourceStr);
            Console.WriteLine("");
            Console.WriteLine("Ошибки, найденные во время парсинга");
            Console.WriteLine("===============================================================");
            Console.WriteLine(str.GetParsingMessages());
            Console.WriteLine("===============================================================");
            Console.WriteLine("");
            Console.WriteLine($"Сохранение файла по пути {pathConvertTest_ResultStr}");
                str.Save(pathConvertTest_ResultStr);
            Console.WriteLine("");
            Console.Write($"Сравнение файлов {pathSaveTest_SourceStr} и {pathConvertTest_ResultStr}. Результат: ");
            if (str == new StrFile(pathConvertTest_ResultStr))
                Console.WriteLine("==");
            else 
                Console.WriteLine("!=");

            Console.WriteLine();
            Console.WriteLine("***************************************************************");
            Console.WriteLine("КОНЕЦ ТЕСТИРОВАНИЯ СОХРАНЕНИЙ");
            Console.WriteLine();
            Console.WriteLine();
        }

        static void ConvertTest()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("НАЧАЛО ТЕСТИРОВАНИЯ КОНВЕРТАЦИИ");
            Console.WriteLine("***************************************************************");
            Console.WriteLine();

            Console.WriteLine($"Парсинг CSF файла по пути {pathConvertTest_SourceCsf}");
                CsfFile csf = new CsfFile(pathConvertTest_SourceCsf);
            Console.WriteLine($"Конвертируем ли файл: {csf.IsConvertable()}");
            Console.WriteLine($"Конвертация файла и сохранение его в {pathConvertTest_ResultStr}");
                csf.ToStr().Save(pathConvertTest_ResultStr);

            Console.WriteLine();

            Console.WriteLine($"Парсинг STR файла по пути {pathConvertTest_SourceStr}");
                StrFile str = new StrFile(pathConvertTest_SourceStr);
            Console.WriteLine($"Конвертируем ли файл: {str.IsConvertable()}");
            Console.WriteLine($"Конвертация файла и сохранение его в {pathConvertTest_ResultCsf}");
                str.ToCsf().Save(pathConvertTest_ResultCsf);

            Console.WriteLine();
            Console.WriteLine("***************************************************************");
            Console.WriteLine("КОНЕЦ ТЕСТИРОВАНИЯ КОНВЕРТАЦИИ");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
