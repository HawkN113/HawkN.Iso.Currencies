using HawkN.Iso.Currencies.Generators.Models;
using Microsoft.CodeAnalysis;
namespace HawkN.Iso.Currencies.Generators.Factories;

public class ErrorFactory
{
    private readonly List<ErrorDescription> _errorDescriptors = [];
    public void Create(ErrorDescription descriptor)
    {
        if (!_errorDescriptors.Exists(q =>
                q.DiagnosticDescriptor.Id.Equals(descriptor.DiagnosticDescriptor.Id, StringComparison.OrdinalIgnoreCase) &&
                q.GeneratorType == descriptor.GeneratorType))
        {
            _errorDescriptors.Add(descriptor);
        }
    }

    public bool IsExists()
    {
        return _errorDescriptors.Any();
    }

    public void Clear()
    {
        _errorDescriptors.Clear();
    }

    public void ShowDiagnostics(SourceProductionContext context, GeneratorType type)
    {
        var descriptors = _errorDescriptors.FindAll(q => q.GeneratorType == type);
        if (!IsExists()) return;
        var list = descriptors.Select(descriptor =>
                Diagnostic.Create(descriptor.DiagnosticDescriptor, descriptor.Location, descriptor.MessageArgs))
            .ToList();
        foreach (var item in list)
            context.ReportDiagnostic(item);
    }
}