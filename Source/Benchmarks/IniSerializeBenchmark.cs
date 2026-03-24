using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Rampastring.Tools;

namespace mah_boi.Tools.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class IniSerializeBenchmark
{
    IniFile ini = new(@"Resources\Config.ini", applyBaseIni: false);

    public IniSerializeBenchmark()
    {
    }

    [Benchmark]
    public void SerializerInit()
    {
        IniSerializer iniSerializer = new(new ConversionsExtension());
        ClassSample? tcVar = null;

        IniDeserializationOptions desser_options = new()
        {
            SectionName = nameof(tcVar),
            SkipEmptyKeys = true,
            SkipUnableToParseTypes = true,
        };

        tcVar = iniSerializer.Deserialize<ClassSample>(ini, desser_options);
    }

    [Benchmark]
    public void ParserInit()
    {
        ClassSample tcVar = new();
        tcVar.Name = ini.GetStringValue(nameof(tcVar), "Name", string.Empty );
        tcVar.Description = ini.GetStringValue(nameof(tcVar), "Description", string.Empty );
        tcVar.Id = ini.GetIntValue(nameof(tcVar), "Id", 0 );
    }
}
