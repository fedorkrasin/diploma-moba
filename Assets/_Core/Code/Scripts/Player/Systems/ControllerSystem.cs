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
        [SerializeField] private Button _spellButton;
        [SerializeField] private Button _ultimateButton;
        
        public event Action AttackButtonClicked = delegate { };
        public event Action SpellButtonClicked = delegate { };
        public event Action UltimateButtonClicked = delegate { };

        private void OnEnable()
        {
            _attackButton.onClick.AddListener(OnAttackButtonClicked);
            _spellButton.onClick.AddListener(OnSpellButtonClicked);
            _ultimateButton.onClick.AddListener(OnUltimateButtonClicked);
        }

        private void OnDisable()
        {
            _ultimateButton.onClick.RemoveListener(OnUltimateButtonClicked);
            _spellButton.onClick.RemoveListener(OnSpellButtonClicked);
            _attackButton.onClick.RemoveListener(OnAttackButtonClicked);
        }

        public bool IsMoving => _movementJoystick.IsTouched;
        public Vector2 MovementValue => _movementJoystick.Value;

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