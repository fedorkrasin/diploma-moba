using UnityEngine;

namespace Core.Characters.Data
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "Characters/SpellData")]
    public class SpellData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private float _manaCost;
        [SerializeField] private float _cooldown;
        
        public string Name => _name;
        public float ManaCost => _manaCost;
        public float Cooldown => _cooldown;
    }
}