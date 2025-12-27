using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct PaymentRequiredError(
    string Code = PAYMENT_REQUIRED,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<PaymentRequiredError>
{
    /// <summary>The default status code for payment required errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.PaymentRequired;

    private static PaymentRequiredError _Self = new(PAYMENT_REQUIRED);

    /// <summary>Cached instance for common payment required errors.</summary>
    public static ref PaymentRequiredError Value => ref _Self;
}
