using System;
using Core.Network.Data;
using Core.Network.Misc;
using Core.Network.Services;
using Core.Network.UI.Views;
using Core.UI.ViewManagement.Actors;
using Unity.Netcode;
using UnityEngine;

namespace Core.Network.UI.Presenters
{
    public class CreateLobbyPresenter : Presenter<CreateLobbyView>
    {
        protected CreateLobbyPresenter(CreateLobbyView view) : base(view)
        {
        }

        // public override void Initialize()
        // {
        //     View.LobbyCreated += CreateLobby;
        // }
        //
        // public override void Dispose()
        // {
        //     View.LobbyCreated -= CreateLobby;
        // }
        //
        // private async void CreateLobby(LobbyData data)
        // {
        //     using (new Load("Creating Lobby..."))
        //     {
        //         try
        //         {
        //             await MatchmakingService.CreateLobbyWithAllocation(data);
        //             NetworkManager.Singleton.StartHost();
        //         }
        //         catch (Exception e)
        //         {
        //             Debug.LogError(e);
        //             CanvasUtilities.Instance.ShowError("Failed creating lobby");
        //         }
        //     }
        // }
    }
}