
namespace FizzyLogic.Validators
{
    using FizzyLogic.Forms;
    using FluentValidation;

    /// <summary>
    /// Specifies validation rules for the <see cref="LoginForm">.
    /// </summary>
    public class LoginFormValidator : AbstractValidator<LoginForm>
    {
        /// <summary>
        /// Initializes a new instanc of <see cref="LoginFormHelper"/>.
        /// </summary>
        public LoginFormValidator()
        {
            _ = RuleFor(x => x.EmailAddress).EmailAddress().NotEmpty();
            _ = RuleFor(x => x.Password).NotEmpty();
        }
    }
}