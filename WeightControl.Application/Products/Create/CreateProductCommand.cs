using Ardalis.Result;

namespace WeightControl.Application.Products.Create;

/// <summary>
/// Create a new Product.
/// </summary>
/// <param name="Name"></param>
/// <param name="Weight"></param>
public record CreateProductCommand(string Name, double Weight) : Ardalis.SharedKernel.ICommand<Result<int>>;
