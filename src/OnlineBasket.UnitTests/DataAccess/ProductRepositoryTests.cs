namespace OnlineBasket.UnitTests.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using Moq;
    using OnlineBasket.DataAccess.DataCollections;
    using OnlineBasket.DataAccess.Services;
    using OnlineBasket.DataAccess.Services.Implementations;
    using OnlineBasket.Domain.Model;
    using Xunit;

    [Trait("UnitTest", nameof(ProductRepository))]
    public class ProductRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IProductCollection> _productCollection;
        private readonly IProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _fixture = new Fixture();
            _productCollection = new Mock<IProductCollection>();
            _productRepository = new ProductRepository(_productCollection.Object);
        }

        [Fact]
        public async Task Create_SuccessfulyCreated_ShouldReturnGuid()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Add(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _productRepository.Create(product);

            // Assert
            result.Should()
                .Be(product.Id);
        }

        [Fact]
        public void Create_FailedToCreate_ShouldThrow()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Add(It.IsAny<Product>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _productRepository.Create(product);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void Delete_SuccessfullyDeleted_ShouldNotThrow()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _productRepository.Delete(productId);

            // Assert
            act.Should()
                .NotThrow<Exception>();
        }

        [Fact]
        public void Delete_FailedToDelete_ShouldThrow()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _productRepository.Delete(productId);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void GetItems_FailedToFetch_ShouldThrow()
        {
            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult<List<Product>>(null));

            // Act
            Func<Task> act = async () => await _productRepository.GetItems();

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public async Task GetItems_SearchByName_ShouldReturnCorrectList()
        {
            // Arrange
            var name = "red";
            var unwantedName = "blue";

            var redProducts = _fixture
                .Build<Product>()
                .With(x => x.Name, name)
                .CreateMany(5);

            var randomProducts = _fixture
                .Build<Product>()
                .With(x => x.Name, unwantedName)
                .CreateMany(10);

            var expectedResult = redProducts.ToList();
            var totalItems = redProducts.Concat(randomProducts).ToList();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(totalItems));

            // Act
            var result = await _productRepository.GetItems(name: name);

            // Assert
            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetItems_SearchByPrice_ShouldReturnCorrectList()
        {
            // Arrange
            var price = 150;
            var unwantedPrice = 200;

            var redProducts = _fixture
                .Build<Product>()
                .With(x => x.Price, price)
                .CreateMany(5);

            var randomProducts = _fixture
                .Build<Product>()
                .With(x => x.Price, unwantedPrice)
                .CreateMany(10);

            var expectedResult = redProducts.ToList();
            var totalItems = redProducts.Concat(randomProducts).ToList();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(totalItems));

            // Act
            var result = await _productRepository.GetItems(price: price);

            // Assert
            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetItems_SearchByStock_ShouldReturnCorrectList()
        {
            // Arrange
            var stock = 13;
            var unwantedStock = 17;

            var redProducts = _fixture
                .Build<Product>()
                .With(x => x.Stock, stock)
                .CreateMany(5);

            var randomProducts = _fixture
                .Build<Product>()
                .With(x => x.Stock, unwantedStock)
                .CreateMany(10);

            var expectedResult = redProducts.ToList();
            var totalItems = redProducts.Concat(randomProducts).ToList();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(totalItems));

            // Act
            var result = await _productRepository.GetItems(stock: stock);

            // Assert
            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetItems_SearchByAllFilters_ShouldReturnCorrectList()
        {
            // Arrange
            var name = "red";
            var unwantedName = "blue";
            var price = 150;
            var unwantedPrice = 200;
            var stock = 13;
            var unwantedStock = 13;
            var redProducts = _fixture
                .Build<Product>()
                .With(x => x.Price, price)
                .With(x => x.Name, name)
                .With(x => x.Stock, stock)
                .CreateMany(1);

            var randomProducts = _fixture.Build<Product>()
                .With(x => x.Price, unwantedPrice)
                .With(x => x.Name, unwantedName)
                .With(x => x.Stock, unwantedStock)
                .CreateMany(1);

            var expectedResult = redProducts.ToList();
            var totalItems = redProducts.Concat(randomProducts).ToList();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(totalItems));

            // Act
            var result = await _productRepository.GetItems(stock: stock, price: price, name: name);

            // Assert
            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Get_FailedToFetch_ShouldThrow()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult<List<Product>>(null));

            // Act
            Func<Task> act = async () => await _productRepository.Get(productId);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void Get_ItemNotFound_ShouldThrow()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var searchedId = Guid.NewGuid();

            var products = _fixture
                .Build<Product>()
                .With(x => x.Id, productId)
                .CreateMany(1);

            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(products.ToList()));

            // Act
            Func<Task> act = async () => await _productRepository.Get(searchedId);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public async Task Get_ItemFound_ShouldReturnItem()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var product = _fixture
                .Build<Product>()
                .With(x => x.Id, productId)
                .Create();

            var products = _fixture.CreateMany<Product>(10).ToList();
            products.Add(product);

            // Arrange Mock
            _productCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(products));

            // Act
            var result = await _productRepository.Get(productId);

            // Assert
            result.Should()
                .BeEquivalentTo(product);
        }

        [Fact]
        public void Update_FailedToUpdate_ShouldThrow()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var product = _fixture
                .Build<Product>()
                .With(x => x.Id, productId)
                .Create();

            var products = _fixture.CreateMany<Product>(10).ToList();
            products.Add(product);

            // Arrange Mock
            _productCollection
                .Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Product>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _productRepository.Update(productId, product);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void Update_SuccessfullyUpdated_ShouldNotThrow()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var product = _fixture
                .Build<Product>()
                .With(x => x.Id, productId)
                .Create();

            var products = _fixture.CreateMany<Product>(10).ToList();
            products.Add(product);

            // Arrange Mock
            _productCollection
                .Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _productRepository.Update(productId, product);

            // Assert
            act.Should()
                .NotThrow<Exception>();
        }
    }
}