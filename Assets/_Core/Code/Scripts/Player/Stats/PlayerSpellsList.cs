using Core.Player.Data;
using UnityEngine;

namespace Core.Player.Stats
{
    [CreateAssetMenu(fileName = "PlayerSpellsList", menuName = "Player/Spells/SpellsList")]
    public class PlayerSpellsList : ScriptableObject
    {
        [SerializeField] private SpellData _simple;
        [SerializeField] private SpellData _ultimate;

        public SpellData Simple => _simple;
        public SpellData Ultimate => _ultimate;
    }
}