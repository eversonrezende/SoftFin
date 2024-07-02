using SoftFin.Core.Requests.Account;
using SoftFin.Core.Responses;

namespace SoftFin.Core.Handlers;

public interface IAccountHandler
{
    Task<Response<string>> LoginAsync(LoginRequest request);
    Task<Response<string>> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
}
