using Core.Player.Stats;
using UnityEngine;

namespace Core.Player
{
    public class SpellSystem
    {
        private readonly PlayerSpellsList _spellsList;
        
        public SpellSystem(PlayerSpellsList spellsList)
        {
            _spellsList = spellsList;
        }

        public void UseSpell()
        {
            Debug.Log("Spell: " + _spellsList.Simple.Name);
        }

        public void UseUltimate()
        {
            Debug.Log("Ultimate: " + _spellsList.Ultimate.Name);
        }
    }
}