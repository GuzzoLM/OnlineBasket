namespace OnlineBasket.Domain.DTO
{
    using System;
    using System.Collections.Generic;
    using OnlineBasket.Domain.Enums;

    public class BasketDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public BasketStatus Status { get; set; }

        public List<ProductGroupDTO> Items { get; set; }

        public decimal TotalPrice { get; set; }
    }
}