using System;
using Core.Camera;
using Core.Characters;
using Core.Player;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Data;

namespace Core.Bootstrap.Commands.Impl
{
    public class SelectCharacterCommand : ISelectCharacterCommand
    {
        private readonly ViewManager _viewManager;
        private readonly PlayerSpawner _playerSpawner;
        private readonly CameraController _cameraController;

        public SelectCharacterCommand(
            ViewManager viewManager,
            PlayerSpawner playerSpawner,
            CameraController cameraController)
        {
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
            _playerSpawner = playerSpawner ?? throw new ArgumentNullException(nameof(playerSpawner));
            _cameraController = cameraController ? cameraController : throw new ArgumentNullException(nameof(cameraController));
        }
        
        public void Execute()
        {
            
        }

        public void Execute(CharacterType characterType)
        {
            var player = _playerSpawner.Spawn(characterType);
            // _cameraController.PlayerTransform = player.transform;
            _viewManager.OpenView(ViewId.PlayerController);
        }
    }
}