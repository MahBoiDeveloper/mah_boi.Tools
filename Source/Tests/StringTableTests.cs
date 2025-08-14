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
    public void Modification()
    {
    }

    [TestMethod]
    public void Converting()
    {
        Init();

        string pathConvertTest_SourceCsf = @"mah_boi.Tools\csf2str_orig.csf";
        string pathConvertTest_ResultStr = @"mah_boi.Tools\csf2str_rslt.str";
        string pathConvertTest_SourceStr = @"mah_boi.Tools\str2csf_orig.str";
        string pathConvertTest_ResultCsf = @"mah_boi.Tools\str2csf_rslt.csf";

        Console.WriteLine($"Парсинг CSF файла по пути {pathConvertTest_SourceCsf}");
        CsfFile csf = new CsfFile(pathConvertTest_SourceCsf);
        Console.WriteLine($"Конвертация файла и сохранение его в {pathConvertTest_ResultStr}");
        (StringTable)csf.SaveAs(pathConvertTest_ResultStr);

        Console.WriteLine();

        Console.WriteLine($"Парсинг STR файла по пути {pathConvertTest_SourceStr}");
        StrFile str = new StrFile(pathConvertTest_SourceStr, Encoding.UTF8);
        Console.WriteLine($"Конвертируем ли файл: {str.IsConvertable()}");
        Console.WriteLine($"Конвертация файла и сохранение его в {pathConvertTest_ResultCsf}");
        str.ToCsf().SaveAs(pathConvertTest_ResultCsf);

        Console.WriteLine();
        Console.WriteLine("***************************************************************");
        Console.WriteLine("КОНЕЦ ТЕСТИРОВАНИЯ КОНВЕРТАЦИИ");
        Console.WriteLine();
        Console.WriteLine();
    }
}
