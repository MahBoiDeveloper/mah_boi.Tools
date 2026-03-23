using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Rampastring.Tools;
using Rampastring.Tools.Extensions;

namespace mah_boi.Tools.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class IniSerializerBenchmarks
{
    private const string iniFile =
        """
        [tcVar]
        Name=Hungry
        Description=Testing the ini serializer
        Id=10
        """;
    
    IniFile ini = new(iniFile.ToStream());

    // Legacy code
    public IniSerializerBenchmarks()
    {
        IniSerializer iniSerializer = new(new ConversionsExtension());
        TestClass tcVar = new();
        tcVar.Name = "Hungry";

        IniSerializationOptions ser_options = new()
        {
            SectionName = nameof(tcVar),
            WriteEmptyKeys = false,
            IgnoreProperties = new() { "Name" }
        };

        IniDeserializationOptions desser_options = new()
        {
            SectionName = nameof(tcVar),
            SkipEmptyKeys = false,
            IgnoreProperties = new() { "Name" }
        };

        var tmp = iniSerializer.Serialize(tcVar);

        Console.WriteLine(tmp);

        TestClass? tc2 = iniSerializer.Deserialize<TestClass>(tmp, IniSerializer.DefaultDeserializationOptions with
        {
            SectionName = nameof(TestClass),
            SkipUnableToParseTypes = false
        });

        Console.WriteLine(iniSerializer.Serialize(tc2));
    }

    [Benchmark]
    public void SerializerInit()
    {
        IniSerializer iniSerializer = new(new ConversionsExtension());
        TestClass? tcVar = null;

        IniDeserializationOptions desser_options = new()
        {
            SectionName = nameof(tcVar),
            SkipEmptyKeys = true,
            SkipUnableToParseTypes = true,
        };

        tcVar = iniSerializer.Deserialize<TestClass>(ini, desser_options);
    }

    [Benchmark]
    public void ParserInit()
    {
        TestClass tcVar = new();
        tcVar.Name = ini.GetStringValue(nameof(tcVar), "Name", string.Empty );
        tcVar.Description = ini.GetStringValue(nameof(tcVar), "Description", string.Empty );
        tcVar.Id = ini.GetIntValue(nameof(tcVar), "Id", 0 );
    }
}
