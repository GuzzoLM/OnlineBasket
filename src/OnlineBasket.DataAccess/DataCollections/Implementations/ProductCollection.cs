using System;
using System.Collections.Generic;
using System.Text;
using OnlineBasket.Domain.Model;

namespace OnlineBasket.DataAccess.DataCollections.Implementations
{
    public class ProductCollection : GenericCollection<Product>, IProductCollection
    {
        public ProductCollection()
        {
            var items = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Red Shirt",
                    Price = 79.99M,
                    Stock = 15
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Blue Shirt",
                    Price = 99.99M,
                    Stock = 3
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "White Shirt",
                    Price = 59.99M,
                    Stock = 0
                }
            };

            this.Add(items);
        }
    }
}
