using HawkN.Iso.Currencies.Generators.Factories;
using HawkN.Iso.Currencies.Generators.Models;
using Microsoft.CodeAnalysis;

namespace HawkN.Iso.Currencies.Generators.Tests.Factories;

public class ErrorFactoryTests
{
    private readonly ErrorFactory _factory = new();

    private static ErrorDescription CreateDescriptor(string id = "CUR001", GeneratorType type = GeneratorType.Factory)
    {
        var descriptor = new DiagnosticDescriptor(
            id,
            "Test Title",
            "Test Message",
            "Category",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        return new ErrorDescription
        {
            DiagnosticDescriptor = descriptor,
            GeneratorType = type,
            Location = Location.None,
            MessageArgs = ["Arg1"]
        };
    }

    [Fact]
    public void Create_Should_Add_New_Error_Descriptor()
    {
        // Arrange
        var descriptor = CreateDescriptor();

        // Assert
        _factory.Create(descriptor);

        // Assert
        Assert.True(_factory.IsExists());
    }

    [Fact]
    public void Create_Should_Not_Add_Duplicate_ErrorDescriptor()
    {
        // Arrange
        var descriptor1 = CreateDescriptor("CUR001", GeneratorType.Factory);
        var descriptor2 = CreateDescriptor("cur001", GeneratorType.Factory); // same ID but lowercase

        // Assert
        _factory.Create(descriptor1);
        _factory.Create(descriptor2);

        // Act
        Assert.True(_factory.IsExists());
        Assert.Single(GetPrivateList());
    }

    [Fact]
    public void Create_Should_Allow_Same_Id_For_Different_GeneratorTypes()
    {
        // Arrange
        var descriptor1 = CreateDescriptor("CUR001", GeneratorType.Factory);
        var descriptor2 = CreateDescriptor("CUR001", GeneratorType.Currency);

        // Assert
        _factory.Create(descriptor1);
        _factory.Create(descriptor2);

        // Act
        var list = GetPrivateList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, d => d.GeneratorType == GeneratorType.Factory);
        Assert.Contains(list, d => d.GeneratorType == GeneratorType.Currency);
    }

    [Fact]
    public void IsExists_Should_Return_False_When_Empty()
    {
        // Act & Assert
        Assert.False(_factory.IsExists());
    }

    [Fact]
    public void Clear_Should_Remove_All_Descriptors()
    {
        // Arrange
        _factory.Create(CreateDescriptor());
        Assert.True(_factory.IsExists());

        // Act
        _factory.Clear();

        // Assert
        Assert.False(_factory.IsExists());
    }

    private List<ErrorDescription> GetPrivateList()
    {
        var field = typeof(ErrorFactory)
            .GetField("_errorDescriptors",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (List<ErrorDescription>)field!.GetValue(_factory)!;
    }
}