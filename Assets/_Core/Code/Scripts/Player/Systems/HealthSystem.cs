using System;
using Core.UI.Components.Features;
using Unity.Netcode;
using UnityEngine;

namespace Core.Player.Systems
{
    public class HealthSystem : NetworkBehaviour, IRegenerating
    {
        private const float RegenerationDelay = 0.5f; // temp
        
        private float _regeneration;

        private float _currentExactHealth;
        private float _regenerationTimer;

        public NetworkVariable<int> CurrentHealth { get; } = new();
        public int MaxHealth { get; private set; }

        public event Action Died = delegate { };

        public void Initialize(float health, float regeneration)
        {
            _regeneration = regeneration;
            MaxHealth = (int)health;
            CurrentHealth.Value = (int)health;
        }

        private void Update()
        {
            Regenerate();
        }

        private void OnMouseDown()
        {
            Damage(20);
        }

        public void Damage(float damageAmount)
        {
            if (!IsServer) return;
            
            _currentExactHealth -= damageAmount;
            CurrentHealth.Value = (int)_currentExactHealth;

            if (_currentExactHealth < 0)
            {
                _currentExactHealth = 0;
                CurrentHealth.Value = 0;
                OnDied();
            }
        }

        public void Heal(int healAmount)
        {
            CurrentHealth.Value += healAmount;

            if (CurrentHealth.Value > MaxHealth) CurrentHealth.Value = MaxHealth;
        }

        public void Regenerate()
        {
            if (!IsServer) return;
            
            if (CurrentHealth.Value != (int) _currentExactHealth) _currentExactHealth = CurrentHealth.Value;
            if (CurrentHealth.Value == MaxHealth) return;
            
            if (_regenerationTimer >= RegenerationDelay)
            {
                var regeneration = _regeneration * RegenerationDelay;
                
                if (_currentExactHealth + regeneration < MaxHealth)
                {
                    _currentExactHealth += regeneration;
                    CurrentHealth.Value = (int)_currentExactHealth;
                }
                else
                {
                    _currentExactHealth = MaxHealth;
                    CurrentHealth.Value = MaxHealth;
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