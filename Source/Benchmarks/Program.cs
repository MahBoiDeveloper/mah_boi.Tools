using System;
using Rampastring.Tools;
using Rampastring.Tools.Extensions;
using BenchmarkDotNet;
using BenchmarkDotNet.Running;

namespace mah_boi.Tools.Benchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<RampaToolsBenchmarks>();
        //new RampaToolsBenchmarks().SerializerInit();
    }
}
