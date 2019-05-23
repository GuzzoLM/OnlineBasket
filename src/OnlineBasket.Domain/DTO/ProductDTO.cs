namespace OnlineBasket.Domain.DTO
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductDTO
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}