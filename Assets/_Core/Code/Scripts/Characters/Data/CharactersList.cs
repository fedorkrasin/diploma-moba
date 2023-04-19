using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Characters.Data
{
    [CreateAssetMenu(fileName = "CharactersList", menuName = "Characters/CharactersList", order = 0)]
    public class CharactersList : ScriptableObject
    {
        [field: SerializeField] public List<CharacterData> CharactersData;

        public CharacterData Get(CharacterType type)
        {
            return type switch
            {
                CharacterType.Cactoro => CharactersData.Find(data => data.Type == CharacterType.Cactoro),
                CharacterType.Fish => CharactersData.Find(data => data.Type == CharacterType.Fish),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}