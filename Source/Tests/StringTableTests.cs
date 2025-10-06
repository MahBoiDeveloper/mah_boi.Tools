using System.Text;
using mah_boi.Tools.Extensions;
using mah_boi.Tools.StringTableFormats;

namespace mah_boi.Tools.Tests;

[TestClass]
public sealed class StringTableTests
{
    #region Data
    private Encoding? cp1251 = null;
    private readonly Encoding? unicode = Encoding.Unicode;
    private readonly Encoding? uft8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    private readonly Encoding? ascii = Encoding.ASCII;
    private readonly AbstractStringTable stModification = new StrFile() 
    {
        { "Name:Test", "Test value" }
    };
    private readonly StringTableEntry steTest = new("Focus", "Pocus", "Amogus");
    private readonly DirectoryInfo dataSamples = 
        new(new DirectoryInfo(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName + "\\DataSamples");
    #endregion
    
    public StringTableTests()
    {
        // https://stackoverflow.com/questions/3967716/how-to-find-encoding-for-1251-codepage
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        cp1251 = Encoding.GetEncoding("windows-1251");
    }

    #region Opening
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
        dataSamples.GetFiles().Where(f => f.Extension == "str").ForEach(f => count = new StrFile(f.FullName).Count());
    }

    [TestMethod]
    public void Open_TXT()
    {
        int count = 0;
        dataSamples.GetFiles().Where(f => f.Extension == "txt").ForEach(f => count = new StringTableTxtFile(f.FullName).Count());
    }

    [TestMethod]
    public void Open_INI()
    {
        int count = 0;
        dataSamples.GetFiles().Where(f => f.Extension == "ini").ForEach(f => count = new StringTableIniFile(f.FullName).Count());
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
        StringTableTxtFile expected = new(stModification);

        StringTableTxtFile result = new(stModification);
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

    #region Comparison
    [TestMethod]
    public void Comparing_Csf2Str()
    {
        var csf = new CsfFile(dataSamples.FullName + @"\Csf2Str.csf");
        var str = new StrFile(dataSamples.FullName + @"\Csf2Str.str");
        var tmp = new CsfFile(str);

        Assert.AreEqual(true, csf == tmp);
    }

    [TestMethod]
    public void Comparing_Str2Csf()
    {
        var csf = new CsfFile(dataSamples.FullName + @"\Str2Csf.csf");
        var str = new StrFile(dataSamples.FullName + @"\Str2Csf.str");
        var tmp = new StrFile(csf);

        Assert.AreEqual(true, str == tmp);
    }
    #endregion
}
