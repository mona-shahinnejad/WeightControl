using Ardalis.Result;
using Ardalis.SharedKernel;
using WeightControl.Application.Products;

namespace WeightControl.Application.Products.List;

public record ListProductsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ProductDTO>>>;
