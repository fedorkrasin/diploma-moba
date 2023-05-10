using Core.Characters;
using Core.Player.Stats;
using Core.Player.Systems;
using Core.UI.Views.Impl;
using Unity.Netcode;
using UnityEngine;

namespace Core.Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [field: SerializeField] public Character Character;
        [field: SerializeField] public PlayerAnimator Animator;
        [field: SerializeField] public HealthSystem Health;
        [field: SerializeField] public ManaSystem Mana;
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private PlayerStats _stats;

        private Transform _transform;
        private Vector3 _direction;

        public PlayerControllerView Controller { get; set; }
        
        private void Awake()
        {
            _transform = transform;
            _direction = _transform.forward;
            Health.Initialize(_stats.Health, _stats.HealthRegeneration);
            Mana.Initialize(_stats.Mana, _stats.ManaRegeneration);
        }

        private void FixedUpdate()
        {
            // if (!IsOwner) return;
            if (!Controller) return;
            
            if (Controller.IsMoving)
            {
                Move();
                Rotate();
                Animator.ToggleAnimation(PlayerAnimator.PlayerAnimationBool.IsRunning, true);
            }
            else
            {
                Animator.ToggleAnimation(PlayerAnimator.PlayerAnimationBool.IsRunning, false);
            }
        }

        private void Move()
        {
            var moveDirection = new Vector3(Controller.MovementValue.x, 0f, Controller.MovementValue.y);
            
            if (moveDirection.magnitude > 0f)
            {
                _direction = moveDirection;
            }

            _rigidbody.MovePosition(_transform.position + _stats.MoveSpeed * Time.deltaTime * moveDirection);
        }

        private void Rotate()
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(_direction), _stats.RotationSpeed * Time.deltaTime);
        }

        public void Attack()
        {
            Character.Attack();
            Animator.TriggerAnimation(PlayerAnimator.PlayerAnimationTrigger.Attack);
        }

        public bool UseSpell()
        {
            if (Mana.Spend(Character.Spells.Simple.ManaCost))
            {
                Character.UseSpell();
                return true;
            }

            Debug.Log("Not enough mana!");
            return false;
        }

        public bool UseUltimate()
        {
            if (Mana.Spend(Character.Spells.Ultimate.ManaCost))
            {
                Character.UseUltimate();
                return true;
            }

            Debug.Log("Not enough mana!");
            return false;
        }
    }
}