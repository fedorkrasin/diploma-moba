using System;
using Core.Characters.Impl;

namespace Core.Characters
{
    public static class CharacterConstructor
    {
        public static ICharacter Get(Character character)
        {
            return character switch
            {
                Character.Pudge => new Pudge(),
                Character.Timbersaw => new Timbersaw(),
                _ => throw new ArgumentOutOfRangeException(nameof(character), character, null)
            };
        }
    }
}