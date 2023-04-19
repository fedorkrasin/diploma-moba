using UnityEngine;

namespace Core.Characters.Data
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Characters/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public CharacterType Type;
        [field: SerializeField] public Character CharacterPrefab;

        public string Name => Type.ToString();
    }
}