namespace HawkN.Iso.Currencies.Builders.Abstractions;

/// <summary>
/// Builder interface for including specific currencies by codes, names, or numeric codes.
/// </summary>
public interface IIncludeFilterBuilder
{
    /// <summary>
    /// Including with codes
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    IIncludeFilterBuilder Codes(params string[] codes);
    /// <summary>
    /// Including with names
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    IIncludeFilterBuilder Names(params string[] names);
    /// <summary>
    /// Including with numeric codes
    /// </summary>
    /// <param name="numericCodes"></param>
    /// <returns></returns>
    IIncludeFilterBuilder NumericCodes(params int[] numericCodes);
}