using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace HawkN.Iso.Currencies.Generators.Utility;

internal static class JsonParser
{
    private static int FindJsonBlockEnd(string json, int startIndex, char open, char close)
    {
        if (string.IsNullOrEmpty(json) || startIndex < 0 || startIndex >= json.Length)
            return -1;

        var depth = 0;
        var inString = false;
        var escape = false;

        for (var i = startIndex; i < json.Length; i++)
        {
            var ch = json[i];
            if (HandleEscapeChar(ch, ref inString, ref escape) || inString)
                continue;
            if (ch == open)
            {
                depth++;
                continue;
            }
            if (ch != close)
                continue;
            depth--;
            if (depth == 0)
                return i;
        }
        return -1;
    }

    public static bool HandleEscapeChar(char ch, ref bool inString, ref bool escape)
    {
        if (escape)
        {
            escape = false;
            return true;
        }

        switch (ch)
        {
            case '\\':
                escape = true;
                return true;
            case '"':
                inString = !inString;
                return true;
            default:
                return false;
        }
    }

    public static string ExtractPublishedDate(string json)
    {
        if (string.IsNullOrEmpty(json))
            throw new ArgumentNullException(nameof(json));

        var match = Regex.Match(json, @"""_Pblshd""\s*:\s*""([^""]+)""", RegexOptions.Singleline);
        if (!match.Success)
            throw new InvalidOperationException("Published date ('_Pblshd') is required.");

        return match.Groups[1].Value;
    }

    public static string ExtractArray(string json, string keyName)
    {
        if (string.IsNullOrEmpty(json))
            throw new ArgumentNullException(nameof(json));
        if (string.IsNullOrEmpty(keyName))
            throw new ArgumentNullException(nameof(keyName));

        var keyMatch = Regex.Match(json, $@"""{Regex.Escape(keyName)}""\s*:\s*\[", RegexOptions.Singleline);
        if (!keyMatch.Success)
            throw new InvalidOperationException($"The block '{keyName}' is required in the JSON content.");

        var arrayStart = keyMatch.Index + keyMatch.Length - 1;
        if (arrayStart < 0 || arrayStart >= json.Length || json[arrayStart] != '[')
            throw new InvalidOperationException($"Cannot locate '[' for '{keyName}'.");

        var arrayEnd = FindJsonBlockEnd(json, arrayStart, '[', ']');
        if (arrayEnd == -1)
            throw new InvalidOperationException($"Cannot locate end of '{keyName}' array.");

        return json.Substring(arrayStart + 1, arrayEnd - arrayStart - 1);
    }

    public static string? Extract(string json, string key)
    {
        if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(key))
            return null;

        var value = ExtractString(json, key);
        if (value != null)
            return value;

        value = ExtractNumber(json, key);
        return value ?? ExtractObject(json, key);
    }

    private static string? ExtractString(string json, string key)
    {
        var match = Regex.Match(
            json,
            $@"""{Regex.Escape(key)}""\s*:\s*""((?:\\.|[^""\\])*)""",
            RegexOptions.Singleline);

        return match.Success ? Normalize(UnescapeJsonString(match.Groups[1].Value)) : null;
    }

    private static string? ExtractNumber(string json, string key)
    {
        var match = Regex.Match(
            json,
            $@"""{Regex.Escape(key)}""\s*:\s*([0-9]+)",
            RegexOptions.Singleline);

        return (match.Success ? match.Groups[1].Value : null)!;
    }

    private static string? ExtractObject(string json, string key)
    {
        var match = Regex.Match(
            json,
            $@"""{Regex.Escape(key)}""\s*:\s*\{{(.*?)\}}",
            RegexOptions.Singleline);

        if (!match.Success)
            return null;

        var body = match.Groups[1].Value;

        var textMatch = Regex.Match(
            body,
            @"""__text""\s*:\s*""((?:\\.|[^""\\])*)""",
            RegexOptions.Singleline);

        if (textMatch.Success)
            return Normalize(UnescapeJsonString(textMatch.Groups[1].Value));

        return "{" + body.Trim() + "}";
    }

    private static string UnescapeJsonString(string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        var sb = new StringBuilder(s.Length);
        var len = s.Length;
        var i = 0;

        while (i < len)
        {
            var c = s[i];
            if (c != '\\')
            {
                sb.Append(c);
                i++;
                continue;
            }

            i++;
            if (i >= len)
            {
                sb.Append('\\');
                break;
            }

            var esc = s[i];
            switch (esc)
            {
                case '"': sb.Append('"'); break;
                case '\\': sb.Append('\\'); break;
                case '/': sb.Append('/'); break;
                case 'b': sb.Append('\b'); break;
                case 'f': sb.Append('\f'); break;
                case 'n': sb.Append('\n'); break;
                case 'r': sb.Append('\r'); break;
                case 't': sb.Append('\t'); break;
                case 'u':
                    i = AppendUnicodeEscape(s, sb, i);
                    break;
                default:
                    sb.Append('\\').Append(esc);
                    break;
            }

            i++;
        }

        return sb.ToString();
    }

    private static int AppendUnicodeEscape(string s, StringBuilder sb, int i)
    {
        const int unicodeLength = 4;
        var start = i + 1;

        if (start + unicodeLength > s.Length)
        {
            sb.Append("\\u");
            return i;
        }

        var hex = s.Substring(start, unicodeLength);
        if (int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var code))
            sb.Append((char)code);
        else
            sb.Append("\\u").Append(hex);

        return i + unicodeLength;
    }

    private static string Normalize(string s)
    {
        return s.Replace("\"", "'");
    }
}