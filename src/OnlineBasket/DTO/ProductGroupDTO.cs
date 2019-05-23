namespace OnlineBasket.DTO
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductGroupDTO
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}