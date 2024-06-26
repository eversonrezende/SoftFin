using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Data;
using SoftFin.Api.Handlers;
using SoftFin.Api.Models;
using SoftFin.Core;
using SoftFin.Core.Handlers;

namespace SoftFin.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = builder
            .Configuration
            .GetConnectionString("DefaultConnection")
            ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(y => y.FullName);
        });
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();

        builder.Services.AddAuthorization();
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(x =>
        {
            x.UseSqlServer(Configuration.ConnectionString);
        });

        builder.Services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {

    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }
}
