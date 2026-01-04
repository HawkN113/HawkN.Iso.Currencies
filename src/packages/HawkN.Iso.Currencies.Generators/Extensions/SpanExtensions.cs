namespace HawkN.Iso.Currencies.Generators.Extensions;

internal static class SpanExtensions
{
    public static ReadOnlySpan<char> TrimQuotes(this ReadOnlySpan<char> span)
    {
        span = span.Trim();
        if (span.Length > 0 && span[0] == '"')
            span = span.Slice(1);
        if (span.Length > 0 && span[span.Length - 1] == '"')
            span = span.Slice(0, span.Length - 1);
        return span;
    }
}