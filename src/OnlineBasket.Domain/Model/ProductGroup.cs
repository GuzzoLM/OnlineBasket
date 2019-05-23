namespace OnlineBasket.Domain.Model
{
    using System;

    public class ProductGroup
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;
    }
}