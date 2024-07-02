using Microsoft.AspNetCore.Components.Authorization;
using SoftFin.Core.Models.Account;
using System.Net.Http.Json;
using System.Security.Claims;

namespace SoftFin.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    : AuthenticationStateProvider, ICookieAuthenticationStateProvider
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    private bool _isAuthenticated = false;

    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _isAuthenticated;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<User?> GetUsers()
    {
        try
        {
            return await _client.GetFromJsonAsync<User?>("v1/identity/manage/info");
        }
        catch
        {
            return null;
        }
    }

    private async Task<List<Claim>> GetClaims(User user)
    {

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(
            user.Claims.Where(x =>
            x.Key != ClaimTypes.Name &&
            x.Key != ClaimTypes.Email)
            .Select(x => new Claim(x.Key, x.Value)));

        RoleClaim[]? roles;

        try
        {
            roles = await _client.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
        }
        catch
        {
            return claims;
        }

        claims.AddRange(from role in roles ?? []
                        where !string.IsNullOrEmpty(role.Type)
                        && !string.IsNullOrEmpty(role.Value)
                        select new
                        Claim(role.Type, role.Value, role.ValueType, role.Issuer,
                        role.OriginalIssuer));

        return claims;
    }

    public void NotifyAuthenticationStateChanged()
    => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}
