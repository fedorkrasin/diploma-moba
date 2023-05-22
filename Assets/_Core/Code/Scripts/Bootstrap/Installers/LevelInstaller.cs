using Core.Network.Managers;
using Zenject;

namespace Core.Installers.Bootstrap
{
    public class LevelInstaller : MonoInstaller<LevelInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}