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

    // JwtTokenValidator is removed as JwtToken model is no longer used.
    public class RefreshTokenValidator : AbstractValidator<RefreshToken>
    {
        public RefreshTokenValidator()
        {
            RuleFor(token => token.Token)
                .NotEmpty().WithMessage("The token must not be empty.");

            RuleFor(token => token.Expires)
                .GreaterThan(DateTime.UtcNow).WithMessage("The token expiry date must be in the future.");

            RuleFor(token => token.Created)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("The token creation date must be in the past or present.");

            RuleFor(token => token.UserId)
                .NotEmpty().WithMessage("The user ID must not be empty.");
        }
    }

    public class UserFinancialDataValidator : AbstractValidator<UserFinancialData>
    {
        public UserFinancialDataValidator()
        {
            RuleFor(data => data.InitialInvestment).GreaterThanOrEqualTo(0).WithMessage("Initial investment must be greater than or equal to 0.");
            RuleFor(data => data.StartInvestmentYear).InclusiveBetween(2000, 2200).WithMessage("Start investment year must be between 2000 and 2200.");
            RuleFor(data => data.StartWithdrawalYear).GreaterThan(data => data.StartInvestmentYear).WithMessage("Withdrawal start year must be greater than start investment year.")
                .InclusiveBetween(2001, 2230).WithMessage("Withdrawal start year must be between 2001 and 2230.");
            RuleFor(data => data.YearlyOrMonthlySavings).GreaterThanOrEqualTo(0).WithMessage("Yearly or monthly savings must be greater than or equal to 0.");
            RuleFor(data => data.StockAnnualReturn).InclusiveBetween(0.01M, 200M).WithMessage("Stock annual return must be between 0.01 and 200.");
            RuleFor(data => data.StockCostRatio).InclusiveBetween(0.01M, 100M).WithMessage("Stock cost ratio must be between 0.01 and 100.");
            RuleFor(data => data.BondAnnualReturn).InclusiveBetween(0.01M, 200M).WithMessage("Bond annual return must be between 0.01 and 200.");
            RuleFor(data => data.BondCostRatio).InclusiveBetween(0.01M, 100M).WithMessage("Bond cost ratio must be between 0.01 and 100.");
            RuleFor(data => data.StockToBondRatio).InclusiveBetween(0, 100).WithMessage("Stock to bond ratio must be between 0 and 100.");
            RuleFor(data => data.RetirementDuration).InclusiveBetween(1, 100).WithMessage("Retirement duration must be between 1 and 100 years.");
            RuleFor(data => data.InflationRate).InclusiveBetween(0.01M, 100M).WithMessage("Inflation rate must be between 0.01 and 100.");
            RuleFor(data => data.WithdrawalRate).InclusiveBetween(0.01M, 100M).WithMessage("Withdrawal rate must be between 0.01 and 100.");
        }
    }

    public class ChangePasswordValidator : AbstractValidator<UserPasswordChange>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required.")
                .MinimumLength(5).WithMessage("New password must be at least 5 characters long.")
                .Matches(@"[A-Z]+").WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("New password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("New password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("New password must contain at least one special character.");
        }
    }
}
