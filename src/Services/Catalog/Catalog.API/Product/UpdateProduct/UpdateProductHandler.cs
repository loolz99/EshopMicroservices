using Catalog.API.Exceptions;

namespace Catalog.API.Product.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price):ICommand<UpdateProductResponse>;

public record UpdateProductResult(bool isSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{

    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");
        RuleFor(command => command.Name).NotEmpty().WithMessage("Name is Required");
        RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        

    }

}


internal class UpdateProductCommandHandler(IDocumentSession session ,ILogger<UpdateProductCommandHandler> logger):ICommandHandler<UpdateProductCommand,UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductHandler.Handle Called with {@Command}",command);
        var product = await session.LoadAsync<Models.Product>(command.Id, cancellationToken);
        if (product is null)
            throw new ProductNotFoundException(command.Id);

        product.Name = command.Name;
        product.Description = command.Description;
        product.Category = command.Category;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResponse(true);
    }
}