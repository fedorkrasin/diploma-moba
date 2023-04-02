using Core.Player.Stats;
using Core.Player.Systems;
using UnityEngine;

namespace Core.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [field: SerializeField] public PlayerController Controller;

        [SerializeField] private PlayerStats _stats;
        
        private HealthSystem _healthSystem;

        private void Awake()
        {
            _healthSystem = new HealthSystem(_stats.Health, _stats.HealthRegeneration);
        }

        private void Update()
        {
            _healthSystem.Update();
        }
    }
}