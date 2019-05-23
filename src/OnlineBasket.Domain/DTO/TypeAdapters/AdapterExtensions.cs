namespace OnlineBasket.Domain.DTO.TypeAdapters
{
    using System;
    using System.Linq;
    using OnlineBasket.Domain.Model;

    public static class AdapterExtensions
    {
        #region DTO to Model

        public static Product ToModel(this ProductDTO product)
        {
            return new Product
            {
                Id = product.Id ?? Guid.NewGuid(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public static ProductGroup ToModel(this ProductGroupDTO productGroup)
        {
            return new ProductGroup
            {
                ProductId = productGroup.ProductId,
                Quantity = productGroup.Quantity,
                UnitPrice = productGroup.UnitPrice ?? 0
            };
        }

        public static Basket ToModel(this BasketDTO basket, Guid ownerId)
        {
            return new Basket
            {
                Id = basket.Id,
                Items = basket.Items.Select(ToModel).ToList(),
                OwnerId = ownerId,
                Status = basket.Status
            };
        }

        #endregion DTO to Model

        #region Model to DTO

        public static ProductDTO ToDTO(this Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public static ProductGroupDTO ToDTO(this ProductGroup productGroup)
        {
            return new ProductGroupDTO
            {
                ProductId = productGroup.ProductId,
                Quantity = productGroup.Quantity,
                UnitPrice = productGroup.UnitPrice,
                TotalPrice = productGroup.TotalPrice
            };
        }

        public static BasketDTO ToDTO(this Basket basket, string username)
        {
            return new BasketDTO
            {
                Id = basket.Id,
                Items = basket.Items.Select(ToDTO).ToList(),
                UserName = username,
                Status = basket.Status,
                TotalPrice = basket.TotalPrice
            };
        }

        #endregion Model to DTO
    }
}