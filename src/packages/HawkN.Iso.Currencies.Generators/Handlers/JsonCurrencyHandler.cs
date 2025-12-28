namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class JsonCurrencyHandler(string jsonContent)
    : JsonCurrencyHandlerBase(jsonContent)
{
    protected override string ArrayKey => "CcyNtry";
    protected override bool IsHistorical => false;
}