using System;
using System.Linq;
using Core.UI.ViewManagement.Actors;
using Core.UI.ViewManagement.Data;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Core.UI.ViewManagement
{
    public class ViewManager
    {
        private readonly DiContainer _container;
        private readonly ViewLayers _viewLayers;
        private readonly ViewMappings _viewMappings;

        public ViewManager(
            DiContainer container,
            ViewLayers viewLayers,
            ViewMappings viewMappings)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _viewLayers = viewLayers ? viewLayers : throw new ArgumentNullException(nameof(viewLayers));
            _viewMappings = viewMappings ? viewMappings : throw new ArgumentNullException(nameof(viewMappings));
        }

        public void OpenView(ViewId id)
        {
            var layer = _viewMappings.GetLayer(id);
            var children = _viewLayers.GetParent(layer).Cast<Transform>().ToList();
            children.ForEach(c => Dispose(c.gameObject));

            var viewObject = CreateView(id);
            var presenter = InjectPresenter(viewObject, out var view);

            Initialize(view, presenter);
        }

        public void ClosePopup(GameObject view)
        {
            Dispose(view);
        }

        private GameObject CreateView(ViewId id)
        {
            var layer = _viewMappings.GetLayer(id);
            var parent = _viewLayers.GetParent(layer);
            var viewPrefab = _viewMappings.GetView(id);

            var viewObject = Object.Instantiate(viewPrefab, parent);
            _container.InjectGameObject(viewObject);

            return viewObject;
        }

        private object InjectPresenter(GameObject viewObject, out IView view)
        {
            view = viewObject.GetComponent<IView>();
            var presenterWrapper = viewObject.AddComponent<PresenterWrapper>();
            var presenterType = _container.ResolveType(new InjectContext(_container, typeof(object), view.GetType().Name));
            var presenter = _container.Instantiate(presenterType, new[] { view });
            presenterWrapper.SetPresenter(presenter);

            return presenter;
        }

        private static void Initialize(IView view, object presenter)
        {
            ((IInitializable)view).Initialize();
            ((IInitializable)presenter).Initialize();
        }

        private static void Dispose(GameObject view)
        {
            ((IDisposable)view.GetComponent<PresenterWrapper>()).Dispose();
            ((IDisposable)view.GetComponent<IView>()).Dispose();

            Object.Destroy(view.gameObject);
        }
    }
}