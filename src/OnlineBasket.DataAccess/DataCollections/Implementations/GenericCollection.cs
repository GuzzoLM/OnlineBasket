namespace OnlineBasket.DataAccess.DataCollections.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Interfaces;

    public abstract class GenericCollection<T> : IGenericCollection<T> where T : IIdAware
    {
        private List<T> _items;

        public GenericCollection()
        {
            _items = new List<T>();
        }
        public Task<bool> Add(T user)
        {
            if (_items.Any(existent => existent.Id == user.Id))
                return Task.FromResult(false);

            _items.Add(user);
            return Task.FromResult(true);
        }

        public Task<bool> Add(IEnumerable<T> items)
        {
            if (items.Any(added => _items.Any(existent => existent.Id == added.Id)))
                return Task.FromResult(false);

            _items.AddRange(items);
            return Task.FromResult(true);
        }

        public Task<bool> Delete(Guid id)
        {
            var userToRemove = _items.FirstOrDefault(existent => existent.Id == id);

            if (userToRemove == null)
                return Task.FromResult(false);

            _items.Remove(userToRemove);
            return Task.FromResult(true);
        }

        public Task<List<T>> GetItems() => Task.FromResult(_items.ToList());
    }
}