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

        static void ConvertTest()
        {
        }

        static void ReadTest()
        {
        }

        static void AddTest()
        {
        }

        static void SaveTest()
        {
            CsfFile csf = new CsfFile(path_csf, unicode);
            csf.Save(tmp);
            Console.WriteLine($"Вывод строки с значением в кодировке из файла {path_csf} : " + csf.GetStringValue("Version:Format2"));
            Console.WriteLine(csf.GetParsingMessages());
            Console.WriteLine($"Вывод строки с значением в кодировке из файла {tmp} : " + new CsfFile(tmp, unicode).GetStringValue("Version:Format2"));
            Console.WriteLine(new CsfFile(tmp, unicode).GetParsingMessages());
            Console.WriteLine($"Проверка в SaveTest() окончена. Проверьте файл {tmp}");
        }
    }
}
