namespace OnlineBasket.DataAccess.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineBasket.DataAccess.DataCollections;
    using OnlineBasket.Domain.Model;

    public class ProductRepository : IProductRepository
    {
        private readonly IProductCollection _productCollection;

        public ProductRepository(IProductCollection productCollection)
        {
            _productCollection = productCollection;
        }

        public async Task<Guid> Create(Product item)
        {
            var result = await _productCollection.Add(item);

            if (result)
                return item.Id;

            throw new ArgumentException("Item already exists");
        }

        public async Task Delete(Guid id)
        {
            var result = await _productCollection.Delete(id);

            if (!result)
                throw new KeyNotFoundException("Failed to delete item from database");
        }

        public async Task<List<Product>> GetItems(string name = null, decimal? price = null, int? stock = null)
        {
            IEnumerable<Product> items = await _productCollection.Items();

            items = (!string.IsNullOrEmpty(name)) ? items.Where(x => x.Name == name) : items;
            items = (price != null) ? items.Where(x => x.Price == price) : items;
            items = (stock != null) ? items.Where(x => x.Stock == stock) : items;

            return items.ToList();
        }

        public async Task<Product> Get(Guid id)
        {
            IEnumerable<Product> items = await _productCollection.Items();

            var item = items.FirstOrDefault(x => x.Id == id);

            if (item == null)
                throw new KeyNotFoundException("Item not found");

            return item;
        }

        public async Task Update(Guid id, Product item)
        {
            var result = await _productCollection.Update(id, item);

            if (!result)
                throw new KeyNotFoundException("Item not found");
        }
    }
}