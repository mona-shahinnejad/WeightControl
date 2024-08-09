using Ardalis.GuardClauses;
using Ardalis.Result;
using Ardalis.SharedKernel;
using WeightControl.Core.ProductAggregate;

namespace WeightControl.Application.Products.WeightToleranceCheck;

public class WeightToleranceCheckHandler : IQueryHandler<WeightToleranceCheckQuery, Result<bool>>
{
    private readonly IReadRepository<Product> _repository;
    private readonly IWeightToleranceCheckQueryService _query;

    public WeightToleranceCheckHandler(IWeightToleranceCheckQueryService query, IReadRepository<Product> repository)
    {
        _query = query;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(WeightToleranceCheckQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.OutOfRange(request.MinWeight, nameof(request.MinWeight), 0, double.MaxValue);
        Guard.Against.OutOfRange(request.MaxWeight, nameof(request.MaxWeight), 0, double.MaxValue);

        if (request.MinWeight > request.MaxWeight)
        {
            throw new ArgumentOutOfRangeException(nameof(request.MinWeight), $"{nameof(request.MinWeight)} can not greater than {nameof(request.MaxWeight)}");
        }

        var entity = await _repository.GetByIdAsync(request.ProductId);

        if (entity == null)
        {
            return Result.NotFound();
        }

        var result = await _query.WeightToleranceCheckAsync(request.ProductId, request.MinWeight, request.MaxWeight);

        return Result.Success(result);
    }
}
