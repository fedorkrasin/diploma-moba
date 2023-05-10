using System;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Actors;
using Core.UI.ViewManagement.Data;
using Core.UI.Views;
using Unity.Netcode;

namespace Core.UI.Presenters
{
    public class NetworkControllerPresenter : Presenter<INetworkControllerView>
    {
        private readonly ViewManager _viewManager;
        
        protected NetworkControllerPresenter(
            INetworkControllerView view,
            ViewManager viewManager) : base(view)
        {
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
        }

        public override void Initialize()
        {
            View.ServerClicked += OnServerClicked;
            View.HostClicked += OnHostClicked;
            View.ClientClicked += OnClientClicked;
        }

        public override void Dispose()
        {
            View.ClientClicked -= OnClientClicked;
            View.HostClicked -= OnHostClicked;
            View.ServerClicked -= OnServerClicked;
        }

        private void OnServerClicked()
        {
            NetworkManager.Singleton.StartServer();
        }

        private void OnHostClicked()
        {
            NetworkManager.Singleton.StartHost();
            // _viewManager.OpenView(ViewId.CharacterSelection);
            _viewManager.OpenView(ViewId.PlayerController);
        }

        private void OnClientClicked()
        {
            NetworkManager.Singleton.StartClient();
            // _viewManager.OpenView(ViewId.CharacterSelection);
            _viewManager.OpenView(ViewId.PlayerController);
        }
    }
}