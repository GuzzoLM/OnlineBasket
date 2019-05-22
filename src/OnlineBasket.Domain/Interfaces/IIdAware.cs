namespace OnlineBasket.Domain.Interfaces
{
    using System;

    public interface IIdAware
    {
        Guid Id { get; set; }
    }
}