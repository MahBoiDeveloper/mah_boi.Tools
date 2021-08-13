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
        static string path_csf = @"..\..\..\DataSamples\test.csf";
        static string tmp = @"..\..\..\DataSamples\_tmp.csf";
        //string path = @"..\..\..\DataSamples\gamestrings.csf";
        //string path = @"..\..\..\DataSamples\generals.csf";
        //string path = @"..\..\..\DataSamples\test.csf";
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            cp1251 = Encoding.GetEncoding("windows-1251");

            SaveTest();
        }

        static void SaveTest()
        {
            CsfFile csf = new CsfFile(path_csf, cp1251);
            csf.Save(tmp);
            Console.WriteLine("Вывод строки с значением в кодировке : " + csf.GetStringValue("Version", "Format2"));
            Console.WriteLine($"Проверка в SaveTest() окончена. Проверьте файл {tmp}");
        }
    }
}
