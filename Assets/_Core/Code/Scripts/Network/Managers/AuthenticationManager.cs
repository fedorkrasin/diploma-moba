using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Network.Data;
using Core.Network.Services;
using UnityEngine.SceneManagement;

namespace Core.Network.Managers
{
    public class AuthenticationManager
    {
        public LobbyOrchestrator LobbyOrchestrator { get; set; }
        public Dictionary<ulong, LobbyPlayer> PlayersInLobby { get; set; } // TODO: return to LobbyOrchestrator
        
        public async Task LoginAnonymously()
        {
            await AuthenticationService.Login();
            SceneManager.LoadSceneAsync("Menu");
        }
    }
}