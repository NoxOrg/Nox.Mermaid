using Nox;
using Nox.Mermaid;

namespace ErdTests;

public class SimpleTests
{
    [Test]
    public async Task Can_create_a_diagram_for_entities()
    {
        var config = new NoxConfigurationBuilder()
            .WithYamlFile("./files/entities.solution.nox.yaml")
            .Build();
        Directory.CreateDirectory("./output");
        var outputPath = "./output/erd.mermaid.js";
        var erd = new ErdParser(config)
            .Parse();
        
        await File.WriteAllTextAsync(outputPath, erd);
    }
}