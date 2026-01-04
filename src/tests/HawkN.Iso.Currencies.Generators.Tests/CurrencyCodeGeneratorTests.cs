namespace HawkN.Iso.Currencies.Generators.Tests;

public class CurrencyCodeGeneratorTests
{
    private const string DummySource = "public class Dummy {}";
    private readonly CurrencyCodeGenerator _generator = new();

    [Fact]
    public void Initialize_Should_Register_And_Generate_Enum_On_Success()
    {
        // Act
        var runResult = TestHelper.RunGenerator(_generator, DummySource);

        // Assert
        var generated = runResult.GeneratedTrees
            .FirstOrDefault(t => t.FilePath.EndsWith("CurrencyCode.g.cs"));

        Assert.NotNull(generated);
        var text = generated.GetText().ToString();

        Assert.Contains("public enum CurrencyCode", text);
        Assert.Contains("None,", text);
    }

    [Fact]
    public void GenerateSourceOutput_Should_Include_PublishedDate_In_XmlDocs()
    {
        // Act
        var runResult = TestHelper.RunGenerator(_generator, DummySource);

        // Assert
        var generated = runResult.GeneratedTrees
            .First(t => t.FilePath.EndsWith("CurrencyCode.g.cs"));

        var text = generated.GetText().ToString();

        Assert.Contains("Last published at", text);
    }
}