using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using WeightControl.Application.Products.WeightToleranceCheck;
using WeightControl.Core.ProductAggregate;

namespace WeightControl.UnitTests.Application.Products
{
    public class WeightToleranceCheckHandlerTests
    {
        private readonly IFixture _fixture;

        public WeightToleranceCheckHandlerTests()
        {
            _fixture = new Fixture();
        }

        private WeightToleranceCheckHandler CreateHandler(IWeightToleranceCheckQueryService queryService, IReadRepository<Product> repository)
        {
            return new WeightToleranceCheckHandler(queryService, repository);
        }

        private WeightToleranceCheckQuery CreateValidQuery(double minWeight = 10.0, double maxWeight = 50.0)
        {
            // Ensure MinWeight <= MaxWeight
            if (minWeight > maxWeight)
            {
                (minWeight, maxWeight) = (maxWeight, minWeight);
            }

            return new WeightToleranceCheckQuery(
                ProductId: _fixture.Create<int>(),
                MinWeight: minWeight,
                MaxWeight: maxWeight
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductExistsAndWeightIsInRange()
        {
            // Arrange
            var queryService = Substitute.For<IWeightToleranceCheckQueryService>();
            var repository = Substitute.For<IReadRepository<Product>>();
            var query = CreateValidQuery(); // Ensure valid weight range
            var toleranceCheckResult = _fixture.Create<bool>();
            var product = _fixture.Create<Product>();

            repository.GetByIdAsync(query.ProductId)
                      .Returns(product);

            queryService.WeightToleranceCheckAsync(query.ProductId, query.MinWeight, query.MaxWeight)
                        .Returns(toleranceCheckResult);

            var handler = CreateHandler(queryService, repository);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(toleranceCheckResult);
            await repository.Received(1).GetByIdAsync(query.ProductId);
            await queryService.Received(1).WeightToleranceCheckAsync(query.ProductId, query.MinWeight, query.MaxWeight);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var queryService = Substitute.For<IWeightToleranceCheckQueryService>();
            var repository = Substitute.For<IReadRepository<Product>>();
            var query = CreateValidQuery(); // Ensure valid weight range

            repository.GetByIdAsync(query.ProductId)
                      .Returns((Product)null); // Product not found

            var handler = CreateHandler(queryService, repository);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.IsNotFound().Should().BeTrue();
            await repository.Received(1).GetByIdAsync(query.ProductId);
            await queryService.DidNotReceive().WeightToleranceCheckAsync(Arg.Any<int>(), Arg.Any<double>(), Arg.Any<double>());
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenMinWeightIsGreaterThanMaxWeight()
        {
            // Arrange
            var queryService = Substitute.For<IWeightToleranceCheckQueryService>();
            var repository = Substitute.For<IReadRepository<Product>>();
            var query = new WeightToleranceCheckQuery(
                ProductId: _fixture.Create<int>(),
                MinWeight: 100.0,
                MaxWeight: 50.0
            );

            repository.GetByIdAsync(query.ProductId)
                      .Returns(_fixture.Create<Product>());

            var handler = CreateHandler(queryService, repository);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                     .WithMessage("MinWeight can not greater than MaxWeight (Parameter 'MinWeight')");
            await repository.DidNotReceive().GetByIdAsync(query.ProductId);
            await queryService.DidNotReceive().WeightToleranceCheckAsync(Arg.Any<int>(), Arg.Any<double>(), Arg.Any<double>());
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenMinWeightIsNegative()
        {
            // Arrange
            var queryService = Substitute.For<IWeightToleranceCheckQueryService>();
            var repository = Substitute.For<IReadRepository<Product>>();
            var query = new WeightToleranceCheckQuery(
                ProductId: _fixture.Create<int>(),
                MinWeight: -5.0,
                MaxWeight: 50.0
            );

            repository.GetByIdAsync(query.ProductId)
                      .Returns(_fixture.Create<Product>());

            var handler = CreateHandler(queryService, repository);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                     .WithMessage($"Input {nameof(query.MinWeight)} was out of range (Parameter '{nameof(query.MinWeight)}')");
            await repository.DidNotReceive().GetByIdAsync(query.ProductId);
            await queryService.DidNotReceive().WeightToleranceCheckAsync(Arg.Any<int>(), Arg.Any<double>(), Arg.Any<double>());
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenMaxWeightIsNegative()
        {
            // Arrange
            var queryService = Substitute.For<IWeightToleranceCheckQueryService>();
            var repository = Substitute.For<IReadRepository<Product>>();
            var query = new WeightToleranceCheckQuery(
                ProductId: _fixture.Create<int>(),
                MinWeight: 10.0,
                MaxWeight: -5.0
            );

            repository.GetByIdAsync(query.ProductId)
                      .Returns(_fixture.Create<Product>());

            var handler = CreateHandler(queryService, repository);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                     .WithMessage($"Input {nameof(query.MaxWeight)} was out of range (Parameter '{nameof(query.MaxWeight)}')");
            await repository.DidNotReceive().GetByIdAsync(query.ProductId);
            await queryService.DidNotReceive().WeightToleranceCheckAsync(Arg.Any<int>(), Arg.Any<double>(), Arg.Any<double>());
        }
    }
}
