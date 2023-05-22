using UnityEngine;

namespace Core.Camera
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.rotation = _cameraTransform.rotation;
        }
    }
}