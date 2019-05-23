namespace OnlineBasket.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Enums;
    using OnlineBasket.Domain.Model;

    public interface IBasketRepository
    {
        Task<List<Basket>> GetItems(Guid? ownerId = null, BasketStatus? status = null);

        Task<Basket> Get(Guid id);

        Task<Guid> Create(Basket item);

        Task Update(Guid id, Basket item);

        Task Delete(Guid ownerId, Guid id);
    }
}