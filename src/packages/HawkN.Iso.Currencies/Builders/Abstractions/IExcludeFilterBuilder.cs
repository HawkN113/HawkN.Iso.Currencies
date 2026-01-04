namespace HawkN.Iso.Currencies.Builders.Abstractions;

/// <summary>
/// Builder interface for excluding specific currencies by codes, names, or numeric codes.
/// </summary>
public interface IExcludeFilterBuilder
{
    /// <summary>
    /// Excluding with codes
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    IExcludeFilterBuilder Codes(params string[] codes);
    /// <summary>
    /// Excluding with names
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    IExcludeFilterBuilder Names(params string[] names);
    /// <summary>
    /// Excluding with numeric codes
    /// </summary>
    /// <param name="numericCodes"></param>
    /// <returns></returns>
    IExcludeFilterBuilder NumericCodes(params int[] numericCodes);
}