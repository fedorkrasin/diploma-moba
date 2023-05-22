using System;
using Core.UI.ViewManagement.Actors;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Network.UI.Views
{
    public class LoginView : View
    {
        [SerializeField] private Button _loginButton;

        public event Action LoginClicked = delegate { };

        private void OnEnable()
        {
            _loginButton.onClick.AddListener(OnLoginClicked);
        }

        private void OnDisable()
        {
            _loginButton.onClick.RemoveListener(OnLoginClicked);
        }

        private void OnLoginClicked()
        {
            LoginClicked();
        }
    }
}