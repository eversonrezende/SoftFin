using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Data;
using SoftFin.Api.Endpoints;
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

app.MapGet("/", () => new { message = "OK"});
app.MapEndpoints();

app.Run();
