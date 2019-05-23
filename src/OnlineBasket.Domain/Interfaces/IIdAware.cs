namespace OnlineBasket.Domain.Interfaces
{
    using System;

    /// <summary>
    /// Makes sure all Domain entities implement Id, in order to be able to use DataCollections
    /// </summary>
    public interface IIdAware
    {
        Guid Id { get; set; }
    }
}