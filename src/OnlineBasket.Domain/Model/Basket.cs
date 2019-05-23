namespace OnlineBasket.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OnlineBasket.Domain.Enums;
    using OnlineBasket.Domain.Interfaces;

    public class Basket : IIdAware
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public List<ProductGroup> Items { get; set; }

        public BasketStatus Status { get; set; }

        public decimal TotalPrice => Items
            .Select(x => x.TotalPrice)
            .Aggregate(0M, (x, y) => x + y);

        public Basket()
        {
        }

        public Basket(Guid ownerId)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Items = new List<ProductGroup>();
            Status = BasketStatus.Open;
        }

        public IDictionary<Guid, int> ClearBasket()
        {
            var returnedItems = Items.ToDictionary(x => x.ProductId, x => x.Quantity);
            Items = new List<ProductGroup>();

            return returnedItems;
        }

        public Product UpsertItem(Product product, int quantity)
        {
            var existentQuantity = Items.FirstOrDefault(x => x.ProductId == product.Id)?.Quantity ?? 0;

            var quantityToRemove = existentQuantity - quantity;
            var quantityToAdd = quantityToRemove * -1;

            return (quantityToRemove < 0) ? AddItem(product, quantityToAdd) : RemoveItem(product, quantityToRemove);
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

        public Product CompletelyRemoveItem(Product product)
        {
            var productGroup = Items.FirstOrDefault(x => x.ProductId == product.Id);

            if (productGroup == null)
                throw new KeyNotFoundException();

            product.Stock += productGroup.Quantity;

            Items.Remove(productGroup);

            return product;
        }
    }
}