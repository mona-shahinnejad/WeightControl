using Ardalis.Result;
using Ardalis.SharedKernel;
using WeightControl.Application.Products;

namespace WeightControl.Application.Products.FilterByWeight;

public record FilterByWeightQuery(double MinWeight, double MaxWeight) : IQuery<Result<IEnumerable<ProductDTO>>>;
