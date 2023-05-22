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
        
        public event Action<Dictionary<ulong, LobbyPlayer>> LobbyPlayersUpdated = delegate { };

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
            _authenticationManager.PlayersInLobby = new Dictionary<ulong, LobbyPlayer>();
            NetworkObject.DestroyWithScene = true;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
                _authenticationManager.PlayersInLobby.Add(NetworkManager.Singleton.LocalClientId, new LobbyPlayer());
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
                _authenticationManager.PlayersInLobby.Clear();
                NetworkManager.Singleton.Shutdown();
                await MatchmakingService.LeaveLobby();
            }
        }
        
        public void SetReady()
        {
            SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        public void SelectCharacter(int characterId)
        {
            SelectCharacterServerRpc(NetworkManager.Singleton.LocalClientId, characterId);
        }

        public async void StartGame()
        {
            // using (new Load("Starting the game..."))
            {
                await MatchmakingService.LockLobby();
                _startMatchCommand.Execute();
            }
        }

        [ClientRpc]
        private void UpdatePlayerClientRpc(ulong clientId, LobbyPlayer lobbyPlayer)
        {
            if (IsServer) return;

            if (!_authenticationManager.PlayersInLobby.ContainsKey(clientId)) _authenticationManager.PlayersInLobby.Add(clientId, lobbyPlayer);
            else _authenticationManager.PlayersInLobby[clientId] = lobbyPlayer;
            OnLobbyPlayersUpdated();
        }

        private void PropagateToClients()
        {
            foreach (var player in _authenticationManager.PlayersInLobby)
            {
                UpdatePlayerClientRpc(player.Key, player.Value);
            }
        }
        
        private void OnClientConnectedCallback(ulong playerId)
        {
            if (!IsServer) return;

            _authenticationManager.PlayersInLobby.TryAdd(playerId, new LobbyPlayer());
            PropagateToClients();
            OnLobbyPlayersUpdated();
        }

        private void OnClientDisconnectCallback(ulong playerId)
        {
            if (IsServer)
            {
                if (_authenticationManager.PlayersInLobby.ContainsKey(playerId)) _authenticationManager.PlayersInLobby.Remove(playerId);
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

            if (_authenticationManager.PlayersInLobby.ContainsKey(clientId)) _authenticationManager.PlayersInLobby.Remove(clientId);
            OnLobbyPlayersUpdated();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SetReadyServerRpc(ulong playerId)
        {
            _authenticationManager.PlayersInLobby[playerId].IsReady = true;
            PropagateToClients();
            OnLobbyPlayersUpdated();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SelectCharacterServerRpc(ulong playerId, int characterId)
        {
            _authenticationManager.PlayersInLobby[playerId].SelectedCharacterId = characterId;
            PropagateToClients();
            OnLobbyPlayersUpdated();
        }

        private void OnLobbyPlayersUpdated()
        {
            LobbyPlayersUpdated(_authenticationManager.PlayersInLobby);
        }
    }
}