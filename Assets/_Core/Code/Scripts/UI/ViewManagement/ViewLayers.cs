using System;
using Core.UI.ViewManagement.Data;
using Core.Util.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.ViewManagement
{
    public class ViewLayers : MonoBehaviour
    {
        private readonly Vector2 _portraitReferenceResolution = new(1080, 1920);
        private readonly Vector2 _landscapeReferenceResolution = new(1920, 1080);

        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private Layers _layers;

        private void Start()
        {
            _canvasScaler.referenceResolution = _landscapeReferenceResolution;
        }

        public RectTransform GetParent(ViewLayer viewLayer)
        {
            if (_layers.Mappings.TryGetValue(viewLayer, out var layer) && layer != null)
            {
                return layer;
            }

            throw new Exception($"Unable to find layer for {viewLayer}.");
        }

        public void ShowLayer(ViewLayer viewLayer)
        {
            _layers.Mappings[viewLayer].gameObject.SetActive(true);
        }

        public void HideLayer(ViewLayer viewLayer)
        {
            _layers.Mappings[viewLayer].gameObject.SetActive(false);
        }

        [Serializable]
        private class Layers : Library<ViewLayer, RectTransform>
        {
        }
    }
}