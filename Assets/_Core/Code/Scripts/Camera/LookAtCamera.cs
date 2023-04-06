using UnityEngine;

namespace Core.Camera
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _transform;
        private Transform _cameraTransform;

        private void Start()
        {
            _transform = transform;
            _cameraTransform = UnityEngine.Camera.main!.transform;
        }

        private void Update()
        {
            _transform.rotation = _cameraTransform.rotation;
        }
    }
}