using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Data;
using SoftFin.Api.Endpoints;
using SoftFin.Api.Handlers;
using SoftFin.Api.Models;
using SoftFin.Core.Handlers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(y => y.FullName);
});

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(connStr);
});

builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { message = "OK" });
app.MapEndpoints();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapPost("/logout", async (SignInManager<User> signInManager) => 
    { 
        await signInManager.SignOutAsync();
        return Results.Ok();
    })
    .RequireAuthorization();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapGet("/roles", (ClaimsPrincipal user) =>
    {
        if(user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();

        var identity = user.Identity as ClaimsIdentity;
        var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType
            });

        return TypedResults.Json(roles);
    })
    .RequireAuthorization();

app.Run();
