namespace OnlineBasket.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.DTO;
    using OnlineBasket.Domain.Enums;

    public interface IBasketClient
    {
        Task<BasketDTO> GetBasket(Guid id);

        Task<IEnumerable<BasketDTO>> GetBaskets(BasketStatus? status);

        Task<Guid> Post();

        Task Delete(Guid id);
    }
}