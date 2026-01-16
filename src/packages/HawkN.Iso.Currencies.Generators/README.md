## How to use source generator

The **HawkN.Iso.Currencies** project uses a Source Generator to generate currency code files based on Unicode CLDR data.

### Steps

1. Open the `HawkN.Iso.Currencies.csproj` file.
2. Enable code generation by setting the following property:
```json lines
<GenerateCurrencyFiles>true</GenerateCurrencyFiles>
```
3. Save the changes.
4. Rebuild the solution.
5. Review the generated files:
- `CurrencyCode.cs`
- `LocalCurrencyDatabase.cs`
6. After verification, disable generation:
```json lines
<GenerateCurrencyFiles>false</GenerateCurrencyFiles>
```
7. Save changes and rebuild the solution again.

> Generation is disabled by default to avoid unstable builds, network access during CI, and to keep builds reproducible.

---

## How to update currencies data (CLDR-based)

Currency data is sourced **exclusively** from **Unicode CLDR**.  
ISO XML files, SIX Group resources, and third-party JSON repositories are **not used**.

### Data sources

- **Currency and country mapping**  
  `supplementalData.xml`  
  https://raw.githubusercontent.com/unicode-org/cldr/<release>/common/supplemental/supplementalData.xml

- **English currency names and symbols**  
  `en.xml`  
  https://raw.githubusercontent.com/unicode-org/cldr/<release>/common/main/en.xml

Replace `<release>` with the desired CLDR release version (for example: `release-48`).

## Update process

1. Update the CLDR version if needed:
```json lines
<CldrVersion>release-48</CldrVersion>
```
2. Temporarily enable CLDR downloads and code generation:
```json lines
<DownloadCldrFiles>true</DownloadCldrFiles>
<GenerateCurrencyFiles>true</GenerateCurrencyFiles>
```
3. Rebuild the project.
4. Verify that:
- CLDR XML files were downloaded
- `CurrencyCode.cs` and `LocalCurrencyDatabase.cs` were regenerated
5. Commit the generated changes to the repository.
6. Disable the flags again:
```json lines
<DownloadCldrFiles>false</DownloadCldrFiles>
<GenerateCurrencyFiles>false</GenerateCurrencyFiles>
```
7. Rebuild the project once more.

---

## Notes

- Historical currencies are excluded. Only currently active currencies from CLDR are included.
- All data is distributed under the **Unicode License Agreement**, which is **MIT-compatible**.
- The final NuGet package does **not** contain XML files or runtime parsers.
- Code generation is intended to be run manually and committed, not executed on every build.

---



