namespace OnlineBasket.Configuration
{
    using System;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using OnlineBasket.DataAccess.DataCollections;
    using OnlineBasket.DataAccess.DataCollections.Implementations;
    using OnlineBasket.DataAccess.Services;
    using OnlineBasket.DataAccess.Services.Implementations;
    using OnlineBasket.Security.Configurations;
    using OnlineBasket.Security.Services;
    using OnlineBasket.Security.Services.Implementations;

    internal static class ApplicationDependencies
    {
        /// <summary>
        /// Add application's dependencies to the container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserCollection, UserCollection>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            return services;
        }

        internal static IServiceCollection RegisterDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IUserCollection, UserCollection>();

            return services;
        }

        internal static IServiceCollection RegisterAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }
    }
}