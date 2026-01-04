using HawkN.Iso.Currencies.Builders.Abstractions;
using HawkN.Iso.Currencies.Models;
namespace HawkN.Iso.Currencies.Builders;

internal sealed class CurrencyQueryBuilder :
    ICurrencyQueryStart,
    ICurrencyQueryTypeSelector,
    ICurrencyQueryFilter,
    ICurrencyQueryNoWhereFilter,
    IIncludeFilterBuilder,
    IExcludeFilterBuilder
{
    private readonly IReadOnlyList<Currency> _actualCurrencies;
    private readonly HashSet<CurrencyType> _includedTypes = [];
    private readonly HashSet<string> _withCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withNumericCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNumericCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Func<Currency, bool>> _customPredicates = [];

    public CurrencyQueryBuilder(IReadOnlyList<Currency> currencies)
    {
        _actualCurrencies = currencies;
        Includes = this;
    }

    public ICurrencyQueryTypeSelector Includes { get; }

    public ICurrencyQueryFilter Type(CurrencyType type)
    {
        if (!_includedTypes.Add(type))
            throw new InvalidOperationException($"CurrencyType '{type}' is already included.");
        return this;
    }

    public ICurrencyQueryNoWhereFilter With(Action<IIncludeFilterBuilder> configure)
    {
        configure(this);
        return this;
    }

    public ICurrencyQueryNoWhereFilter Without(Action<IExcludeFilterBuilder> configure)
    {
        configure(this);
        return this;
    }

    public ICurrencyQueryFinal Where(Func<Currency, bool> predicate)
    {
        _customPredicates.Add(predicate);
        return this;
    }

    public IReadOnlyList<Currency> Build()
    {
        Func<Currency, bool> baseFilter = MatchesAllBaseFilters;

        return _actualCurrencies
            .Where(baseFilter)
            .Where(c => _customPredicates.All(p => p(c)))
            .ToList();
    }

    private bool MatchesAllBaseFilters(Currency currency)
    {
        return MatchesIncludedTypes(currency)
               && MatchesCodeFilters(currency)
               && MatchesNameFilters(currency)
               && MatchesNumericCodeFilters(currency);
    }

    private bool MatchesIncludedTypes(Currency currency)
    {
        return _includedTypes.Contains(currency.CurrencyType!.Value);
    }

    private bool MatchesCodeFilters(Currency currency)
    {
        return (_withCodes.Count == 0 || _withCodes.Contains(currency.Code))
               && (_withoutCodes.Count == 0 || !_withoutCodes.Contains(currency.Code));
    }

    private bool MatchesNameFilters(Currency currency)
    {
        return (_withNames.Count == 0 || _withNames.Contains(currency.Name))
               && (_withoutNames.Count == 0 || !_withoutNames.Contains(currency.Name));
    }

    private bool MatchesNumericCodeFilters(Currency currency)
    {
        var numericCode = currency.NumericCodeString;
        var hasNumericCode = !string.IsNullOrEmpty(numericCode);

        var matchesWithNumericCodes =
            _withNumericCodes.Count == 0
            || (hasNumericCode && _withNumericCodes.Contains(numericCode!));

        var matchesWithoutNumericCodes =
            _withoutNumericCodes.Count == 0
            || (hasNumericCode && !_withoutNumericCodes.Contains(numericCode!));

        return matchesWithNumericCodes && matchesWithoutNumericCodes;
    }

    IIncludeFilterBuilder IIncludeFilterBuilder.Codes(params string[] codes)
    {
        foreach (var code in codes) _withCodes.Add(code);
        return this;
    }

    IIncludeFilterBuilder IIncludeFilterBuilder.Names(params string[] names)
    {
        foreach (var name in names) _withNames.Add(name);
        return this;
    }

    IIncludeFilterBuilder IIncludeFilterBuilder.NumericCodes(params int[] numericCodes)
    {
        foreach (var nc in numericCodes) _withNumericCodes.Add(nc.ToString());
        return this;
    }

    IExcludeFilterBuilder IExcludeFilterBuilder.Codes(params string[] codes)
    {
        foreach (var code in codes) _withoutCodes.Add(code);
        return this;
    }

    IExcludeFilterBuilder IExcludeFilterBuilder.Names(params string[] names)
    {
        foreach (var name in names) _withoutNames.Add(name);
        return this;
    }

    IExcludeFilterBuilder IExcludeFilterBuilder.NumericCodes(params int[] numericCodes)
    {
        foreach (var nc in numericCodes) _withoutNumericCodes.Add(nc.ToString());
        return this;
    }
}