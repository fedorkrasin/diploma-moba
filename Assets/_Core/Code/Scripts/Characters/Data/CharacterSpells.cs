using UnityEngine;

namespace Core.Characters.Data
{
    [CreateAssetMenu(fileName = "CharacterSpells", menuName = "Characters/CharacterSpells")]
    public class CharacterSpells : ScriptableObject
    {
        [SerializeField] private SpellData _simple;
        [SerializeField] private SpellData _ultimate;

        public SpellData Simple => _simple;
        public SpellData Ultimate => _ultimate;
    }
}