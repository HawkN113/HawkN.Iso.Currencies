namespace HawkN.Iso.Currencies.Generators.Tests;

public class LocalCurrencyDatabaseGeneratorTests
{
    private const string DummySource = "public class Dummy {}";
    private readonly LocalCurrencyDatabaseGenerator _generator = new();

    [Fact]
    public void Initialize_Should_Register_And_Generate_Database_On_Success()
    {
        // Act
        var runResult = TestHelper.RunGenerator(_generator, DummySource);

        // Assert
        var generated = runResult.GeneratedTrees
            .FirstOrDefault(t => t.FilePath.EndsWith("LocalCurrencyDatabase.g.cs"));

        Assert.NotNull(generated);
        var text = generated.GetText().ToString();

        Assert.Contains("internal static class LocalCurrencyDatabase", text);
        Assert.Contains("public static readonly ImmutableArray<Models.Currency> ActualCurrencies", text);
        Assert.Contains("new(\"USD\", \"US Dollar\",", text);
        Assert.Empty(runResult.Diagnostics);
    }

    [Fact]
    public void GenerateSourceOutput_Should_Include_PublishedDate_In_XmlDocs()
    {
        // Act
        var runResult = TestHelper.RunGenerator(_generator, DummySource);

        // Assert
        var generated = runResult.GeneratedTrees
            .First(t => t.FilePath.EndsWith("LocalCurrencyDatabase.g.cs"));

        var text = generated.GetText().ToString();
        Assert.Contains("Last published at", text);
    }
}