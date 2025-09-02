using System.Text;
using mah_boi.Tools.Extensions;
using mah_boi.Tools.StringTableFormats;

namespace mah_boi.Tools.Tests;

[TestClass]
public sealed class StringTableTests
{
    private Encoding? cp1251 = null;
    private readonly Encoding? unicode = Encoding.Unicode;
    private readonly Encoding? uft8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    private readonly Encoding? ascii = Encoding.ASCII;
    private readonly StringTable stModification = new StrFile() 
    {
        { "Name:Test", "Test value" }
    };
    private readonly StringTableEntry steTest = new("Focus", "Pocus", "Amogus");
    private readonly DirectoryInfo dataSamples = 
        new(new DirectoryInfo(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName + "\\DataSamples");

    #region Opening
    public StringTableTests()
    {
        // https://stackoverflow.com/questions/3967716/how-to-find-encoding-for-1251-codepage
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        cp1251 = Encoding.GetEncoding("windows-1251");
    }

    [TestMethod]
    public void Open_CSF()
    {
        int count = 0;
        dataSamples.GetFiles().Where(f => f.Extension == "csf").ForEach(f => count = new CsfFile(f.FullName).Count());
    }

    [TestMethod]
    public void Open_STR()
    {
        int count = 0;
        dataSamples.GetFiles().Where(f => f.Extension == "str").ForEach(f => count = new CsfFile(f.FullName).Count());
    }

    [TestMethod]
    public void Open_TXT()
    {
        int count = 0;
        dataSamples.GetFiles().Where(f => f.Extension == "txt").ForEach(f => count = new CsfFile(f.FullName).Count());
    }

    [TestMethod]
    public void Open_INI()
    {
        int count = 0;
        dataSamples.GetFiles().Where(f => f.Extension == "ini").ForEach(f => count = new CsfFile(f.FullName).Count());
    }
    #endregion

    #region Mutation
    [TestMethod]
    public void Modification_CSF()
    {
        CsfFile expected = new(stModification);
        
        CsfFile result = new(stModification);
        result.Add(steTest);
        result.DeleteExtraValues();
        result.DeleteStringByName(steTest.Name);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Modification_STR()
    {
        StrFile expected = new(stModification);

        StrFile result = new(stModification);
        result.Add(steTest);
        result.DeleteStringsOnMatch(steTest);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Modification_TXT()
    {
        StarkkuTxtFormat expected = new(stModification);

        StarkkuTxtFormat result = new(stModification);
        result.Add(steTest);
        result.Delete(steTest);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Modification_INI()
    {
        StringTableIniFile expected = new(stModification);

        StringTableIniFile result = new(stModification);
        result.Add(steTest);
        result.DeleteStringsOnMatch(steTest);

        Assert.AreEqual(expected, result);
    }
    #endregion

    [TestMethod]
    public void Converting_AllTypes()
    {
        string pathConvertTest_SourceCsf = @"mah_boi.Tools\csf2str_orig.csf";
        string pathConvertTest_ResultStr = @"mah_boi.Tools\csf2str_rslt.str";
        string pathConvertTest_SourceStr = @"mah_boi.Tools\str2csf_orig.str";
        string pathConvertTest_ResultCsf = @"mah_boi.Tools\str2csf_rslt.csf";

        Console.WriteLine($"Парсинг CSF файла по пути {pathConvertTest_SourceCsf}");
        CsfFile csf = new CsfFile(pathConvertTest_SourceCsf);
        Console.WriteLine($"Конвертация файла и сохранение его в {pathConvertTest_ResultStr}");
        new StrFile(csf).SaveAs(pathConvertTest_ResultStr);

        Console.WriteLine();

        Console.WriteLine($"Парсинг STR файла по пути {pathConvertTest_SourceStr}");
        StrFile str = new StrFile(pathConvertTest_SourceStr, Encoding.UTF8);
        Console.WriteLine($"Конвертация файла и сохранение его в {pathConvertTest_ResultCsf}");
        new CsfFile(str).SaveAs(pathConvertTest_ResultCsf);
    }
}
