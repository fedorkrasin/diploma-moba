using System;
using System.Collections.Generic;
using Core.Network.UI.Views;
using Core.UI.ViewManagement.Actors;

namespace Core.Network.UI.Presenters
{
    public class MainLobbyPresenter : Presenter<MainLobbyScreen>
    {
        private readonly Dictionary<ulong, bool> _playersInLobby = new();
        
        private float _nextLobbyUpdate;
        public static event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated = delegate { };
        
        protected MainLobbyPresenter(MainLobbyScreen screen) : base(screen)
        {
        }
    }
}