using UnityEngine;

namespace Core.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _followSpeed;

        private Transform _transform;
        private Vector3 _rotation;
        
        private void Start()
        {
            _transform = transform;
            _transform.parent = null;
        }

        private void FixedUpdate()
        {
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            _transform.position = Vector3.Lerp(_transform.position, _player.position + _offset, _followSpeed);
        }
    }
}