namespace OnlineBasket.DataAccess.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineBasket.DataAccess.DataCollections;
    using OnlineBasket.Domain.Enums;
    using OnlineBasket.Domain.Model;

    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketCollection _basketCollection;

        public BasketRepository(IBasketCollection basketCollection)
        {
            _basketCollection = basketCollection;
        }

        public async Task<Guid> Create(Basket item)
        {
            var result = await _basketCollection.Add(item);

            if (result)
                return item.Id;

            throw new ArgumentException("Item already exists");
        }

        public async Task Delete(Guid id)
        {
            var result = await _basketCollection.Delete(id);

            if (!result)
                throw new KeyNotFoundException("Failed to delete item from database");
        }

        public async Task<List<Basket>> GetItems(Guid? ownerId = null, BasketStatus? status = null)
        {
            IEnumerable<Basket> items = await _basketCollection.Items();

            items = (ownerId.HasValue) ? items.Where(x => x.OwnerId == ownerId.Value) : items;
            items = (status.HasValue) ? items.Where(x => x.Status == status.Value) : items;

            return items.ToList();
        }

        public async Task<Basket> Get(Guid id)
        {
            IEnumerable<Basket> items = await _basketCollection.Items();

            var item = items.FirstOrDefault(x => x.Id == id);

            if (item == null)
                throw new KeyNotFoundException("Item not found");

            return item;
        }

        public async Task Update(Guid id, Basket item)
        {
            var result = await _basketCollection.Update(id, item);

            if (!result)
                throw new KeyNotFoundException("Item not found");
        }
    }
}