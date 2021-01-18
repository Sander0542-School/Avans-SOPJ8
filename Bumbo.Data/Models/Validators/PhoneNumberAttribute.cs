using System.ComponentModel.DataAnnotations;
namespace Bumbo.Data.Models.Validators
{
    public class PhoneNumberAttribute : RegularExpressionAttribute
    {
        public PhoneNumberAttribute() : base(@"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)")
        {
            ErrorMessage = "The phone number number is not valid";
        }
    }
}
