using Core.Bootstrap.Commands;
using Core.UI.ViewManagement.Actors;
using UnityEngine;
using Zenject;

namespace Core.Util.Extensions
{
    public static class ContainerExtensions
    {
        public static FromBinderNonGeneric BindInterfaceToConcrete<TContract, TConcrete>(this DiContainer container)
            where TConcrete : TContract
        {
            return container.Bind(typeof(TContract)).To<TConcrete>();
        }

        public static FromBinderNonGeneric BindInterfacesToConcrete<TContract1, TContract2, TConcrete>(this DiContainer container)
            where TConcrete : TContract1, TContract2
        {
            return container.Bind(typeof(TContract1), typeof(TContract2)).To<TConcrete>();
        }

        public static FromBinderNonGeneric BindInterfacesToConcrete<TContract1, TContract2, TContract3, TConcrete>(this DiContainer container)
            where TConcrete : TContract1, TContract2, TContract3
        {
            return container.Bind(typeof(TContract1), typeof(TContract2), typeof(TContract3)).To<TConcrete>();
        }

        public static FromBinderNonGeneric BindInterfacesToConcrete<TContract1, TContract2, TContract3, TContract4, TConcrete>(this DiContainer container)
            where TConcrete : TContract1, TContract2, TContract3, TContract4
        {
            return container.Bind(typeof(TContract1), typeof(TContract2), typeof(TContract3), typeof(TContract4)).To<TConcrete>();
        }

        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindFromInstance<TContract>(this DiContainer container, TContract instance)
        {
            return container.Bind<TContract>().FromInstance(instance);
        }
        
        public static void BindActors<TView, TPresenter>(this DiContainer container)
            where TView : IView
        {
            container.Bind<object>().WithId(typeof(TView).Name).To<TPresenter>().AsTransient();
        }

        public static void BindCommand<TContract, TConcrete>(this DiContainer container)
            where TContract : ICommand
            where TConcrete : TContract
        {
            container.Bind(typeof(TContract)).To<TConcrete>().AsSingle();
        }

        public static void BindInterfaceToCommand<TCommandContract, TContract, TConcrete>(this DiContainer container)
            where TCommandContract : ICommand
            where TConcrete : TCommandContract, TContract
        {
            container.Bind(typeof(TCommandContract), typeof(TContract)).To<TConcrete>().AsSingle();
        }

        public static void BindToComponentOnNewGameObject<TContract, TConcrete>(this DiContainer container, Transform parent = null)
            where TConcrete : Component, TContract
        {
            container
                .Bind<TContract>()
                .FromMethod(c =>
                {
                    var obj = c.Container.InstantiateComponentOnNewGameObject<TConcrete>();

                    if (parent)
                    {
                        obj.transform.SetParent(parent);
                    }

                    return obj;
                })
                .AsSingle()
                .NonLazy();
        }

        public static void BindToComponentOnGameObject<TContract, TConcrete>(this DiContainer container, GameObject gameObject)
            where TConcrete : Component, TContract
        {
            container
                .Bind<TContract>()
                .FromMethod(c => c.Container.InstantiateComponent<TConcrete>(gameObject))
                .AsSingle()
                .NonLazy();
        }

        public static void BindToComponentOnGameObject<TConcrete>(this DiContainer container, GameObject gameObject)
            where TConcrete : Component
        {
            container
                .Bind<TConcrete>()
                .FromMethod(c => c.Container.InstantiateComponent<TConcrete>(gameObject))
                .AsSingle()
                .NonLazy();
        }

        public static void BindFactoryFromPrefab<TComponent>(this DiContainer container, Object prefab)
            where TComponent : Component
        {
            container.BindFactory<TComponent, PlaceholderFactory<TComponent>>().FromComponentInNewPrefab(prefab);
        }
    }
}