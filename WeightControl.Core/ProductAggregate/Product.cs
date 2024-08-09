using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace WeightControl.Core.ProductAggregate;

public class Product : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }
    public double Weight { get; private set; }

    public Product(string name, double weight)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Weight = Guard.Against.OutOfRange(weight, nameof(weight), 0, double.MaxValue);
    }

    public void UpdateName(string newName)
    {
        Name = Guard.Against.NullOrWhiteSpace(newName, nameof(newName));
    }

    public void UpdateWeight(double newWeight)
    {
        Weight = Guard.Against.OutOfRange(newWeight, nameof(newWeight), 0, double.MaxValue);
    }
}
