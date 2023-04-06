using Core.Player.Stats;
using Core.Player.Systems;
using UnityEngine;

namespace Core.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [field: SerializeField] public PlayerController Controller;
        [field: SerializeField] public PlayerAnimator Animator;
        [field: SerializeField] public HealthSystem Health;
        [field: SerializeField] public ManaSystem Mana;
        
        [SerializeField] private PlayerStats _stats;

        private void Awake()
        {
            Health.Initialize(_stats.Health, _stats.HealthRegeneration);
            Mana.Initialize(_stats.Mana, _stats.ManaRegeneration);
        }
    }
}