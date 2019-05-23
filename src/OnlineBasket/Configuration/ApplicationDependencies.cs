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
        /// Add applications services to container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IBasketRepository, BasketRepository>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            return services;
        }

        /// <summary>
        /// Add collection services that simulate database
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection RegisterDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IUserCollection, UserCollection>();
            services.AddSingleton<IProductCollection, ProductCollection>();
            services.AddSingleton<IBasketCollection, BasketCollection>();

            return services;
        }

        /// <summary>
        /// Register and configure authentication services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal static IServiceCollection RegisterAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            // This configuration will hold secret key and crendentials
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            // COnfiguration for token, eg, expiration time, issuer, etc.
            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            // Add JWT default schemas
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
                // Add a policy to check if user has a valid token
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }
    }
}