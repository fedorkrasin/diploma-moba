using System;
using UnityEngine;

namespace Core.Player.Systems
{
    public class HealthSystem : IRegenerating
    {
        private const float HealthRegenerationDelay = 0.1f; // temp
        
        private int _maxHealth;
        private float _regeneration;

        private int _currentHealth;
        private float _currentExactHealth;
        private float _regenerationTimer;

        public event Action Died = delegate { };

        public HealthSystem(float health, float regeneration)
        {
            _maxHealth = (int)health;
            _currentHealth = (int)health;
            _regeneration = regeneration;
        }

        public void Update()
        {
            Regenerate();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Damage(10);
            }
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
            
            if (_regenerationTimer >= HealthRegenerationDelay)
            {
                var regeneration = _regeneration * HealthRegenerationDelay;
                
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