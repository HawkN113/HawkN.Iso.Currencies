using HawkN.Iso.Currencies.Generators.Extensions;
namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class CurrencyCodesHandler(string csvContent)
{
    private const char Delimiter = ',';
    private const string HeaderAlphabeticCodeName = "AlphabeticCode";
    private const string HeaderNumericCodeName = "NumericCode";

    public Dictionary<string, string> LoadCurrencyCodes()
    {
        var codes = new Dictionary<string, string>();
        if (string.IsNullOrWhiteSpace(csvContent)) return codes;

        var span = csvContent.AsSpan();
        var headerEnd = span.IndexOf('\n');
        if (headerEnd < 0) return codes;

        var headerLine = span.Slice(0, headerEnd).TrimEnd('\r');
        var lineStart = headerEnd + 1;

        var indices = ParseHeader(headerLine, out var maxIndex);

        while (lineStart < span.Length)
        {
            var lineEnd = span.Slice(lineStart).IndexOf('\n');
            var lineSpan = lineEnd < 0
                ? span.Slice(lineStart).TrimEnd('\r')
                : span.Slice(lineStart, lineEnd).TrimEnd('\r');

            lineStart += lineEnd < 0 ? span.Length - lineStart : lineEnd + 1;

            if (TryParseCurrencyRow(lineSpan, indices, maxIndex, out var pair))
                codes[pair.Key] = pair.Value;
        }

        return codes;
    }

    private static Dictionary<string, int> ParseHeader(ReadOnlySpan<char> headerLine, out int maxIndex)
    {
        var fields = ParseFields(headerLine);
        var indices = new Dictionary<string, int>
        {
            [HeaderAlphabeticCodeName] = -1,
            [HeaderNumericCodeName] = -1
        };

        for (var i = 0; i < fields.Length; i++)
        {
            var f = fields[i];
            switch (f.ToLowerInvariant())
            {
                case var _ when f.Equals(HeaderAlphabeticCodeName, StringComparison.OrdinalIgnoreCase):
                    indices[HeaderAlphabeticCodeName] = i; break;
                case var _ when f.Equals(HeaderNumericCodeName, StringComparison.OrdinalIgnoreCase):
                    indices[HeaderNumericCodeName] = i; break;
            }
        }

        if (indices.Values.Any(v => v < 0))
            throw new InvalidDataException("CSV missing required columns.");

        maxIndex = indices.Values.Max();
        return indices;
    }

    private static bool TryParseCurrencyRow(ReadOnlySpan<char> lineSpan, Dictionary<string, int> indices, int maxIndex, out KeyValuePair<string, string> pair)
    {
        var fields = ParseFields(lineSpan);
        if (fields.Length <= maxIndex) return false;

        var alphabeticCode = fields[indices[HeaderAlphabeticCodeName]];
        var numericCode = fields[indices[HeaderNumericCodeName]];

        if (alphabeticCode.Length != 3 || numericCode.Length == 0)
            return false;

        pair = new KeyValuePair<string, string>(alphabeticCode, numericCode);
        return true;
    }

    private static string[] ParseFields(ReadOnlySpan<char> line)
    {
        var result = new List<string>();
        var start = 0;
        var inQuotes = false;

        for (var i = 0; i <= line.Length; i++)
        {
            var end = i == line.Length || (line[i] == Delimiter && !inQuotes);
            if (i < line.Length && line[i] == '"') inQuotes = !inQuotes;

            if (!end) continue;

            var field = line.Slice(start, i - start).TrimQuotes().ToString();
            result.Add(field);

            start = i + 1;
        }
        return result.ToArray();
    }
}