using UnityEngine;
using UnityEngine.UI;

namespace Core.Network.Misc
{
    public class Scroller : MonoBehaviour
    {
        [SerializeField] private Vector2 _dir = new(0, 0.01f);
        [SerializeField] private RawImage _image;

        private void Update()
        {
            _image.uvRect = new Rect(_image.uvRect.position + _dir * Time.fixedDeltaTime, _image.uvRect.size);
        }
    }
}