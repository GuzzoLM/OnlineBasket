﻿namespace OnlineBasket.DataAccess.DataCollections.Implementations
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
        public Task<bool> Add(T item)
        {
            if (_items.Any(existent => existent.Id == item.Id))
                return Task.FromResult(false);

            _items.Add(item);
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
            var itemToRemove = _items.FirstOrDefault(existent => existent.Id == id);

            if (itemToRemove == null)
                return Task.FromResult(false);

            _items.Remove(itemToRemove);
            return Task.FromResult(true);
        }

        public Task<List<T>> Items() => Task.FromResult(_items.ToList());

        public Task<bool> Update(Guid id, T item)
        {
            var existentItem = _items.FirstOrDefault(x => x.Id == item.Id);

            if (existentItem == null)
                return Task.FromResult(false);

            _items.Remove(existentItem);
            _items.Add(item);

            return Task.FromResult(true);
        }
    }
}