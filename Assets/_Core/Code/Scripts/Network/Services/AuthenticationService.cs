using System.Threading.Tasks;
using Unity.Services.Core;

namespace Core.Network.Services
{
    public static class AuthenticationService
    {
        public static string PlayerId { get; private set; }

        public static async Task Login()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                var options = new InitializationOptions();
                await UnityServices.InitializeAsync(options);
            }

            if (!Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
            {
                await Unity.Services.Authentication.AuthenticationService.Instance.SignInAnonymouslyAsync();
                PlayerId = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;
            }
        }
    }
}