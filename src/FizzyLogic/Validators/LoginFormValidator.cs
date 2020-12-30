using FizzyLogic.Forms;
using FluentValidation;

namespace FizzyLogic.Validators
{
    public class LoginFormValidator: AbstractValidator<LoginForm>
    {
        public LoginFormValidator()
        {
            RuleFor(x => x.EmailAddress).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}