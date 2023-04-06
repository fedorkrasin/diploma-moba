using Core.Characters.Data;
using UnityEngine;

namespace Core.Characters
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private CharacterSpells _spells;

        public CharacterSpells Spells => _spells;
        
        public abstract void Attack();
        public abstract void UseSpell();
        public abstract void UseUltimate();
    }
}