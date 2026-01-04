using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace HawkN.Iso.Currencies.Generators.Tests;

public static class TestHelper
{
    /// <summary>
    /// Helper method to run the incremental generator against the test source file.
    /// </summary>
    public static GeneratorDriverRunResult RunGenerator(
        IIncrementalGenerator generator,
        string sourceCode)
    {
        // Arrange: Setup the compilation environment
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        // Required references for basic C# compilation
        var refs = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.RuntimeFeature).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location)
        };

        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references: refs,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        var driver = CSharpGeneratorDriver.Create(generator);
        var run = driver.RunGenerators(compilation).GetRunResult();

        return run;
    }
}