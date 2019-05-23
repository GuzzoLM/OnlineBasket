using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model = OnlineBasket.Domain.Model;

namespace OnlineBasket.DTO.TypeAdapters
{
    public static class AdapterExtensions
    {
        public static Model.Product ToModel(this Product product)
        {
            return new Model.Product
            {
                Id = product.Id ?? Guid.NewGuid(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }
    }
}
