using System;
using Core.UI.Components.Features;
using UnityEngine;

namespace Core.Player.Systems
{
    public class HealthSystem : MonoBehaviour, IRegenerating
    {
        [SerializeField] private FeatureBar _healthbar;
        
        private const float RegenerationDelay = 0.5f; // temp
        
        private int _maxHealth;
        private float _regeneration;

        private int _currentHealth;
        private float _currentExactHealth;
        private float _regenerationTimer;

        public event Action Died = delegate { };

        public void Initialize(float health, float regeneration)
        {
            _maxHealth = (int)health;
            _currentHealth = (int)health;
            _regeneration = regeneration;
        }

        private void Update()
        {
            Regenerate();
            _healthbar.UpdateValue(_currentExactHealth, _maxHealth);
        }

        private void OnMouseDown()
        {
            Damage(20);
        }

        public void Damage(float damageAmount)
        {
            _currentExactHealth -= damageAmount;
            _currentHealth = (int)_currentExactHealth;

            if (_currentExactHealth < 0)
            {
                _currentExactHealth = 0;
                _currentHealth = 0;
                OnDied();
            }
        }

        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;

            if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        }

        public void Regenerate()
        {
            if (_currentHealth != (int) _currentExactHealth) _currentExactHealth = _currentHealth;
            if (_currentHealth == _maxHealth) return;
            
            if (_regenerationTimer >= RegenerationDelay)
            {
                var regeneration = _regeneration * RegenerationDelay;
                
                if (_currentExactHealth + regeneration < _maxHealth)
                {
                    _currentExactHealth += regeneration;
                    _currentHealth = (int)_currentExactHealth;
                }
                else
                {
                    _currentExactHealth = _maxHealth;
                    _currentHealth = _maxHealth;
                }

                _regenerationTimer = 0;
            }
            else
            {
                _regenerationTimer += Time.deltaTime;
            }
        }

        private void OnDied()
        {
            Died();
        }
    }
}