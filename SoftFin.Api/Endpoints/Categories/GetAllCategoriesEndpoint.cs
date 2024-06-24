using Microsoft.AspNetCore.Mvc;
using SoftFin.Api.Common.Api;
using SoftFin.Core;
using SoftFin.Core.Handlers;
using SoftFin.Core.Models;
using SoftFin.Core.Requests.Categories;
using SoftFin.Core.Responses;

namespace SoftFin.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/", HandleAsync)
        .WithName("Categories: Get All")
        .WithSummary("Recupera todas as categorias")
        .WithDescription("Recupera todas as categorias")
        .WithOrder(5)
        .Produces<PagedResponse<List<Category>?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize
        )
    {
        var request = new GetAllCategoriesRequest
        {
            UserId = "teste@teste.com",
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await handler.GetAllAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
