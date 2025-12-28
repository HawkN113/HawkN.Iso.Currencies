using System.Text.RegularExpressions;
using HawkN.Iso.Currencies.Generators.Utility;
namespace HawkN.Iso.Currencies.Generators.Handlers;

internal abstract class JsonCurrencyHandlerBase(string jsonContent)
{
    protected abstract string ArrayKey { get; }
    protected abstract bool IsHistorical { get; }

    public CurrencyDataSet LoadCurrencies()
    {
        var publishedDate = JsonParser.ExtractPublishedDate(jsonContent);
        var arrayContent = JsonParser.ExtractArray(jsonContent, ArrayKey);
        var currencies = ParseCurrencies(arrayContent);

        return new CurrencyDataSet
        {
            PublishedDate = publishedDate,
            Currencies = currencies
                .GroupBy(c => c.Code)
                .Select(g => g.First())
                .ToList()
        };
    }

    private List<CurrencyRaw> ParseCurrencies(string arrayContent)
    {
        var currencies = new List<CurrencyRaw>();
        var (depth, inString, escape, objStart) = (0, false, false, -1);

        for (var i = 0; i < arrayContent.Length; i++)
        {
            var ch = arrayContent[i];
            if (JsonParser.HandleEscapeChar(ch, ref inString, ref escape) || inString)
                continue;

            switch (ch)
            {
                case '{':
                    if (depth == 0) objStart = i;
                    depth++;
                    break;
                case '}':
                    depth--;
                    if (depth == 0 && objStart >= 0)
                    {
                        var obj = arrayContent.Substring(objStart, i - objStart + 1);
                        var currency = ParseCurrency(obj);
                        if (currency != null)
                            currencies.Add(currency);
                        objStart = -1;
                    }
                    break;
            }
        }

        return currencies;
    }

    private CurrencyRaw? ParseCurrency(string obj)
    {
        var code = JsonParser.Extract(obj, "Ccy");
        if (string.IsNullOrEmpty(code))
            return null;

        var name = ExtractCurrencyName(obj);
        var country = JsonParser.Extract(obj, "CtryNm") ?? string.Empty;
        var num = JsonParser.Extract(obj, "CcyNbr");
        var withdrawalDate = IsHistorical ? JsonParser.Extract(obj, "WthdrwlDt") : null;

        return new CurrencyRaw
        {
            Code = code!,
            Name = name,
            Country = country,
            NumericCode = string.IsNullOrEmpty(num) ? null : num,
            WithdrawalDate = string.IsNullOrEmpty(withdrawalDate) ? null : withdrawalDate
        };
    }

    private static string ExtractCurrencyName(string obj)
    {
        var name = JsonParser.Extract(obj, "CcyNm");
        if (!string.IsNullOrEmpty(name) &&
            !name!.TrimStart().StartsWith("{") &&
            !name.Contains("\"__text\""))
            return name;

        var innerMatch = Regex.Match(obj,
            @"""__text""\s*:\s*""([^""]*)""",
            RegexOptions.Compiled | RegexOptions.Singleline);

        return innerMatch.Success
            ? innerMatch.Groups[1].Value
            : name ?? string.Empty;
    }
}