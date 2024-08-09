using WeightControl.Application.Products;

namespace WeightControl.Application.Products.FilterByWeight;


public interface IFilterByWeightService
{
    Task<IEnumerable<ProductDTO>> FilterByWeightAsync(double minWeight, double maxWeight);
}
