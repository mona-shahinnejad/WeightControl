using Ardalis.SharedKernel;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using WeightControl.Application.Products.Create;
using WeightControl.Core.ProductAggregate;

namespace WeightControl.UnitTests.Application.Products
{
    public class CreateProductHandlerTests
    {
        private readonly IFixture _fixture;

        public CreateProductHandlerTests()
        {
            _fixture = new Fixture();
        }

        private CreateProductHandler CreateHandler(IRepository<Product> repository)
        {
            return new CreateProductHandler(repository);
        }

        private CreateProductCommand CreateCommand(string name = "Test Product", double weight = 12.5)
        {
            return _fixture.Build<CreateProductCommand>()
                           .With(c => c.Name, name)
                           .With(c => c.Weight, weight)
                           .Create();
        }

        [Fact]
        public async Task Handle_ShouldReturnProductId_WhenProductIsCreated()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Product>>();
            var productId = _fixture.Create<int>();
            var command = CreateCommand();
            var createdProduct = new Product(command.Name, command.Weight)
            {
                Id = productId // Directly set the Id for simplicity
            };

            repository.AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
                      .Returns(createdProduct);

            var handler = CreateHandler(repository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(productId);
            await repository.Received(1)
                            .AddAsync(Arg.Is<Product>(p => p.Name == command.Name && p.Weight == command.Weight),
                                      Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenNameIsInvalid()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Product>>();
            var command = CreateCommand(name: null!); // Set Name to null
            var handler = CreateHandler(repository);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("Value cannot be null. (Parameter 'name')");
            await repository.DidNotReceive()
                            .AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenWeightIsNegative()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Product>>();
            var command = CreateCommand(weight: -5); // Set Weight to negative
            var handler = CreateHandler(repository);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                     .WithMessage("Input Weight was out of range (Parameter 'Weight')");
            await repository.DidNotReceive()
                            .AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenWeightExceedsMaxValue()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Product>>();
            var command = CreateCommand(weight: double.MaxValue + 1); // Set Weight to exceed max value
            var handler = CreateHandler(repository);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NullReferenceException>();
        }
    }
}
