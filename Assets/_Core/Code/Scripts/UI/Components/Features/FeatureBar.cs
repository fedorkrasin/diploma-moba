using System;
using Core.Player.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Components.Features
{
    public class FeatureBar : MonoBehaviour
    {
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private ManaSystem _manaSystem;
        [SerializeField] private Image _healthForeground;
        [SerializeField] private Image _manaForeground;
        [SerializeField] private float _reduceSpeed;

        private void OnEnable()
        {
            _healthSystem.CurrentHealth.OnValueChanged += OnHealthChanged;
            _manaSystem.CurrentMana.OnValueChanged += OnManaChanged;
        }
        
        private void OnDisable()
        {
            _manaSystem.CurrentMana.OnValueChanged -= OnManaChanged;
            _healthSystem.CurrentHealth.OnValueChanged -= OnHealthChanged;
        }

        private void UpdateHealthVisual(float value)
        {
            // if (Math.Abs(_healthForeground.fillAmount - value) > 0.01f) // TODO: return this
            // {
            //     _healthForeground.fillAmount = Mathf.MoveTowards(_healthForeground.fillAmount, value, _reduceSpeed * Time.deltaTime);
            // }

            _healthForeground.fillAmount = value;
        }
        
        private void UpdateManaVisual(float value)
        {
            // if (Math.Abs(_manaForeground.fillAmount - value) > 0.01f)
            // {
            //     _manaForeground.fillAmount = Mathf.MoveTowards(_manaForeground.fillAmount, value, _reduceSpeed * Time.deltaTime);
            // }

            _manaForeground.fillAmount = value;
        }

        private void OnHealthChanged(int previousValue, int newValue)
        {
            UpdateHealthVisual((float)newValue / _healthSystem.MaxHealth);
        }
        
        private void OnManaChanged(int previousValue, int newValue)
        {
            UpdateManaVisual((float)newValue / _manaSystem.MaxMana);
        }
    }
}