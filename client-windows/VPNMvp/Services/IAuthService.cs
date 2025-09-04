using System.Threading.Tasks;


namespace VPNMvp.Services
{
public interface IAuthService
{
Task<bool> LoginAsync(string email, string password);
}
}