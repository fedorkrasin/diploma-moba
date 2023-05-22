using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Bootstrap.Commands;
using Core.Network.Data;
using Core.Network.Services;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Zenject;

namespace Core.Network.Managers
{
    public class LobbyOrchestrator : NetworkBehaviour
    {
        private AuthenticationManager _authenticationManager;
        private IStartMatchCommand _startMatchCommand;
        private float _nextLobbyUpdate;
        
        public event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated = delegate { };
        public event Action GameStarted = delegate { };

        public Dictionary<ulong, bool> PlayersInLobby { get; private set; }
        
        [Inject]
        public void Construct(
            AuthenticationManager authenticationManager,
            IStartMatchCommand startMatchCommand)
        {
            _authenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
            _startMatchCommand = startMatchCommand ?? throw new ArgumentNullException(nameof(startMatchCommand));
        }
        
        private void OnEnable()
        {
            _authenticationManager.LobbyOrchestrator = this;
            PlayersInLobby = new Dictionary<ulong, bool>();
            NetworkObject.DestroyWithScene = true;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
                PlayersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
                OnLobbyPlayersUpdated();
            }
        
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }
        }
        
        public async Task<bool> CreateLobby(LobbyData data)
        {
            // using (new Load("Creating Lobby..."))
            {
                try
                {
                    await MatchmakingService.CreateLobbyWithAllocation(data);
                    NetworkManager.Singleton.StartHost();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }
        }
        
        public async Task<bool> SelectLobby(Lobby lobby)
        {
            // using (new Load("Joining Lobby..."))
            {
                try
                {
                    await MatchmakingService.JoinLobbyWithAllocation(lobby.Id);
                    NetworkManager.Singleton.StartClient();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }
        }
        
        public async void LeftLobby()
        {
            // using (new Load("Leaving Lobby..."))
            {
                PlayersInLobby.Clear();
                NetworkManager.Singleton.Shutdown();
                await MatchmakingService.LeaveLobby();
            }
        }
        
        public void SetReady()
        {
            SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        public async void StartGame()
        {
            // using (new Load("Starting the game..."))
            {
                await MatchmakingService.LockLobby();
                GameStarted();
                _startMatchCommand.Execute();
            }
        }

        [ClientRpc]
        private void UpdatePlayerClientRpc(ulong clientId, bool isReady)
        {
            if (IsServer) return;

            if (!PlayersInLobby.ContainsKey(clientId)) PlayersInLobby.Add(clientId, isReady);
            else PlayersInLobby[clientId] = isReady;
            OnLobbyPlayersUpdated();
        }

        private void PropagateToClients()
        {
            foreach (var player in PlayersInLobby)
            {
                UpdatePlayerClientRpc(player.Key, player.Value);
            }
        }
        
        private void OnClientConnectedCallback(ulong playerId)
        {
            if (!IsServer) return;

            PlayersInLobby.TryAdd(playerId, false);
            PropagateToClients();
            OnLobbyPlayersUpdated();
        }

        private void OnClientDisconnectCallback(ulong playerId)
        {
            if (IsServer)
            {
                if (PlayersInLobby.ContainsKey(playerId)) PlayersInLobby.Remove(playerId);
                RemovePlayerClientRpc(playerId);
                OnLobbyPlayersUpdated();
            }
            else
            {
                LeftLobby();
            }
        }

        [ClientRpc]
        private void RemovePlayerClientRpc(ulong clientId)
        {
            if (IsServer) return;

            if (PlayersInLobby.ContainsKey(clientId)) PlayersInLobby.Remove(clientId);
            OnLobbyPlayersUpdated();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SetReadyServerRpc(ulong playerId)
        {
            PlayersInLobby[playerId] = true;
            PropagateToClients();
            OnLobbyPlayersUpdated();
        }

        private void OnLobbyPlayersUpdated()
        {
            LobbyPlayersUpdated(PlayersInLobby);
        }
    }
}