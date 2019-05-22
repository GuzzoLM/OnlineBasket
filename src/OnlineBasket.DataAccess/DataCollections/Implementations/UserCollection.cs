namespace OnlineBasket.DataAccess.DataCollections.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Access;

    public class UserCollection : IUserCollection
    {
        private List<User> _users;

        public UserCollection()
        {
            _users = new List<User>();
        }

        public Task<bool> Add(User user)
        {
            if (_users.Any(existent => existent.UserName == user.UserName))
                return Task.FromResult(false);

            _users.Add(user);
            return Task.FromResult(true);
        }

        public Task<bool> Add(IEnumerable<User> users)
        {
            if (users.Any(added => _users.Any(existent => existent.UserName == added.UserName)))
                return Task.FromResult(false);

            _users.AddRange(users);
            return Task.FromResult(true);
        }

        public Task<bool> Delete(string username)
        {
            var userToRemove = _users.FirstOrDefault(existent => existent.UserName == username);

            if (userToRemove == null)
                return Task.FromResult(false);

            _users.Remove(userToRemove);
            return Task.FromResult(true);
        }

        public Task<List<User>> GetUsers() => Task.FromResult(_users.ToList());
    }
}