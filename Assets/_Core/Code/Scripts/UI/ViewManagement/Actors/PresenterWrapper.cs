using System;
using UnityEngine;

namespace Core.UI.ViewManagement.Actors
{
    public class PresenterWrapper : MonoBehaviour, IDisposable
    {
        private IDisposable _presenter;
        private bool _isInitialized;

        public void SetPresenter(object presenter)
        {
            if (_isInitialized)
            {
                Debug.LogWarning($"{nameof(PresenterWrapper)} already initialized.");

                return;
            }

            _presenter = (IDisposable)presenter;
            _isInitialized = true;
        }

        public void Dispose()
        {
            _presenter?.Dispose();
        }
    }
}