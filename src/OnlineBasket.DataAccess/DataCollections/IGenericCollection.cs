namespace OnlineBasket.DataAccess.DataCollections
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Interfaces;

    public interface IGenericCollection<T> where T : IIdAware
    {
        Task<List<T>> GetItems();

        Task<bool> Add(T user);

        Task<bool> Add(IEnumerable<T> users);

        Task<bool> Delete(Guid id);
    }
}