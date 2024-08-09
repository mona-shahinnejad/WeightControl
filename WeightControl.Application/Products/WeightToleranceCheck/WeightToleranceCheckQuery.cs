using Ardalis.Result;
using Ardalis.SharedKernel;

namespace WeightControl.Application.Products.WeightToleranceCheck;

public record WeightToleranceCheckQuery(int ProductId, double MinWeight, double MaxWeight) : IQuery<Result<bool>>;
