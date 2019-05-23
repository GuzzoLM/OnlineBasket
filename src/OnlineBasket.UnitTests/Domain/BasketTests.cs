namespace OnlineBasket.UnitTests.Domain
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using FluentAssertions;
    using OnlineBasket.Domain.Model;
    using Xunit;

    [Trait("UnitTest", nameof(Basket))]
    public class BasketTests
    {
        private readonly Fixture _fixture;

        public BasketTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ClearBasket_ListShouldBeEmpty_ShouldReturnedProductsQuantities()
        {
            // Arrange
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var productQuantity1 = 5;
            var productQuantity2 = 17;

            var productGroups = new List<ProductGroup>
            {
                new ProductGroup
                {
                    ProductId = productId1,
                    Quantity = productQuantity1,
                    UnitPrice = 100
                },
                new ProductGroup
                {
                    ProductId = productId2,
                    Quantity = productQuantity2,
                    UnitPrice = 100
                }
            };

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedReturnedProducts = new Dictionary<Guid, int>
            {
                { productId1, productQuantity1 },
                { productId2, productQuantity2 }
            };

            // Act
            var returnedProducts = basket.ClearBasket();

            // Assert
            basket.Items.Should()
                .BeEmpty();

            returnedProducts.Should()
                .BeEquivalentTo(expectedReturnedProducts);
        }

        [Fact]
        public void AddItem_ProductGroupDoesNotExist_ShouldCreateProductGroupAndAddItem()
        {
            // Arrange
            var productStock = 15;
            var addedQuantity = 3;

            var product = _fixture
                .Build<Product>()
                .With(x => x.Stock, productStock)
                .Create();

            var productGroups = new List<ProductGroup>();

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock - addedQuantity
            };

            var expectedItems = new List<ProductGroup>
            {
                new ProductGroup
                {
                    ProductId = product.Id,
                    UnitPrice = product.Price,
                    Quantity = addedQuantity
                }
            };

            // Act
            var returnedProduct = basket.AddItem(product, addedQuantity);

            // Assert
            basket.Items.Should()
                .BeEquivalentTo(expectedItems);

            returnedProduct.Should()
                .BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public void AddItem_ProductGroupExists_ShouldAddItem()
        {
            // Arrange
            var productStock = 15;
            var addedQuantity = 3;

            var product = _fixture
                .Build<Product>()
                .With(x => x.Stock, productStock)
                .Create();

            var productGroups = new List<ProductGroup>
            {
                new ProductGroup
                {
                    ProductId = product.Id,
                    Quantity = 5,
                    UnitPrice = product.Price
                }
            };

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock - addedQuantity
            };

            // Act
            var returnedProduct = basket.AddItem(product, addedQuantity);

            // Assert
            returnedProduct.Should()
                .BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public void AddItem_AddedQuantityGreaterThanStock_ShouldThrow()
        {
            // Arrange
            var productStock = 2;
            var addedQuantity = 3;

            var product = _fixture
                .Build<Product>()
                .With(x => x.Stock, productStock)
                .Create();

            var productGroups = new List<ProductGroup>
            {
                new ProductGroup
                {
                    ProductId = product.Id,
                    Quantity = 5,
                    UnitPrice = product.Price
                }
            };

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock - addedQuantity
            };

            // Act
            Action act = () => basket.AddItem(product, addedQuantity);

            // Assert
            act.Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void RemoveItem_ProductGroupExists_ShouldRemoveItem()
        {
            // Arrange
            var productStock = 15;
            var removedQuantity = 3;

            var product = _fixture
                .Build<Product>()
                .With(x => x.Stock, productStock)
                .Create();

            var productGroups = new List<ProductGroup>
            {
                new ProductGroup
                {
                    ProductId = product.Id,
                    Quantity = 5,
                    UnitPrice = product.Price
                }
            };

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock + removedQuantity
            };

            // Act
            var returnedProduct = basket.RemoveItem(product, removedQuantity);

            // Assert
            returnedProduct.Should()
                .BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public void RemoveItem_RemoveAllProductItems_ShouldRemoveItemAndProductGroup()
        {
            // Arrange
            var productStock = 15;
            var removedQuantity = 5;

            var product = _fixture
                .Build<Product>()
                .With(x => x.Stock, productStock)
                .Create();

            var productGroups = new List<ProductGroup>
            {
                new ProductGroup
                {
                    ProductId = product.Id,
                    Quantity = 5,
                    UnitPrice = product.Price
                }
            };

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock + removedQuantity
            };

            // Act
            var returnedProduct = basket.RemoveItem(product, removedQuantity);

            // Assert
            basket.Items.Should()
                .BeEmpty();

            returnedProduct.Should()
                .BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public void RemoveItem_RemoveMoreItemsThanProductGroupQuantity_ShouldThrow()
        {
            // Arrange
            var productStock = 15;
            var removedQuantity = 10;

            var product = _fixture
                .Build<Product>()
                .With(x => x.Stock, productStock)
                .Create();

            var productGroups = new List<ProductGroup>
            {
                new ProductGroup
                {
                    ProductId = product.Id,
                    Quantity = 5,
                    UnitPrice = product.Price
                }
            };

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock + removedQuantity
            };

            // Act
            Action act = () => basket.RemoveItem(product, removedQuantity);

            // Assert
            act.Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void RemoveItem_ProductGroupDoesNotExist_ShouldThrow()
        {
            // Arrange
            var productStock = 15;
            var removedQuantity = 10;

            var product = _fixture
                .Build<Product>()
                .With(x => x.Stock, productStock)
                .Create();

            var productGroups = new List<ProductGroup>();

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Items, productGroups)
                .Create();

            var expectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock + removedQuantity
            };

            // Act
            Action act = () => basket.RemoveItem(product, removedQuantity);

            // Assert
            act.Should()
                .Throw<KeyNotFoundException>();
        }
    }
}