using Ardalis.Result;
using Ardalis.SharedKernel;
using WeightControl.Core.ProductAggregate;

namespace WeightControl.Application.Products.Create;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Result<int>>
{
    private readonly IRepository<Product> _repository;

    public CreateProductHandler(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(CreateProductCommand request,
      CancellationToken cancellationToken)
    {
        var newProduct = new Product(request.Name, request.Weight);
        var createdItem = await _repository.AddAsync(newProduct, cancellationToken);

        return createdItem.Id;
    }
}
