using System.ComponentModel.DataAnnotations;

namespace Common.Commons.Validators;
[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property|AttributeTargets.Parameter, AllowMultiple = false)]
public class RequiredGuidAttribute:ValidationAttribute
{
    public const string DefaultErrorMessage = "The field {0} is required and not Guid.Empty.";

    public RequiredGuidAttribute() : base(DefaultErrorMessage) { }

    public override bool IsValid(object value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is Guid)
        {
            Guid guid = (Guid)value;
            return guid!=Guid.Empty;
        }
        else
        {
            return false;
        }
    }
}