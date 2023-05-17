using Core.Network.Misc;
using Core.Network.Services;
using Core.Network.UI.Views;
using Core.UI.ViewManagement.Actors;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Core.Network.UI.Presenters
{
    public class RoomPresenter : Presenter<RoomView>
    {
        protected RoomPresenter(RoomView view) : base(view)
        {
        }

        // public override void Initialize()
        // {
        //     View.LobbyLeft += OnLobbyLeft;
        //     View.StartPressed += OnStartPressed;
        // }
        //
        // public override void Dispose()
        // {
        //     View.StartPressed += OnStartPressed;
        //     View.LobbyLeft += OnLobbyLeft;
        // }
        //
        // private async void OnLobbyLeft()
        // {
        //     using (new Load("Leaving Lobby..."))
        //     {
        //         _playersInLobby.Clear();
        //         NetworkManager.Singleton.Shutdown();
        //         await MatchmakingService.LeaveLobby();
        //     }
        // }
        //
        // private async void OnStartPressed()
        // {
        //     using (new Load("Starting the game..."))
        //     {
        //         await MatchmakingService.LockLobby();
        //         NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        //     }
        // }
    }
}