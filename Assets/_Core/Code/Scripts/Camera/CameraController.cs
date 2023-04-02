using UnityEngine;

namespace Core.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _followSpeed;

        private Transform _transform;
        
        private void Start()
        {
            _transform = transform;
        }

        private void LateUpdate()
        {
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            _transform.position = Vector3.Lerp(_transform.position, _player.position + _offset, _followSpeed * Time.deltaTime);
        }
    }
}