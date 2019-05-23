namespace OnlineBasket.Client.Interfaces
{
    using System.Threading.Tasks;
    using OnlineBasket.Security.Model;

    public interface ILoginClient
    {
        Task<AuthenticatedToken> Login(string username, string password);
    }
}