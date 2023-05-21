using System.Threading.Tasks;
using Core.Network.Services;
using UnityEngine.SceneManagement;

namespace Core.Network.Managers
{
    public class AuthenticationManager
    {
        public LobbyOrchestrator LobbyOrchestrator { get; set; }
        
        public async Task LoginAnonymously()
        {
            await AuthenticationService.Login();
            SceneManager.LoadSceneAsync("Menu");
        }
    }
}