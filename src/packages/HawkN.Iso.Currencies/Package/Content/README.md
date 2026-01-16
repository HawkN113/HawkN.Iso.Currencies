# HawkN.Iso.Currencies

**HawkN.Iso.Currencies** provides ISO 4217 currency codes, historical currency data, and replacement mappings.

## Features
- **Actual currency list** - Provides a complete and up-to-date set of currency codes and their details according to the ISO 4217 standard.
- **Strongly typed currency codes** – `CurrencyCode` enum is generated at compile-time.
- **Historical currency support** – Access withdrawn currencies.
- **Lightweight & Dependency-Free** – Minimal overhead, compatible with .NET 8 and above.
- **Integration ready** – Use in libraries, console apps, or web applications.

---

## Getting Started

### Install via NuGet

```bash
dotnet add package HawkN.Iso.Currencies
```
---

### Required Namespaces
```csharp
using HawkN.Iso.Currencies;
using HawkN.Iso.Currencies.Abstractions;
using HawkN.Iso.Currencies.Models;
using HawkN.Iso.Currencies.Extensions;
```
---

### Usage Example

#### Registration
Use extension method `.AddCurrencyService();`
```csharp
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCurrencyService();
    })
    .Build();
```
To get service instance:
```csharp
var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
```
or inject
```csharp
app.MapGet("/weatherforecast", ([FromServices] ICurrencyService currencyService) => ...
````

#### Get all existing currencies
```csharp
var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
var currencies = currencyService?.Query()
    .Includes
        .Type(CurrencyType.Fiat)
        .Type(CurrencyType.SpecialUnit)
        .Type(CurrencyType.SpecialReserve)
        .Type(CurrencyType.PreciousMetal)
   .Build();
```

#### Get fiat currencies
```csharp
var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
var currencies = currencyService?.Query()
   .Includes
        .Type(CurrencyType.Fiat)
   .Build();
```

#### Get currencies by query
Excludes `EUR` and `USD` from the list: 
```csharp
var currencies = currencyService?.Query()
   .Includes
        .Type(CurrencyType.Fiat)
   .Without(w => w.Codes(nameof(CurrencyCode.EUR), nameof(CurrencyCode.USD)))
   .Build();
```

Includes only `EUR` and `USD` in the list:
```csharp
var currencies = currencyService?.Query()
   .Includes
        .Type(CurrencyType.Fiat)
   .With(w => w.Codes("EUR", "usd"))
   .Build();
```

#### Get currencies by advanced query (LINQ)
Includes only `EUR` and `USD` in the list:
```csharp
var currencies = currencyService?.Query()
   .Includes
        .Type(CurrencyType.Fiat)
   .Where(q => q.Code is "EUR" or nameof(CurrencyCode.USD))
   .Build();
```

#### Get historical currencies
```csharp
var historical = currencyService.GetAllHistorical();
foreach (var currency in historical)
{
    Console.WriteLine($"{currency.Code} - {currency.Name} (Withdrawn: {currency.WithdrawnOn})");
}
```

#### Lookup currency
By string code
```csharp
var afnWithString = currencyService.Get("AFN");
```
By currency code
```csharp
var afnWithCode = currencyService.Get(CurrencyCode.AFN);
```

#### Validate currency
By string code
```csharp
var validResult = currencyService.TryValidate("AFN", out var validateResult);
```
By currency code
```csharp
var validResult = currencyService.TryValidate(CurrencyCode.AFN, out var validateResult);
```
---

## Supported currencies
See the currency list with the [link](https://github.com/HawkN113/HawkN.Iso.Currencies?tab=readme-ov-file#supported-currencies)
Last updated at `16.01.2026`

---

## Generated Types
- `CurrencyCode` – strongly-typed enum with all ISO 4217 codes.
- `Currency` – domain model representing a currency (code, name, numeric code, withdrawn date).

---

## License

### Code
This project’s source code is licensed under the [MIT License](LICENSE).

### Data
This project uses data derived from the following sources:

- **Unicode Common Locale Data Repository (CLDR)**  
  Licensed under the [Unicode License Agreement](https://unicode.org/license.html).

- **ISO 4217 currency codes dataset**  
  Source: https://github.com/datasets/currency-codes  
  Licensed under the **Open Database License (ODbL) v1.0**.

The above data licenses are **permissive and compatible with MIT-licensed code**  
when used for reference and code generation.

See [DATA-LICENSE](DATA-LICENSE) for details.

---

## References
- [ISO 4217 Standard](https://www.iso.org/iso-4217-currency-codes.html)
- [GitHub Repository](https://github.com/HawkN113/HawkN.Iso.Currencies)

