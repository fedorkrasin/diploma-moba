using System;
using Core.Characters.Data;
using Core.Network.Services;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Data;
using Unity.Netcode;
using Zenject;

namespace Core.Network.Managers
{
    public class GameManager : NetworkBehaviour
    {
        private ViewManager _viewManager;
        private AuthenticationManager _authenticationManager;
        private CharactersList _charactersList;
        
        [Inject]
        private void Construct(
            ViewManager viewManager, 
            AuthenticationManager authenticationManager,
            CharactersList charactersList)
        {
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
            _authenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
            _charactersList = charactersList ? charactersList : throw new ArgumentNullException(nameof(charactersList));
        }

        public override void OnNetworkSpawn()
        {
            _viewManager.OpenView(ViewId.PlayerController);
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayerServerRpc(ulong playerId)
        {
            var character = _charactersList.Get(_authenticationManager.PlayersInLobby[playerId].SelectedCharacterId).CharacterPrefab;
            var spawn = Instantiate(character);
            spawn.NetworkObject.SpawnWithOwnership(playerId);
        }

        public override async void OnDestroy()
        {
            base.OnDestroy();
            await MatchmakingService.LeaveLobby();
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.Shutdown();
        }
    }
}