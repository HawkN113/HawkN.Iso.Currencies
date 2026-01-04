using Microsoft.CodeAnalysis;
namespace HawkN.Iso.Currencies.Generators.Models;

public class ErrorDescription
{
    public DiagnosticDescriptor DiagnosticDescriptor { get; set; } = null!;
    public object?[]? MessageArgs { get; set; }
    public Location? Location { get; set; } = Location.None;
    public GeneratorType GeneratorType { get; set; } = GeneratorType.Factory;
}