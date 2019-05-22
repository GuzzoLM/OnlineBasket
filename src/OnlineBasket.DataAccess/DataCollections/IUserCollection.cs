namespace OnlineBasket.DataAccess.DataCollections
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Access;

    public interface IUserCollection
    {
        Task<List<User>> GetUsers();

        Task<bool> Add(User user);

        Task<bool> Add(IEnumerable<User> users);

        Task<bool> Delete(string username);
    }
}