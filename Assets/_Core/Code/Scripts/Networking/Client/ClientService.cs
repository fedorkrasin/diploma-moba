using System;
using System.Threading.Tasks;
using Core.Networking.Client.Services;
using Core.Networking.Data;
using Core.Networking.Data.Enums;
using Core.Networking.Server;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Networking.Client
{
    public class ClientService : IDisposable
    {
        public event Action<Matchplayer> MatchPlayerSpawned = delegate { };
        public event Action<Matchplayer> MatchPlayerDespawned = delegate { };
        public MatchplayUser User { get; private set; }
        public NetworkClient NetworkClient { get; private set; }

        public bool Initialized { get; private set; } = false;

        public ClientService()
        {
            User = new MatchplayUser();
        }
        
        public void Dispose()
        {
            NetworkClient?.Dispose();
        }

        public async Task InitAsync()
        {
            await UnityServices.InitializeAsync();

            NetworkClient = new NetworkClient();
            var authenticationResult = await AuthenticationWrapper.DoAuth();

            if (authenticationResult == AuthenticationState.Authenticated)
            {
                User.AuthId = AuthenticationWrapper.PlayerID();
            }
            else
            {
                User.AuthId = Guid.NewGuid().ToString();
            }

            Debug.Log($"did Auth?{authenticationResult} {User.AuthId}");
            Initialized = true;
        }

        public void BeginConnection(string ip, int port)
        {
            Debug.Log($"Starting networkClient @ {ip}:{port}\nWith : {User}");
            NetworkClient.StartClient(ip, port);
        }

        public async Task<JoinAllocation> BeginConnection(string joinCode)
        {
            Debug.Log($"Starting networkClient with join code {joinCode}\nWith : {User}");
            return await NetworkClient.StartClient(joinCode);
        }

        public void Disconnect()
        {
            NetworkClient.DisconnectClient();
        }

        public async Task CancelMatchmaking()
        {
            // await Matchmaker.CancelMatchmaking();
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void AddMatchPlayer(Matchplayer player)
        {
            MatchPlayerSpawned?.Invoke(player);
        }

        public void RemoveMatchPlayer(Matchplayer player)
        {
            MatchPlayerDespawned?.Invoke(player);
        }

        public void SetGameQueue(GameQueue queue)
        {
            User.QueuePreference = queue;
        }

        public void ExitGame()
        {
            Dispose();
            Application.Quit();
        }
    }
}