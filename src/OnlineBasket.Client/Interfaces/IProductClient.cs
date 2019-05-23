namespace OnlineBasket.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.DTO;

    public interface IProductClient
    {
        Task<IEnumerable<ProductDTO>> GetProducts(string name, decimal? price, int? stock);

        Task<ProductDTO> GetProduct(Guid id);

        Task<Guid> Post(ProductDTO product);

        Task Put(Guid id, ProductDTO product);

        Task Delete(Guid id);
    }
}