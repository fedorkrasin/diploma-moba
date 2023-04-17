using System;
using Core.Util.Misc;
using UnityEngine;

namespace Core.UI.ViewManagement.Data
{
    [CreateAssetMenu(menuName = "UI/ViewMappings", fileName = "ViewMappings")]
    public class ViewMappings : ScriptableObjectLibrary<ViewId, View>
    {
        public ViewLayer GetLayer(ViewId viewId)
        {
            var mapping = GetMapping(viewId, () => $"Unknown view {viewId}.");
            return mapping.Layer;
        }

        public GameObject GetView(ViewId viewId)
        {
            var mapping = GetMapping(viewId, () => $"Unable to find prefab for {viewId}.", m => m.Prefab != null);
            return mapping.Prefab;
        }

        private View GetMapping(ViewId viewId, Func<string> message = null, Predicate<View> predicate = null)
        {
            predicate ??= _ => true;
            message ??= () => string.Empty;

            if (Library.TryGetValue(viewId, out var view) && view != null && predicate(view))
            {
                return view;
            }

            throw new Exception(message());
        }
    }
    
    [Serializable]
    public class View
    {
        public ViewLayer Layer;
        public GameObject Prefab;
    }
}