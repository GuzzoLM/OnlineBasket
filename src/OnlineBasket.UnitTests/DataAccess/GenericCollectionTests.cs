namespace OnlineBasket.UnitTests.DataAccess
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using OnlineBasket.UnitTests.DataAccess.Stubs;
    using Xunit;

    [Trait("UnitTest", "GenericCollection")]
    public class GenericCollectionTests
    {
        private readonly Fixture _fixture;
        public GenericCollectionTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GenericCollection_AddItem_ShouldReturnTrue()
        {
            // Arrange
            var collection = new StubCollection();
            var item = _fixture.Create<StubModel>();
            var expectedResult = true;

            // Act
            var result = await collection.Add(item);

            // Assert
            result
                .Should()
                .Be(expectedResult);
        }

        [Fact]
        public async Task GenericCollection_AddItems_ShouldReturnTrue()
        {
            // Arrange
            var collection = new StubCollection();
            var items = _fixture.CreateMany<StubModel>(5);
            var expectedResult = true;

            // Act
            var result = await collection.Add(items);

            // Assert
            result
                .Should()
                .Be(expectedResult);
        }

        [Fact]
        public async Task GenericCollection_GetItems_ShouldReturnAddedItems()
        {
            // Arrange
            var collection = new StubCollection();
            var items = _fixture.CreateMany<StubModel>(5);
            await collection.Add(items);
            var expectedResult = items.ToList();

            // Act
            var result = await collection.Items();

            // Assert
            result
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GenericCollection_DeleteItem_ShouldReturnTrue()
        {
            // Arrange
            var collection = new StubCollection();
            var item = _fixture.Create<StubModel>();
            await collection.Add(item);
            var expectedResult = true;

            // Act
            var result = await collection.Delete(item.Id);

            // Assert
            result
                .Should()
                .Be(expectedResult);
        }
    }
}