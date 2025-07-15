using mah_boi.Tools.StringTable;
using System.Text;

namespace mah_boi.Tools.Tests;

[TestClass]
public sealed class StringTableTests
{
    private Encoding? cp1251 = null;
    private Encoding? unicode = Encoding.Unicode;
    private Encoding? uft8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    private Encoding? ascii = Encoding.ASCII;

    private void Init()
    {
        // https://stackoverflow.com/questions/3967716/how-to-find-encoding-for-1251-codepage
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        cp1251 = Encoding.GetEncoding("windows-1251");
    }

    [TestMethod]
    // TODO: Implement test.
    public void AddTest()
    {
    }

    [TestMethod]
    // TODO: Implement test.
    public void SaveTest()
    {
        Init();

        string pathSaveTest_SourceCsf    = @"";
        string pathConvertTest_ResultCsf = @"";
        
        string pathSaveTest_SourceStr    = @"";
        string pathConvertTest_ResultStr = @"";

        Console.WriteLine($"Создание CSF файла по пути {pathSaveTest_SourceCsf}");
        CsfFile csf = new CsfFile(pathSaveTest_SourceCsf);
        Console.WriteLine("Ошибки, найденные во время парсинга");
        Console.WriteLine(csf.GetParsingMessages());
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
    }

    [TestMethod]
    public void ConvertTest()
    {
        Init();

        string pathConvertTest_SourceCsf = @"C:\D-Drive\_Github\mah_boi.Tools\csf2str_orig.csf";
        string pathConvertTest_ResultStr = @"C:\D-Drive\_Github\mah_boi.Tools\csf2str_rslt.str";
        string pathConvertTest_SourceStr = @"C:\D-Drive\_Github\mah_boi.Tools\str2csf_orig.str";
        string pathConvertTest_ResultCsf = @"C:\D-Drive\_Github\mah_boi.Tools\str2csf_rslt.csf";

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
        StrFile str = new StrFile(pathConvertTest_SourceStr, Encoding.UTF8);
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
