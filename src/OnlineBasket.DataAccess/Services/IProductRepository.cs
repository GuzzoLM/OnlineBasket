namespace OnlineBasket.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Model;

    public interface IProductRepository
    {
        Task<List<Product>> Get(string name, decimal price, int stock);

        Task<Product> Get(Guid id);

        Task<Guid> Create(Product item);

        Task<bool> Update(Guid id, Product item);

        Task<bool> Delete(Guid id);
    }
}