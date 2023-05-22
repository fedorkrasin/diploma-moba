using Core.UI.Components.Features;
using Unity.Netcode;
using UnityEngine;

namespace Core.Player.Systems
{
    public class ManaSystem : NetworkBehaviour, IRegenerating
    {
        private const float RegenerationDelay = 0.5f; // temp
        
        private float _regeneration;
        
        private float _currentExactMana;
        private float _regenerationTimer;
        
        public NetworkVariable<int> CurrentMana { get; } = new();
        public int MaxMana { get; private set; }

        public void Initialize(float mana, float regeneration)
        {
            _regeneration = regeneration;
            MaxMana = (int)mana;
            CurrentMana.Value = (int)mana;
        }

        private void Update()
        {
            Regenerate();
        }

        public bool Spend(float manaAmount)
        {
            Debug.Log(_currentExactMana - manaAmount);
            if (!IsServer) return false;
            
            if (_currentExactMana - manaAmount < 0) return false;
            
            _currentExactMana -= manaAmount;
            CurrentMana.Value = (int)_currentExactMana;

            return true;
        }

        public void Regenerate(int manaAmount)
        {
            CurrentMana.Value += manaAmount;

            if (CurrentMana.Value > MaxMana) CurrentMana.Value = MaxMana;
        }

        public void Regenerate()
        {
            if (!IsServer) return;
            
            if (CurrentMana.Value != (int) _currentExactMana) _currentExactMana = CurrentMana.Value;
            if (CurrentMana.Value == MaxMana) return;
            
            if (_regenerationTimer >= RegenerationDelay)
            {
                var regeneration = _regeneration * RegenerationDelay;
                
                if (_currentExactMana + regeneration < MaxMana)
                {
                    _currentExactMana += regeneration;
                    CurrentMana.Value = (int)_currentExactMana;
                }
                else
                {
                    _currentExactMana = MaxMana;
                    CurrentMana.Value = MaxMana;
                }

                _regenerationTimer = 0;
            }
            else
            {
                _regenerationTimer += Time.deltaTime;
            }
        }
    }
}