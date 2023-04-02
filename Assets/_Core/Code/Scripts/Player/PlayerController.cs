using UnityEngine;

namespace Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Joystick _joystick;

        private Transform _transform;
        private Vector3 _direction;

        private void Start()
        {
            _transform = transform;
            _direction = _transform.forward;
        }

        private void Update()
        {
            if (_joystick.IsTouched)
            {
                Move();
                Rotate();
            }
        }

        private void Move()
        {
            var moveDirection = new Vector3(_joystick.Value.x, 0f, _joystick.Value.y);
            
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
    }
}
