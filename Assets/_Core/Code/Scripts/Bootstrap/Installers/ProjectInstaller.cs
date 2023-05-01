using Core.Camera;
using Core.Characters.Data;
using Core.Player;
using Core.UI.ViewManagement;
using Core.UI.ViewManagement.Data;
using UnityEngine;
using Zenject;

namespace Core.Installers.Bootstrap
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ViewLayers _viewLayers;
        [SerializeField] private ViewMappings _viewMappings;

        [SerializeField] private CharactersList _charactersList;

        [SerializeField] private CameraController _cameraController;
        
        public override void InstallBindings()
        {
            CommandInstaller.Install(Container);
            UiInstaller.Install(Container);
            
            Container.Bind<ViewLayers>().FromInstance(_viewLayers).AsSingle();
            Container.BindInstance(_viewMappings).AsSingle();
            Container.BindInterfacesAndSelfTo<ViewManager>().AsSingle();
            
            Container.Bind<PlayerSpawner>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<CharactersList>().FromInstance(_charactersList).AsSingle();

            Container.BindInstance(_cameraController).AsSingle();
        }
    }
}