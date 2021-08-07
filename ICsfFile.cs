namespace mah_boi.Tools
{
    interface ICsfFile : IStringTable
    {
        StrFile ToStr();
        StrFile ToStr(string fileName);
        StrFile ToStr(CsfFile fileSample);
    }
}
