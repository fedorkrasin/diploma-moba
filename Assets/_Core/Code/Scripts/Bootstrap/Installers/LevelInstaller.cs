using Core.Network.Managers;
using Zenject;

namespace Core.Installers.Bootstrap
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LobbyOrchestrator>().FromComponentInHierarchy().AsSingle();
        }
    }
}