using Unity.Netcode;
using UnityEngine;

namespace Core.Camera
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _followSpeed;

        private Transform _playerTransform;

        private Transform _transform;
        private Vector3 _rotation;
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                _camera.depth = 1;
            }
            else
            {
                _camera.depth = -1;
            }
        }
        
        private void Start()
        {
            _transform = transform;
            _playerTransform = _transform.parent;
            _transform.parent = null;
        }

        private void FixedUpdate()
        {
            if (_playerTransform)
            {
                FollowPlayer();
            }
        }

        private void FollowPlayer()
        {
            _transform.position = Vector3.Lerp(_transform.position, _playerTransform.position + _offset, _followSpeed);
        }
    }
}