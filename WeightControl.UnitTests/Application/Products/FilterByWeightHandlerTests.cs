using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using WeightControl.Application.Products;
using WeightControl.Application.Products.FilterByWeight;

namespace WeightControl.UnitTests.Application.Products;

public class FilterByWeightHandlerTests
{
    private readonly IFilterByWeightService _filterByWeightService;
    private readonly FilterByWeightHandler _handler;

    public FilterByWeightHandlerTests()
    {
        _filterByWeightService = Substitute.For<IFilterByWeightService>();
        _handler = new FilterByWeightHandler(_filterByWeightService);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnFilteredProducts_WhenWeightRangeIsValid(
        [Frozen] IFixture fixture,
        FilterByWeightQuery request,
        IEnumerable<ProductDTO> filteredProducts)
    {
        // Arrange
        request = fixture.Build<FilterByWeightQuery>()
                         .With(q => q.MinWeight, 10.0)
                         .With(q => q.MaxWeight, 50.0)
                         .Create();

        _filterByWeightService.FilterByWeightAsync(request.MinWeight, request.MaxWeight)
                              .Returns(filteredProducts);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(filteredProducts);

        await _filterByWeightService.Received(1)
                                    .FilterByWeightAsync(request.MinWeight, request.MaxWeight);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenMinWeightIsGreaterThanMaxWeight(
        [Frozen] IFixture fixture,
        FilterByWeightQuery request)
    {
        // Arrange
        request = fixture.Build<FilterByWeightQuery>()
                         .With(q => q.MinWeight, 100.0)
                         .With(q => q.MaxWeight, 50.0)
                         .Create();

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        await _filterByWeightService.DidNotReceive()
                                    .FilterByWeightAsync(Arg.Any<double>(), Arg.Any<double>());
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrowArgumentOutOfRangeException_WhenWeightIsOutOfRange(
        [Frozen] IFixture fixture,
        FilterByWeightQuery request)
    {
        // Arrange
        request = fixture.Build<FilterByWeightQuery>()
                         .With(q => q.MinWeight, -5.0)
                         .With(q => q.MaxWeight, 50.0)
                         .Create();

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        await _filterByWeightService.DidNotReceive()
                                    .FilterByWeightAsync(Arg.Any<double>(), Arg.Any<double>());
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoProductsMatchCriteria(
        [Frozen] IFixture fixture,
        FilterByWeightQuery request)
    {
        // Arrange
        request = fixture.Build<FilterByWeightQuery>()
                         .With(q => q.MinWeight, 10.0)
                         .With(q => q.MaxWeight, 50.0)
                         .Create();

        _filterByWeightService.FilterByWeightAsync(request.MinWeight, request.MaxWeight)
                              .Returns(Enumerable.Empty<ProductDTO>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        await _filterByWeightService.Received(1)
                                    .FilterByWeightAsync(request.MinWeight, request.MaxWeight);
    }
}
