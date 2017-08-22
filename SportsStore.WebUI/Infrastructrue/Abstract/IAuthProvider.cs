
namespace SportsStore.WebUI.Infrastructrue.Abstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string passwrod);
    }
}