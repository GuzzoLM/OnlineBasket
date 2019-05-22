namespace OnlineBasket.Security.Services
{
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Access;
    using OnlineBasket.Security.Model;

    public interface IAuthenticationService
    {
        Task<AuthenticationResult> Authenticate(User user);
    }
}