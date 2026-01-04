using System.Text.RegularExpressions;
namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class JsonReplacementCurrencyHandler(string jsonContent)
{
    public Dictionary<string, string> LoadNameReplacements()
    {
        var matches = Regex.Matches(jsonContent, @"\{([^}]*)\}");
        var replacements = (from Match match in matches
                            select match.Groups[1].Value
                into obj
                            let code = Extract(obj, "Ccy")
                            let name = Extract(obj, "CcyNm")
                            where !string.IsNullOrEmpty(code)
                            select new { code, name })
            .ToDictionary(x => x.code, x => x.name, StringComparer.OrdinalIgnoreCase);
        return replacements;
    }

    private static string Extract(string json, string key)
    {
        var pattern = $"\"{Regex.Escape(key)}\"\\s*:\\s*\"([^\"]+)\"";
        var match = Regex.Match(json, pattern);
        return match.Success ? match.Groups[1].Value : "";
    }
}