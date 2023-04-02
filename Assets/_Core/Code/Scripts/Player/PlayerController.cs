using Core.Player.Systems;
using UnityEngine;

namespace Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ControllerSystem _controller;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        private Transform _transform;
        private Vector3 _direction;

        private void OnEnable()
        {
            _transform = transform;
            _direction = _transform.forward;

            _controller.AttackButtonClicked += Attack;
            _controller.SpellButtonClicked += UseSpell;
            _controller.UltimateButtonClicked += UseUltimate;
        }

        private void OnDisable()
        {
            _controller.UltimateButtonClicked -= UseUltimate;
            _controller.SpellButtonClicked -= UseSpell;
            _controller.AttackButtonClicked -= Attack;
        }

        private void Update()
        {
            if (_controller.IsMoving)
            {
                Move();
                Rotate();
            }
        }

        private void Move()
        {
            var moveDirection = new Vector3(_controller.MovementValue.x, 0f, _controller.MovementValue.y);
            
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

        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void UseSpell()
        {
            Debug.Log("Spell");
        }

        private void UseUltimate()
        {
            Debug.Log("Ultimate");
        }
    }
}
