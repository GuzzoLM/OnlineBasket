namespace OnlineBasket.Security.Services.Implementations
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Tokens;
    using OnlineBasket.DataAccess.Services;
    using OnlineBasket.Domain.Access;
    using OnlineBasket.Security.Configurations;
    using OnlineBasket.Security.Model;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;

        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;

        public AuthenticationService(
            IUserService userService,
            TokenConfigurations tokenConfigurations,
            SigningConfigurations signingConfigurations)
        {
            _userService = userService;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
        }

        public async Task<AuthenticationResult> Authenticate(User user)
        {
            var existentUser = await _userService.FindUser(user.UserName);

            if (existentUser == null)
            {
                await _userService.AddUser(user);
                existentUser = await _userService.FindUser(user.UserName);
            }

            if (existentUser.Password == user.Password)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.UserName, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    }
                );

                DateTime creationDate = DateTime.Now;
                DateTime expirationDate = creationDate +
                    TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _tokenConfigurations.Issuer,
                    Audience = _tokenConfigurations.Audience,
                    SigningCredentials = _signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = creationDate,
                    Expires = expirationDate
                });
                var token = handler.WriteToken(securityToken);

                return new AuthenticationResult
                {
                    Authenticated = true,
                    Token = new AuthenticatedToken
                    {
                        access_token = token,
                        expires_in = _tokenConfigurations.Seconds.ToString(),
                        token_type = "bearer"
                    }
                };
            }

            return new AuthenticationResult
            {
                Authenticated = false
            };
        }
    }
}