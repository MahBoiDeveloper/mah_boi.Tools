using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mah_boi.Tools.Benchmarks;

public class TestClass
{
    public string[] NamesArray { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "Description";
    public string Type { get; set; }
    public string TypeDescription { get; set; } = "Test";
    public int Id { get; set; } = 0;
    public bool IsStatic { get; set; } = false;
}
