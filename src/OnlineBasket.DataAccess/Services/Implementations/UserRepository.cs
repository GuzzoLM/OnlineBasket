namespace OnlineBasket.DataAccess.Services.Implementations
{
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineBasket.DataAccess.DataCollections;
    using OnlineBasket.Domain.Access;

    public class UserRepository : IUserRepository
    {
        private readonly IUserCollection _userCollection;

        public UserRepository(IUserCollection userCollection)
        {
            _userCollection = userCollection;
        }

        public Task<bool> AddUser(User user)
        {
            return _userCollection.Add(user);
        }

        public async Task<User> FindUser(string userName)
        {
            var users = await _userCollection.Items();
            return users.FirstOrDefault(user => user.UserName == userName);
        }
    }
}