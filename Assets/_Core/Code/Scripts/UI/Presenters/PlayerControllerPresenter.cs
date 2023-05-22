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

        public PlayerBehaviour Player { get; set; }
        
        protected PlayerControllerPresenter(
            PlayerSpawner playerSpawner,
            IPlayerControllerView view) : base(view)
        {
            _playerSpawner = playerSpawner ?? throw new ArgumentNullException(nameof(playerSpawner));
        }

        public override void Initialize()
        {
            View.Presenter = this; // TODO: fix
            
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
            Player.Attack();
        }

        private void OnSpellButtonClicked()
        {
            if (Player.UseSpell())
            {
                View.SpellButton.SetCooldown(Player.Character.Spells.Simple.Cooldown);
            }
        }

        private void OnUltimateButtonClicked()
        {
            if (Player.UseUltimate())
            {
                View.UltimateButton.SetCooldown(Player.Character.Spells.Ultimate.Cooldown);
            }
        }
    }
}