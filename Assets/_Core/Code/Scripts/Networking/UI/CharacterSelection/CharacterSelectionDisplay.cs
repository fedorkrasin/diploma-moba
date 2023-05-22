using System.Collections.Generic;
using Core.Characters.Data;
using Core.Networking.Data;
using Core.Networking.Server;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Networking.UI.CharacterSelection
{
    public class CharacterSelectionDisplay : NetworkBehaviour
    {
        [Header("References")] 
        [SerializeField] private CharactersList _charactersList;
        [SerializeField] private Transform _charactersHolder;
        [SerializeField] private CharacterSelectionButton _selectionButtonPrefab;
        [SerializeField] private PlayerCard[] _playerCards;
        [SerializeField] private GameObject _characterInfoPanel;
        [SerializeField] private TMP_Text _characterNameText;
        [SerializeField] private TMP_Text _joinCodeText;
        [SerializeField] private Button _lockInButton;

        private readonly List<CharacterSelectionButton> _characterButtons = new();
        private NetworkList<CharacterSelectionState> _players;

        // [SerializeField] private Transform introSpawnPoint; TODO: character intro 
        // private GameObject introInstance;

        private void Awake()
        {
            _players = new NetworkList<CharacterSelectionState>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                var allCharacters = _charactersList.CharactersData;

                foreach (var character in allCharacters)
                {
                    var selectButtonInstance = Instantiate(_selectionButtonPrefab, _charactersHolder);
                    selectButtonInstance.SetCharacter(this, character);
                    _characterButtons.Add(selectButtonInstance);
                }

                _players.OnListChanged += HandlePlayersStateChanged;
            }

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

                foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            }

            if (IsHost)
            {
                // _joinCodeText.text = HostSingleton.Instance.RelayHostData.JoinCode;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
            {
                _players.OnListChanged -= HandlePlayersStateChanged;
            }

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            _players.Add(new CharacterSelectionState(clientId));
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].ClientId != clientId)
                {
                    continue;
                }

                _players.RemoveAt(i);
                break;
            }
        }

        public void Select(CharacterData characterData)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].ClientId != NetworkManager.Singleton.LocalClientId)
                {
                    continue;
                }

                if (_players[i].IsLockedIn)
                {
                    return;
                }

                if (_players[i].CharacterId == characterData.Id)
                {
                    return;
                }

                if (IsCharacterTaken(characterData.Id, false))
                {
                    return;
                }
            }

            _characterNameText.text = characterData.Name;

            _characterInfoPanel.SetActive(true);

            // if (introInstance != null) TODO: character intro 
            // {
            //     Destroy(introInstance);
            // }
            //
            // introInstance = Instantiate(characterData.IntroPrefab, introSpawnPoint);

            SelectServerRpc(characterData.Id);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].ClientId != serverRpcParams.Receive.SenderClientId)
                {
                    continue;
                }

                if (!_charactersList.IsValidCharacterId(characterId))
                {
                    return;
                }

                if (IsCharacterTaken(characterId, true))
                {
                    return;
                }

                _players[i] = new CharacterSelectionState(
                    _players[i].ClientId,
                    characterId,
                    _players[i].IsLockedIn
                );
            }
        }

        public void LockIn()
        {
            LockInServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void LockInServerRpc(ServerRpcParams serverRpcParams = default)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].ClientId != serverRpcParams.Receive.SenderClientId)
                {
                    continue;
                }

                if (!_charactersList.IsValidCharacterId(_players[i].CharacterId))
                {
                    return;
                }

                if (IsCharacterTaken(_players[i].CharacterId, true))
                {
                    return;
                }

                _players[i] = new CharacterSelectionState(
                    _players[i].ClientId,
                    _players[i].CharacterId,
                    true
                );
            }

            foreach (var player in _players)
            {
                if (!player.IsLockedIn)
                {
                    return;
                }
            }

            foreach (var player in _players)
            {
                MatchplayNetworkServer.Instance.SetCharacter(player.ClientId, player.CharacterId);
            }

            MatchplayNetworkServer.Instance.StartGame();
        }

        private void HandlePlayersStateChanged(NetworkListEvent<CharacterSelectionState> changeEvent)
        {
            for (int i = 0; i < _playerCards.Length; i++)
            {
                if (_players.Count > i)
                {
                    _playerCards[i].UpdateVisuals(_players[i]);
                }
                else
                {
                    _playerCards[i].DisableDisplay();
                }
            }

            foreach (var button in _characterButtons)
            {
                if (button.IsDisabled)
                {
                    continue;
                }

                if (IsCharacterTaken(button.Character.Id, false))
                {
                    button.SetDisabled();
                }
            }

            foreach (var player in _players)
            {
                if (player.ClientId != NetworkManager.Singleton.LocalClientId)
                {
                    continue;
                }

                if (player.IsLockedIn)
                {
                    _lockInButton.interactable = false;
                    break;
                }

                if (IsCharacterTaken(player.CharacterId, false))
                {
                    _lockInButton.interactable = false;
                    break;
                }

                _lockInButton.interactable = true;

                break;
            }
        }

        private bool IsCharacterTaken(int characterId, bool checkAll)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (!checkAll)
                {
                    if (_players[i].ClientId == NetworkManager.Singleton.LocalClientId)
                    {
                        continue;
                    }
                }

                if (_players[i].IsLockedIn && _players[i].CharacterId == characterId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}