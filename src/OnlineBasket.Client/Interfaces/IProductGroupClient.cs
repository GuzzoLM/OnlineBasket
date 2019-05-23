namespace OnlineBasket.Client.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.DTO;

    public interface IProductGroupClient
    {
        Task Put(Guid bid, ProductGroupDTO productGroup);

        Task Delete(Guid bid, Guid? id);
    }
}