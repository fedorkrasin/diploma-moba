using System;
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

        public SelectCharacterCommand(
            ViewManager viewManager,
            PlayerSpawner playerSpawner)
        {
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
            _playerSpawner = playerSpawner ?? throw new ArgumentNullException(nameof(playerSpawner));
        }
        
        public void Execute()
        {
            
        }

        public void Execute(CharacterType characterType)
        {
            _playerSpawner.Spawn(characterType);
            _viewManager.OpenView(ViewId.PlayerController);
        }
    }
}