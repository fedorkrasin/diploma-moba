using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI.Components.Controller
{
    public sealed class MovementJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform _joystickBackground;
        [SerializeField] private RectTransform _joystickHandle;

        private Vector2 _inputDirection;
        private float _maxLength;
        
        public bool IsTouched { get; private set; }
        public Vector2 Value => _inputDirection.normalized;

        private void Start()
        {
            _maxLength = _joystickBackground.sizeDelta.x / 3f;
        }

        public void OnDrag(PointerEventData data)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground, data.position, data.pressEventCamera, out var position))
            {
                Debug.Log(Value);
                _inputDirection = new Vector2(position.x, position.y);
                _inputDirection = _inputDirection.magnitude > _maxLength ? _inputDirection.normalized * _maxLength : _inputDirection;
                _joystickHandle.anchoredPosition = new Vector2(_inputDirection.x, _inputDirection.y);
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            IsTouched = true;
            OnDrag(data);
        }

        public void OnPointerUp(PointerEventData data)
        {
            IsTouched = false;
            _inputDirection = Vector2.zero;
            _joystickHandle.anchoredPosition = Vector2.zero;
        }
    }
}