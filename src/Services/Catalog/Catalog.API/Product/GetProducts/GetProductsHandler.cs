using Marten.Pagination;

namespace Catalog.API.Product.GetProducts;


public record GetProductsQuery(int? PageNumber =1 ,int? PageSize =10) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Models.Product> Products);

internal class GetProductsHandler(IDocumentSession session,ILogger<GetProductsHandler> logger) : IQueryHandler<GetProductsQuery,GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken) {
        logger.LogInformation("GetProductsQueryHandler.Handle Called  with {@Query}",query);
        var products = await session.Query<Models.Product>()
            .ToPagedListAsync(query.PageNumber?? 1,query.PageSize??10,cancellationToken);

        return new GetProductsResult(products);
    }
}