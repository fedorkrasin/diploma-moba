using Core.UI.Views.Impl;
using UnityEngine;

namespace Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private PlayerBehaviour _player;
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        private Transform _transform;
        private Vector3 _direction;

        public PlayerControllerView Controller { get; set; }

        private void Awake()
        {
            _transform = transform;
            _direction = _transform.forward;
        }

        private void FixedUpdate()
        {
            if (!Controller) return;
            
            if (Controller.IsMoving)
            {
                Move();
                Rotate();
                _player.Animator.ToggleAnimation(PlayerAnimator.PlayerAnimationBool.IsRunning, true);
            }
            else
            {
                _player.Animator.ToggleAnimation(PlayerAnimator.PlayerAnimationBool.IsRunning, false);
            }
        }

        private void Move()
        {
            var moveDirection = new Vector3(Controller.MovementValue.x, 0f, Controller.MovementValue.y);
            
            if (moveDirection.magnitude > 0f)
            {
                _direction = moveDirection;
            }

            _rigidbody.MovePosition(_transform.position + _moveSpeed * Time.deltaTime * moveDirection);
        }

        private void Rotate()
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.deltaTime);
        }

        public void Attack()
        {
            _player.Character.Attack();
            _player.Animator.TriggerAnimation(PlayerAnimator.PlayerAnimationTrigger.Attack);
        }

        public bool UseSpell()
        {
            if (_player.Mana.Spend(_player.Character.Spells.Simple.ManaCost))
            {
                _player.Character.UseSpell();
                return true;
            }

            Debug.Log("Not enough mana!");
            return false;
        }

        public bool UseUltimate()
        {
            if (_player.Mana.Spend(_player.Character.Spells.Ultimate.ManaCost))
            {
                _player.Character.UseUltimate();
                return true;
            }

            Debug.Log("Not enough mana!");
            return false;
        }
    }
}
