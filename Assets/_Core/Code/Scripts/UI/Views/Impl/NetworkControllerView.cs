using System;
using Core.UI.ViewManagement.Actors;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views.Impl
{
    public class NetworkControllerView : View, INetworkControllerView
    {
        [SerializeField] private Button _serverButton;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        
        public event Action ServerClicked = delegate { }; 
        public event Action HostClicked = delegate { }; 
        public event Action ClientClicked = delegate { }; 

        private void OnEnable()
        {
            _serverButton.onClick.AddListener(OnServerClicked);
            _hostButton.onClick.AddListener(OnHostClicked);
            _clientButton.onClick.AddListener(OnClientClicked);
        }
        
        private void OnDisable()
        {
            _clientButton.onClick.RemoveListener(OnClientClicked);
            _hostButton.onClick.RemoveListener(OnHostClicked);
            _serverButton.onClick.RemoveListener(OnServerClicked);
        }

        private void OnServerClicked()
        {
            ServerClicked();
        }

        private void OnHostClicked()
        {
            HostClicked();
        }

        private void OnClientClicked()
        {
            ClientClicked();
        }
    }
}