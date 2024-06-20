using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Data;
using SoftFin.Core.Models;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient<Handler>();
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

app.MapGet("/", () => "Hello World!");

app.MapPost(
    "/v1/categories",
    (Request request, Handler handler) =>
    handler.Handle(request))
    .WithName("Categories: Create")
    .WithSummary("Cria uma nova categoria")
    .Produces<Response>();

app.Run();
