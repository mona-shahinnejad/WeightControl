using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeightControl.Core.ProductAggregate;
using WeightControl.Infrustructure.Data;

namespace WeightControl.Console;

public static class SeedData
{
    public static readonly Product Product1 = new("TestProduct", 85);

    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
        {
            if (dbContext.Products.Any())
            {
                return;   // DB has been seeded
            }

            PopulateTestData(dbContext);
        }
    }
    public static void PopulateTestData(AppDbContext dbContext)
    {
        foreach (var item in dbContext.Products)
        {
            dbContext.Remove(item);
        }

        dbContext.SaveChanges();

        dbContext.Products.Add(Product1);

        dbContext.SaveChanges();
    }
}
