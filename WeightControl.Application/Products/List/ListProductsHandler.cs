using Ardalis.Result;
using Ardalis.SharedKernel;
using WeightControl.Application.Products;

namespace WeightControl.Application.Products.List;

public class ListProductsHandler : IQueryHandler<ListProductsQuery, Result<IEnumerable<ProductDTO>>>
{
    private readonly IListProductsQueryService _query;

    public ListProductsHandler(IListProductsQueryService query)
    {
        _query = query;
    }

    public async Task<Result<IEnumerable<ProductDTO>>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _query.ListAsync();

        return Result.Success(result);
    }
}
