using System;
using Core.UI.Components.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Player.Systems
{
    public class ControllerSystem : MonoBehaviour
    {
        [SerializeField] private MovementJoystick _movementJoystick;
        [SerializeField] private Button _attackButton;
        [SerializeField] private CooldownButton _spellButton;
        [SerializeField] private CooldownButton _ultimateButton;
        
        public bool IsMoving => _movementJoystick.IsTouched;
        public Vector2 MovementValue => _movementJoystick.Value;
        public CooldownButton SpellButton => _spellButton;
        public CooldownButton UltimateButton => _ultimateButton;
        
        public event Action AttackButtonClicked = delegate { };
        public event Action SpellButtonClicked = delegate { };
        public event Action UltimateButtonClicked = delegate { };

        private void OnEnable()
        {
            _attackButton.onClick.AddListener(OnAttackButtonClicked);
            _spellButton.Button.onClick.AddListener(OnSpellButtonClicked);
            _ultimateButton.Button.onClick.AddListener(OnUltimateButtonClicked);
        }

        private void OnDisable()
        {
            _ultimateButton.Button.onClick.RemoveListener(OnUltimateButtonClicked);
            _spellButton.Button.onClick.RemoveListener(OnSpellButtonClicked);
            _attackButton.onClick.RemoveListener(OnAttackButtonClicked);
        }

        private void OnAttackButtonClicked()
        {
            AttackButtonClicked();
        }

        private void OnSpellButtonClicked()
        {
            SpellButtonClicked();
        }

        private void OnUltimateButtonClicked()
        {
            UltimateButtonClicked();
        }
    }
}