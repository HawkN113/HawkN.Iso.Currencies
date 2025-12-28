namespace HawkN.Iso.Currencies.Models;

/// <summary>
/// Represents the result of a currency validation operation.
/// </summary>
public sealed class ValidationResult
{
    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success() =>
        new()
        {
            Reason = null,
            ValidationType = ValidationType.None,
            IsValid = true
        };

    /// <summary>
    /// Creates a failed validation result with a reason and validation type.
    /// </summary>
    /// <param name="reason">The reason why the validation failed.</param>
    /// <param name="type">The type of validation that failed (default: None).</param>
    public static ValidationResult Invalid(string reason, ValidationType type = ValidationType.None) =>
        new()
        {
            Reason = reason,
            ValidationType = type,
            IsValid = false
        };

    /// <summary>
    /// Indicates whether the validation succeeded.
    /// </summary>
    public bool IsValid { get; private set; }

    /// <summary>
    /// Provides the reason why validation failed, if applicable.
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// The type of validation performed.
    /// </summary>
    private ValidationType ValidationType { get; set; }
}

/// <summary>
/// Defines the type of validation that was performed.
/// </summary>
public enum ValidationType
{
    /// <summary>No validation type specified.</summary>
    None = 0,

    /// <summary>Validation performed on a value (name).</summary>
    Value = 1,

    /// <summary>Validation performed on a currency code.</summary>
    Code = 2
}