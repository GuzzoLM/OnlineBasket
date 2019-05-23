namespace OnlineBasket.Domain.Model
{
    using System;
    using OnlineBasket.Domain.Interfaces;

    public class Product : IIdAware
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}