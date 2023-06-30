using Nox;
using Nox.Mermaid;
using Nox.Solution;

namespace ErdTests;

public class SimpleTests
{
    [Test]
    public async Task Can_create_a_diagram_for_entities()
    {
        var solution = new NoxSolutionBuilder()
            .UseYamlFile("/home/jan/demo/NoxYamlTest/.nox/design/NoxYamlTest.solution.nox.yaml")
            .Build();
        Directory.CreateDirectory("./output");
        var outputPath = "./output/erd.mermaid.js";
        var erd = new ErdParser(solution)
            .Parse();
        
        await File.WriteAllTextAsync(outputPath, erd);
    }
}