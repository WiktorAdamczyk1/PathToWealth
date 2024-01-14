using FluentValidation;

namespace PathToWealthAPI.Extensions
{
    public static class FluentValidationServiceExtensions
    {
        public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UserLoginValidator>();
            services.AddValidatorsFromAssemblyContaining<UserRegistrationValidator>();
            services.AddValidatorsFromAssemblyContaining<RefreshTokenValidator>();
            services.AddValidatorsFromAssemblyContaining<UserFinancialDataValidator>();

            return services;
        }
    }
}
