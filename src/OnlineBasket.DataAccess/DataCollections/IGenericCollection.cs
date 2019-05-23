namespace OnlineBasket.DataAccess.DataCollections
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OnlineBasket.Domain.Interfaces;

    public interface IGenericCollection<T> where T : IIdAware
    {
        Task<List<T>> Items();

        Task<bool> Add(T item);

        Task<bool> Add(IEnumerable<T> items);

        Task<bool> Delete(Guid id);

        Task<bool> Update(Guid id, T item);
    }
}