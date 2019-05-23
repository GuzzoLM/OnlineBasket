namespace OnlineBasket.UnitTests.DataAccess.Stubs
{
    using System;
    using OnlineBasket.Domain.Interfaces;

    public class StubModel : IIdAware
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }
    }
}