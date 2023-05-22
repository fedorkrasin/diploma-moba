using Core.Network.Managers;
using Zenject;

namespace Core.Installers.Bootstrap
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LobbyOrchestrator>().FromComponentInHierarchy().AsSingle();
        }
    }
}