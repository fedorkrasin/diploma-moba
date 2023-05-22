using System;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Data;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Core.Bootstrap.Commands.Impl
{
    public class StartMatchCommand : IStartMatchCommand
    {
        private readonly ViewManager _viewManager;

        public StartMatchCommand(ViewManager viewManager)
        {
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
        }
        
        public void Execute()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}