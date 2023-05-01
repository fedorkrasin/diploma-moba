using System;
using Core.Player;
using Core.UI.ViewManagement.Actors;
using Core.UI.Views;
using Core.UI.Views.Impl;

namespace Core.UI.Presenters
{
    public class PlayerControllerPresenter : Presenter<IPlayerControllerView>
    {
        private readonly PlayerSpawner _playerSpawner;

        private PlayerBehaviour _player;
        
        protected PlayerControllerPresenter(
            PlayerSpawner playerSpawner,
            IPlayerControllerView view) : base(view)
        {
            _playerSpawner = playerSpawner ?? throw new ArgumentNullException(nameof(playerSpawner));
        }

        public override void Initialize()
        {
            _player = _playerSpawner.Player;
            _player.Controller.Controller = (PlayerControllerView)View; // TODO: fix that
            
            View.AttackButtonClicked += OnAttackButtonClicked;
            View.SpellButtonClicked += OnSpellButtonClicked;
            View.UltimateButtonClicked += OnUltimateButtonClicked;
        }

        public override void Dispose()
        {
            View.UltimateButtonClicked -= OnUltimateButtonClicked;
            View.SpellButtonClicked -= OnSpellButtonClicked;
            View.AttackButtonClicked -= OnAttackButtonClicked;
        }

        private void OnAttackButtonClicked()
        {
            _player.Controller.Attack();
        }

        private void OnSpellButtonClicked()
        {
            if (_player.Controller.UseSpell())
            {
                View.SpellButton.SetCooldown(_player.Character.Spells.Simple.Cooldown);
            }
        }

        private void OnUltimateButtonClicked()
        {
            if (_playerSpawner.Player.Controller.UseUltimate())
            {
                View.UltimateButton.SetCooldown(_player.Character.Spells.Ultimate.Cooldown);
            }
        }
    }
}