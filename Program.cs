using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace mah_boi.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            // Кодировки
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding cp1251 = Encoding.GetEncoding("windows-1251");
            Encoding unicode = Encoding.Unicode;
            Encoding uft8 = Encoding.UTF8;
            Encoding ascii = Encoding.ASCII;

            // Переменные
            string path_str = @"..\..\..\DataSamples\_test.str";
            string path_csf = @"..\..\..\DataSamples\_test.csf";
            string tmp = @"..\..\..\DataSamples\_tmp.csf";
            //string path = @"..\..\..\DataSamples\gamestrings.csf";
            //string path = @"..\..\..\DataSamples\generals.csf";
            //string path = @"..\..\..\DataSamples\test.csf";

            CsfFile csf = new CsfFile(path_csf, unicode);
            Console.WriteLine("=============================");
            csf.Save(tmp);
            //CsfFile _csf = new CsfFile(tmp, cp1251);
            //Console.WriteLine(csf == _csf);
        }
    }
}
