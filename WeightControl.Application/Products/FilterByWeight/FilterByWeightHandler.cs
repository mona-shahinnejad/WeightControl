using Ardalis.GuardClauses;
using Ardalis.Result;
using Ardalis.SharedKernel;
using WeightControl.Application.Products;

namespace WeightControl.Application.Products.FilterByWeight;

public class FilterByWeightHandler : IQueryHandler<FilterByWeightQuery, Result<IEnumerable<ProductDTO>>>
{
    private readonly IFilterByWeightService _query;

    public FilterByWeightHandler(IFilterByWeightService query)
    {
        _query = query;
    }

    public async Task<Result<IEnumerable<ProductDTO>>> Handle(FilterByWeightQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.OutOfRange(request.MinWeight, nameof(request.MinWeight), 0, double.MaxValue);
        Guard.Against.OutOfRange(request.MaxWeight, nameof(request.MaxWeight), 0, double.MaxValue);
        
        if (request.MinWeight > request.MaxWeight)
        {
            throw new ArgumentOutOfRangeException($"{nameof(request.MinWeight)} can not greater than {nameof(request.MaxWeight)}");
        }

        var result = await _query.FilterByWeightAsync(request.MinWeight, request.MaxWeight);

        return Result.Success(result);
    }
}
