using Core.Bootstrap.Commands;
using Core.Bootstrap.Commands.Impl;
using Core.Util.Extensions;
using Zenject;

namespace Core.Installers.Bootstrap
{
    public class CommandInstaller : Installer<CommandInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfaceToCommand<IEntryPointCommand, IInitializable, EntryPointCommand>();
            Container.BindCommand<ISelectCharacterCommand, SelectCharacterCommand>();
            Container.BindCommand<IStartMatchCommand, StartMatchCommand>();
        }
    }
}