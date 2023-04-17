using Zenject;

namespace Core.Installers.Bootstrap
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            CommandInstaller.Install(Container);
        }
    }
}