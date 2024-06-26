using Microsoft.AspNetCore.Identity;
using SoftFin.Api.Common.Api;
using SoftFin.Api.Models;
using SoftFin.Core.Handlers;
using SoftFin.Core.Requests.Transactions;
using System.Security.Claims;

namespace SoftFin.Api.Endpoints.Identity;

public class LogoutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/logout", HandleAsync)
        .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
}
