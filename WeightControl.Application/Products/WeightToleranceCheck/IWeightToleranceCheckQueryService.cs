namespace WeightControl.Application.Products.WeightToleranceCheck;


public interface IWeightToleranceCheckQueryService
{
    Task<bool> WeightToleranceCheckAsync(int productId, double minWeight, double maxWeight);
}
