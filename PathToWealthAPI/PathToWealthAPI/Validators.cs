using FluentValidation;
using static PathToWealthAPI.Data.Models;

namespace PathToWealthAPI
{

    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(login => login.UsernameOrEmail)
                .NotEmpty().WithMessage("Username or email is required.");

            RuleFor(login => login.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }

    public class UserRegistrationValidator : AbstractValidator<UserRegistration>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
                .MinimumLength(5).WithMessage("Password must be at least 5 characters long.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Password must contain at least one special character.");
            RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password).WithMessage("Passwords must match.");
        }
    }

    public class JwtTokenValidator : AbstractValidator<JwtToken>
    {
        public JwtTokenValidator()
        {
            RuleFor(token => token.Token).NotEmpty().WithMessage("Token is required.");
            RuleFor(token => token.ExpiryDate).NotEmpty().WithMessage("Expiry date is required.");
        }
    }

    public class UserFinancialDataValidator : AbstractValidator<UserFinancialData>
    {
        public UserFinancialDataValidator()
        {
            RuleFor(data => data.Age).InclusiveBetween(16, 100).WithMessage("Age must be between 16 and 100.");
            RuleFor(data => data.InitialInvestment).GreaterThanOrEqualTo(0).WithMessage("Initial investment must be greater than or equal to 0.");
        }
    }

}
