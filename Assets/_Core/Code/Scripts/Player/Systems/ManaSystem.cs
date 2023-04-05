using Core.UI.Components.Features;
using UnityEngine;

namespace Core.Player.Systems
{
    public class ManaSystem : MonoBehaviour, IRegenerating
    {
        [SerializeField] private FeatureBar _manabar;
        
        private const float RegenerationDelay = 0.5f; // temp
        
        private int _maxMana;
        private float _regeneration;

        private int _currentMana;
        private float _currentExactMana;
        private float _regenerationTimer;

        public void Initialize(float health, float regeneration)
        {
            _maxMana = (int)health;
            _currentMana = (int)health;
            _regeneration = regeneration;
        }

        private void Update()
        {
            Regenerate();
            _manabar.UpdateValue(_currentExactMana, _maxMana);
        }

        public bool Spend(float manaAmount)
        {
            if (_currentExactMana - manaAmount < 0) return false;
            
            _currentExactMana -= manaAmount;
            _currentMana = (int)_currentExactMana;

            return true;
        }

        public void Regenerate(int healAmount)
        {
            _currentMana += healAmount;

            if (_currentMana > _maxMana) _currentMana = _maxMana;
        }

        public void Regenerate()
        {
            if (_currentMana != (int) _currentExactMana) _currentExactMana = _currentMana;
            if (_currentMana == _maxMana) return;
            
            if (_regenerationTimer >= RegenerationDelay)
            {
                var regeneration = _regeneration * RegenerationDelay;
                
                if (_currentExactMana + regeneration < _maxMana)
                {
                    _currentExactMana += regeneration;
                    _currentMana = (int)_currentExactMana;
                }
                else
                {
                    _currentExactMana = _maxMana;
                    _currentMana = _maxMana;
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