using Catalog.API.Exceptions;
using Catalog.API.Product.GetProductById;

namespace Catalog.API.Product.GetProductByCategory;


public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Models.Product> Products);


internal class GetProductByCategoryQueryHandler(IDocumentSession session,ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery,GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken) {
        logger.LogInformation("GetProductByCategoryQueryHandler.Handle Called  with {@Query}",query);
        var products = await session.Query<Models.Product>().Where(p => p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);
        

        return new GetProductByCategoryResult(products);
            
    }
}

