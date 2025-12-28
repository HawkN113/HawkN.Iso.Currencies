### [How to use source generator](#how-use-sourceGenerator)

- In the project __HawkN.Iso.Currencies__ use the parameter setting ``GenerateCurrencyFiles`` in the project. Set `true` value:
```json lines
  <GenerateCurrencyFiles>true</GenerateCurrencyFiles>
```
- Save changes;
- Rebuild the solution;
- Review changes in ``CurrencyCode.cs`` and ``LocalDatabase.cs``files; 
- The parameter setting ``GenerateCurrencyFiles`` set `false` value;
- Save changes again;
- Rebuild the solution again.

### How to update currencies
- Open https://www.iso.org/iso-4217-currency-codes.html
- Download an XML file https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-one.xml and convert to JSON
- Save JSON data in the file ``Content\list-original-currencies.json``
- Download an XML file https://www.six-group.com/dam/download/financial-information/data-center/iso-currrency/lists/list-three.xml and convert to JSON
- Save JSON data in the file ``Content\list-historical-currencies.json``
- Corrected names in the file ``Content\list-replacement-currency-names.json`` 
- Use command from section <a id="how-use-sourceGenerato">How to use source generator</a>