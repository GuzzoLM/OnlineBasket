namespace OnlineBasket.Domain.Access
{
    using System;
    using OnlineBasket.Domain.Interfaces;

    public class User : IIdAware
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}