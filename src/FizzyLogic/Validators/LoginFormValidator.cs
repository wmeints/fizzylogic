
namespace FizzyLogic.Validators
{
    using FizzyLogic.Forms;
    using FluentValidation;

    public class LoginFormValidator : AbstractValidator<LoginForm>
    {
        public LoginFormValidator()
        {
            _ = RuleFor(x => x.EmailAddress).EmailAddress().NotEmpty();
            _ = RuleFor(x => x.Password).NotEmpty();
        }
    }
}