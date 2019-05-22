namespace OnlineBasket.DataAccess.Services
{
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Access;

    public interface IUserService
    {
        Task<User> FindUser(string userName);

        Task<bool> AddUser(User user);
    }
}