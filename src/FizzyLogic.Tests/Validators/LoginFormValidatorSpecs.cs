namespace FizzyLogic.Tests.Validators
{
    using Chill;
    using FizzyLogic.Validators;
    using FluentAssertions;
    using FluentValidation.Results;
    using System.Linq;
    using Xunit;

    public class LoginFormWhenEmailAddressIsNotSpecified : GivenSubject<LoginFormValidator, ValidationResult>
    {
        public LoginFormWhenEmailAddressIsNotSpecified()
        {
            WithSubject(resolver => new LoginFormValidator());

            When(() => Subject.Validate(new Forms.LoginForm
            {
                EmailAddress = "",
                Password = "123Test!"
            }));
        }

        [Fact]
        public void ThenReturnsEmailAddressEmptyError()
        {
            _ = Result.Errors
                .FirstOrDefault(x => x.ErrorMessage == "'Email Address' must not be empty.")
                .Should().NotBeNull();
        }

        [Fact]
        public void ThenReturnsEmailAddressInvalidError()
        {
            _ = Result.Errors
                .FirstOrDefault(x => x.ErrorMessage == "'Email Address' is not a valid email address.")
                .Should().NotBeNull();
        }
    }

    public class LoginFormWhenPasswordIsNotSpecified : GivenSubject<LoginFormValidator, ValidationResult>
    {
        public LoginFormWhenPasswordIsNotSpecified()
        {
            WithSubject(resolver => new LoginFormValidator());
            When(() => Subject.Validate(new Forms.LoginForm
            {
                EmailAddress = "test@domain.org",
                Password = ""
            }));
        }

        [Fact]
        public void ThenPasswordEmptyErrorIsReturned()
        {
            _ = Result.Errors
                .FirstOrDefault(x => x.ErrorMessage == "'Password' must not be empty.")
                .Should().NotBeNull();
        }
    }
}
