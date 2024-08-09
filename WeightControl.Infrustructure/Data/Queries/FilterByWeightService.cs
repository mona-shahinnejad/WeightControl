using Microsoft.EntityFrameworkCore;
using WeightControl.Application.Products;
using WeightControl.Application.Products.FilterByWeight;
using WeightControl.Infrustructure.Data;

namespace WeightControl.Infrustructure.Data.Queries
{
    internal class FilterByWeightService : IFilterByWeightService
    {
        private readonly AppDbContext _db;

        public FilterByWeightService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProductDTO>> FilterByWeightAsync(double minWeight, double maxWeight)
        {
            var result = await _db.Products.Where(p => p.Weight >= minWeight && p.Weight <= maxWeight).Select(p => new ProductDTO(p.Id, p.Name, p.Weight))
              .ToListAsync();

            return result;
        }
    }
}
