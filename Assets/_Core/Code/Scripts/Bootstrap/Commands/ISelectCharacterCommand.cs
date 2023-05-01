using Core.Characters;

namespace Core.Bootstrap.Commands
{
    public interface ISelectCharacterCommand : ICommand
    {
        void Execute(CharacterType characterType);
    }
}