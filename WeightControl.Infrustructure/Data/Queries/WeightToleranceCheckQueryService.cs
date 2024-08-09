using Microsoft.EntityFrameworkCore;
using WeightControl.Application.Products.WeightToleranceCheck;

namespace WeightControl.Infrustructure.Data.Queries;

public class WeightToleranceCheckQueryService : IWeightToleranceCheckQueryService
{
    private readonly AppDbContext _db;

    public WeightToleranceCheckQueryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> WeightToleranceCheckAsync(int productId, double minWeight, double maxWeight)
    {
        var result = await _db.Products.AnyAsync(p => p.Id == productId && p.Weight >= minWeight && p.Weight <= maxWeight);

        return result;
    }
}
