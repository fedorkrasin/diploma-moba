using Core.Player;
using UnityEngine;

namespace Core.Characters.Data
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Characters/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public int Id;
        [field: SerializeField] public CharacterType Type;
        [field: SerializeField] public PlayerBehaviour CharacterPrefab;
        [field: SerializeField] public Sprite Icon;

        public string Name => Type.ToString();
    }
}