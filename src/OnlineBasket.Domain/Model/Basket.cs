namespace OnlineBasket.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OnlineBasket.Domain.Enums;

    public class Basket
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public List<ProductGroup> Items { get; set; }

        public decimal TotalPrice => Items
            .Select(x => x.TotalPrice)
            .Aggregate((x, y) => x + y);

        public BasketStatus Status { get; set; }

        public IDictionary<Guid, int> ClearBasket()
        {
            var returnedItems = Items.ToDictionary(x => x.ProductId, x => x.Quantity);
            Items = new List<ProductGroup>();

            return returnedItems;
        }

        public Product AddItem(Product product, int quantity)
        {
            var finalStock = product.Stock - quantity;

            if (finalStock < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Not enougth items in stock");

            var productGroup = Items.FirstOrDefault(x => x.ProductId == product.Id);
            if (productGroup == null)
            {
                productGroup = new ProductGroup
                {
                    ProductId = product.Id,
                    UnitPrice = product.Price,
                    Quantity = 0
                };

                Items.Add(productGroup);
            }

            productGroup.Quantity += quantity;

            product.Stock = finalStock;

            return product;
        }

        public Product RemoveItem(Product product, int quantity)
        {
            var productGroup = Items.FirstOrDefault(x => x.ProductId == product.Id);

            if (productGroup == null)
                throw new KeyNotFoundException();

            var finalQuantity = productGroup.Quantity - quantity;

            if (finalQuantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Tryed to remove more items then what was in basket");

            if (finalQuantity == 0)
                Items.Remove(productGroup);
            else
                productGroup.Quantity = finalQuantity;

            product.Stock += quantity;

            return product;
        }
    }
}