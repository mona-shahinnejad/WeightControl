namespace WeightControl.Application.Products.List;


public interface IListProductsQueryService
{
    Task<IEnumerable<ProductDTO>> ListAsync();
}
