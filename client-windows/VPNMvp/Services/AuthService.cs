using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace VPNMvp.Services
{
public class LoginResponse { public string Token { get; set; } = ""; }


public class AuthService : IAuthService
{
private readonly IHttpClientFactory _factory;


public AuthService(IHttpClientFactory factory)
{
_factory = factory;
}


public async Task<bool> LoginAsync(string email, string password)
{
var client = _factory.CreateClient("Api");
var resp = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
if (!resp.IsSuccessStatusCode) return false;
var body = await resp.Content.ReadFromJsonAsync<LoginResponse>();
// store token temporarily (not secure) - for demo we just ignore
return body != null && !string.IsNullOrEmpty(body.Token);
}
}
}