using UnityEngine;

namespace Core.Player.Stats
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField] private float _health;
        [SerializeField] private float _healthRegeneration;
        [SerializeField] private float _mana;
        [SerializeField] private float _manaRegeneration;

        public float Health => _health;
        public float HealthRegeneration => _healthRegeneration;
        
        public float Mana => _mana;
        public float ManaRegeneration => _manaRegeneration;
    }
}