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
    using OnlineBasket.Domain.Enums;
    using OnlineBasket.Domain.Model;
    using Xunit;

    [Trait("UnitTest", nameof(BasketRepository))]
    public class BasketRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IBasketCollection> _basketCollection;
        private readonly IBasketRepository _basketRepository;

        public BasketRepositoryTests()
        {
            _fixture = new Fixture();
            _basketCollection = new Mock<IBasketCollection>();
            _basketRepository = new BasketRepository(_basketCollection.Object);
        }

        [Fact]
        public async Task Create_SuccessfulyCreated_ShouldReturnGuid()
        {
            // Arrange
            var basket = _fixture.Create<Basket>();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Add(It.IsAny<Basket>()))
                .ReturnsAsync(true);

            // Act
            var result = await _basketRepository.Create(basket);

            // Assert
            result.Should()
                .Be(basket.Id);
        }

        [Fact]
        public void Create_FailedToCreate_ShouldThrow()
        {
            // Arrange
            var basket = _fixture.Create<Basket>();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Add(It.IsAny<Basket>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _basketRepository.Create(basket);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void Delete_SuccessfullyDeleted_ShouldNotThrow()
        {
            // Arrange
            var basketId = Guid.NewGuid();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _basketRepository.Delete(basketId);

            // Assert
            act.Should()
                .NotThrow<Exception>();
        }

        [Fact]
        public void Delete_FailedToDelete_ShouldThrow()
        {
            // Arrange
            var basketId = Guid.NewGuid();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _basketRepository.Delete(basketId);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void GetItems_FailedToFetch_ShouldThrow()
        {
            // Arrange Mock
            _basketCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult<List<Basket>>(null));

            // Act
            Func<Task> act = async () => await _basketRepository.GetItems();

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public async Task GetItems_SearchByPrice_ShouldReturnCorrectList()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unwantedId = Guid.NewGuid();

            var redProducts = _fixture
                .Build<Basket>()
                .With(x => x.OwnerId, id)
                .CreateMany(5);

            var randomProducts = _fixture
                .Build<Basket>()
                .With(x => x.OwnerId, unwantedId)
                .CreateMany(10);

            var expectedResult = redProducts.ToList();
            var totalItems = redProducts.Concat(randomProducts).ToList();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(totalItems));

            // Act
            var result = await _basketRepository.GetItems(ownerId: id);

            // Assert
            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetItems_SearchByStock_ShouldReturnCorrectList()
        {
            // Arrange
            var status = BasketStatus.Open;
            var StatusStock = BasketStatus.Close;

            var redProducts = _fixture
                .Build<Basket>()
                .With(x => x.Status, status)
                .CreateMany(5);

            var randomProducts = _fixture
                .Build<Basket>()
                .With(x => x.Status, StatusStock)
                .CreateMany(10);

            var expectedResult = redProducts.ToList();
            var totalItems = redProducts.Concat(randomProducts).ToList();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(totalItems));

            // Act
            var result = await _basketRepository.GetItems(status: status);

            // Assert
            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetItems_SearchByAllFilters_ShouldReturnCorrectList()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unwantedId = Guid.NewGuid();
            var status = BasketStatus.Open;
            var unwantedStatus = BasketStatus.Sold;
            var baskets = _fixture
                .Build<Basket>()
                .With(x => x.OwnerId, id)
                .With(x => x.Status, status)
                .CreateMany(1);

            var randomBaskets = _fixture.Build<Basket>()
                .With(x => x.OwnerId, unwantedId)
                .With(x => x.Status, unwantedStatus)
                .CreateMany(1);

            var expectedResult = baskets.ToList();
            var totalItems = baskets.Concat(randomBaskets).ToList();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(totalItems));

            // Act
            var result = await _basketRepository.GetItems(status: status, ownerId: id);

            // Assert
            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Get_FailedToFetch_ShouldThrow()
        {
            // Arrange
            var basketId = Guid.NewGuid();

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult<List<Basket>>(null));

            // Act
            Func<Task> act = async () => await _basketRepository.Get(basketId);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void Get_ItemNotFound_ShouldThrow()
        {
            // Arrange
            var basketId = Guid.NewGuid();
            var searchedId = Guid.NewGuid();

            var baskets = _fixture
                .Build<Basket>()
                .With(x => x.Id, basketId)
                .CreateMany(1);

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(baskets.ToList()));

            // Act
            Func<Task> act = async () => await _basketRepository.Get(searchedId);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public async Task Get_ItemFound_ShouldReturnItem()
        {
            // Arrange
            var basketId = Guid.NewGuid();

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Id, basketId)
                .Create();

            var baskets = _fixture.CreateMany<Basket>(10).ToList();
            baskets.Add(basket);

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Items())
                .Returns(Task.FromResult(baskets));

            // Act
            var result = await _basketRepository.Get(basketId);

            // Assert
            result.Should()
                .BeEquivalentTo(basket);
        }

        [Fact]
        public void Update_FailedToUpdate_ShouldThrow()
        {
            // Arrange
            var basketId = Guid.NewGuid();

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Id, basketId)
                .Create();

            var baskets = _fixture.CreateMany<Basket>(10).ToList();
            baskets.Add(basket);

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Basket>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _basketRepository.Update(basketId, basket);

            // Assert
            act.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void Update_SuccessfullyUpdated_ShouldNotThrow()
        {
            // Arrange
            var basketId = Guid.NewGuid();

            var basket = _fixture
                .Build<Basket>()
                .With(x => x.Id, basketId)
                .Create();

            var baskets = _fixture.CreateMany<Basket>(10).ToList();
            baskets.Add(basket);

            // Arrange Mock
            _basketCollection
                .Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Basket>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _basketRepository.Update(basketId, basket);

            // Assert
            act.Should()
                .NotThrow<Exception>();
        }
    }
}