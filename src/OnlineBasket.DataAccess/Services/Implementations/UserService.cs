namespace OnlineBasket.DataAccess.Services.Implementations
{
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineBasket.DataAccess.DataCollections;
    using OnlineBasket.Domain.Access;

    public class UserService : IUserService
    {
        private readonly IUserCollection _userCollection;

        public UserService(IUserCollection userCollection)
        {
            _userCollection = userCollection;
        }

        public Task<bool> AddUser(User user)
        {
            return _userCollection.Add(user);
        }

        public async Task<User> FindUser(string userName)
        {
            var users = await _userCollection.GetUsers();
            return users.FirstOrDefault(user => user.UserName == userName);
        }
    }
}