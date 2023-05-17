using Core.Network.Misc;
using Core.Network.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Network.Managers
{
    public class AuthenticationManager : MonoBehaviour
    {
        [SerializeField] private LobbyOrchestrator _lobbyOrchestrator;
        [SerializeField] private GameObject _loginScreen;
        
        public async void LoginAnonymously()
        {
            // using (new Load("Logging you in..."))
            {
                await AuthenticationService.Login();
                // SceneManager.LoadSceneAsync("Lobby");
                _loginScreen.SetActive(false);
                _lobbyOrchestrator.gameObject.SetActive(true);
            }
        }
    }
}