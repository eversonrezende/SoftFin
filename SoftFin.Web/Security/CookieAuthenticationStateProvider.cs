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

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _isAuthenticated = false;

        // Criar um usuário, pois precisamos de algum objeto ClaimsPrincipal, senão irá gerar um erro no app
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        // Buscando o usuário de fato
        var userInfo = await GetUsers();

        // Se usuário nulo, retorna o user instanciado
        if (userInfo is null)
            return new AuthenticationState(user);

        // Caso contrário, buscamos os claims do usuário
        var claims = await GetClaims(userInfo);

        // Criar o ClaimsIdentity passando os claims do usuário e o tipo de autenticação
        var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));

        // Ai passamos ao user
        user = new ClaimsPrincipal(id);

        _isAuthenticated = true;

        return new AuthenticationState(user);
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
