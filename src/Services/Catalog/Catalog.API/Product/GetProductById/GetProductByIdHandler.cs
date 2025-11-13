using Catalog.API.Exceptions;

namespace Catalog.API.Product.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Models.Product Product);


    internal class GetProductByIdHandler(IDocumentSession session,ILogger<GetProductByIdHandler> logger) : IQueryHandler<GetProductByIdQuery,GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken) {
            logger.LogInformation("GetProductsQueryHandler.Handle Called  with {@Query}",session);
            var product = await session.LoadAsync<Models.Product>(query.Id, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(query.Id);
            }

            return new GetProductByIdResult(product);
            
        }

       
    }
