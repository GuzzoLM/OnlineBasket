namespace OnlineBasket.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Model;

    public interface IProductRepository
    {
        Task<List<Product>> GetItems(string name = null, decimal? price = null, int? stock = null);

        Task<Product> Get(Guid id);

        Task<Guid> Create(Product item);

        Task Update(Guid id, Product item);

        Task Delete(Guid id);
    }
}