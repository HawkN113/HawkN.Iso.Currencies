using HawkN.Iso.Currencies.Abstractions;
using HawkN.Iso.Currencies.Builders;
using HawkN.Iso.Currencies.Builders.Abstractions;
using HawkN.Iso.Currencies.Models;
namespace HawkN.Iso.Currencies.Services;

internal sealed class CurrencyService : ICurrencyService
{
    private readonly IReadOnlyDictionary<string, Currency> _actualCurrencies =
        LocalCurrencyDatabase.ActualCurrencies.ToDictionary(c => c.Code, StringComparer.OrdinalIgnoreCase);

    public bool TryValidate(string value, out ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = ValidationResult.Invalid("Value is null or empty.", ValidationType.Value);
            return false;
        }

        var isExistCurrency = Exists(value);
        result = isExistCurrency
            ? ValidationResult.Success()
            : ValidationResult.Invalid($"The currency code with value '{value}' does not exist");
        return isExistCurrency;
    }

    public bool TryValidate(CurrencyCode code, out ValidationResult result)
    {
        if (code == CurrencyCode.None)
        {
            result = ValidationResult.Invalid("Code should not be 'None'.", ValidationType.Code);
            return false;
        }

        var isExistCurrency = Exists(code);
        result = isExistCurrency
            ? ValidationResult.Success()
            : ValidationResult.Invalid($"The currency code '{code}' does not exist");
        return isExistCurrency;
    }

    public bool Exists(string value) =>
        !string.IsNullOrWhiteSpace(value) && _actualCurrencies.ContainsKey(value.Trim());

    public bool Exists(CurrencyCode code) =>
        code != CurrencyCode.None && _actualCurrencies.ContainsKey(code.ToString());

    public Currency? Get(string value)
    {
        if (!Exists(value)) return null;
        _actualCurrencies.TryGetValue(value, out var currency);
        return currency;
    }

    public Currency? Get(CurrencyCode code)
    {
        if (!Exists(code)) return null;
        _actualCurrencies.TryGetValue(code.ToString(), out var currency);
        return currency;
    }

    public ICurrencyQueryStart Query()
    {
        return new CurrencyQueryBuilder(LocalCurrencyDatabase.ActualCurrencies);
    }
}