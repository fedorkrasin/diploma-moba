using System;
using System.Threading.Tasks;
using Core.Network.Managers;
using Core.Network.UI.Views;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Actors;
using Core.UI.ViewManagement.Data;

namespace Core.Network.UI.Presenters
{
    public class LoginPresenter : Presenter<LoginView>
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly ViewManager _viewManager;
        
        protected LoginPresenter(
            AuthenticationManager authenticationManager,
            ViewManager viewManager,
            LoginView view) : base(view)
        {
            _authenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
        }

        public override void Initialize()
        {
            View.LoginClicked += OnLoginClicked;
        }

        public override void Dispose()
        {
            View.LoginClicked -= OnLoginClicked;
        }

        private async void OnLoginClicked()
        {
            await _authenticationManager.LoginAnonymously();
            await Task.Delay(1000);
            _viewManager.OpenView(ViewId.Lobby);
        }
    }
}