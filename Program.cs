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
            // Код для тестов
            string path = @"..\..\..\DataSamples\test.csf";
            string _path = @"..\..\..\DataSamples\_tmp.csf";
            //string path = @"..\..\..\DataSamples\gamestrings.csf";
            //string path = @"..\..\..\DataSamples\generals.csf";
            //string path = @"..\..\..\DataSamples\test.csf";

            CsfFile csf = new CsfFile(path);

            csf.Save(_path);

            Console.WriteLine(new CsfFile(_path).ToString());
            //Console.WriteLine(csf.ToString());
        }
    }
}
