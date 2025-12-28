namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class JsonHistoricalCurrencyHandler(string jsonContent) : JsonCurrencyHandlerBase(jsonContent)
{
    protected override string ArrayKey => "HstrcCcyNtry";
    protected override bool IsHistorical => true;
}