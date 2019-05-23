﻿namespace OnlineBasket.DTO
{
    using System;

    public class ProductGroupDTO
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}