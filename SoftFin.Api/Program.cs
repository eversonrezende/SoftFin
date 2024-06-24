using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Data;
using SoftFin.Api.Handlers;
using SoftFin.Core.Handlers;
using SoftFin.Core.Models;
using SoftFin.Core.Requests.Categories;
using SoftFin.Core.Responses;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(y => y.FullName);
});

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(connStr);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost(
    "/v1/categories",
    async (
        CreateCategoryRequest request,
        ICategoryHandler handler) =>
            await handler.CreateAsync(request))
        .WithName("Categories: Create")
        .WithSummary("Cria uma nova categoria")
        .Produces<Response<Category?>>();

app.MapPut(
    "/v1/categories/{id}",
    async (
        long id,
        UpdateCategoryRequest request,
        ICategoryHandler handler) =>
            {
                request.Id = id;
                return await handler.UpdateAsync(request);
            }
        )
        .WithName("Categories: Update")
        .WithSummary("Atualiza uma categoria")
        .Produces<Response<Category?>>();

app.MapDelete(
    "/v1/categories/{id}",
    async (
        long id,
        ICategoryHandler handler) =>
            {
                var request = new DeleteCategoryRequest
                {
                    Id = id,
                    UserId = "UserID 02"

                };
                return await handler.DeleteAsync(request);
            }
        )
        .WithName("Categories: Delete")
        .WithSummary("Exclui uma categoria")
        .Produces<Response<Category?>>();

app.MapGet(
    "/v1/categories",
    async (
        ICategoryHandler handler) =>
    {
        var request = new GetAllCategoriesRequest
        {
            UserId = "teste@teste.com"

        };
        return await handler.GetAllAsync(request);
    }
        )
        .WithName("Categories: Get All")
        .WithSummary("Recupera todas as categorias de um usuário")
        .Produces<PagedResponse<List<Category>?>>();

app.MapGet(
    "/v1/categories/{id}",
    async (
        long id,
        ICategoryHandler handler) =>
    {
        var request = new GetCategoryByIdRequest
        {
            Id = id,
            UserId = "teste@teste.com"

        };
        return await handler.GetByIdAsync(request);
    }
        )
        .WithName("Categories: Get By Id")
        .WithSummary("Recupera uma categoria")
        .Produces<Response<Category?>>();

app.Run();
