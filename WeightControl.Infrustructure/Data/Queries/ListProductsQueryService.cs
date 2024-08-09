using Microsoft.EntityFrameworkCore;
using WeightControl.Application.Products;
using WeightControl.Application.Products.List;
using WeightControl.Infrustructure.Data;

namespace WeightControl.Infrustructure.Data.Queries;

public class ListProductsQueryService : IListProductsQueryService
{
    private readonly AppDbContext _db;

    public ListProductsQueryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductDTO>> ListAsync()
    {
        var result = await _db.Products.Select(p => new ProductDTO(p.Id, p.Name, p.Weight))
          .ToListAsync();

        return result;
    }
}
