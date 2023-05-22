using Core.UI.ViewManagement.Actors;
using UnityEngine;

namespace Core.Network.UI.Views
{
    public class LobbyView : View
    {
        [SerializeField] private MainLobbyScreen _mainLobbyScreen;
        [SerializeField] private CreateLobbyScreen _createLobbyScreen;
        [SerializeField] private RoomScreen _roomScreen;

        public MainLobbyScreen MainLobby => _mainLobbyScreen;
        public CreateLobbyScreen CreateLobby => _createLobbyScreen;
        public RoomScreen Room => _roomScreen;

        public void ShowMainLobby()
        {
            _mainLobbyScreen.gameObject.SetActive(true);
            _createLobbyScreen.gameObject.SetActive(false);
            _roomScreen.gameObject.SetActive(false);
        }

        public void ShowCreateLobby()
        {
            _mainLobbyScreen.gameObject.SetActive(false);
            _createLobbyScreen.gameObject.SetActive(true);
            _roomScreen.gameObject.SetActive(false);
        }

        public void ShowRoom()
        {
            _mainLobbyScreen.gameObject.SetActive(false);
            _createLobbyScreen.gameObject.SetActive(false);
            _roomScreen.gameObject.SetActive(true);
        }
    }
}