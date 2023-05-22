using Core.Characters.Data;
using Unity.Netcode;
using UnityEngine;

namespace Core.Characters
{
    public abstract class Character : NetworkBehaviour
    {
        [SerializeField] private CharacterSpells _spells;

        public CharacterSpells Spells => _spells;
        
        public abstract void ExecuteAttack();
        public abstract void ExecuteUseSpell();
        public abstract void ExecuteUseUltimate();

        public void Attack()
        {
            if (!IsOwner) return;
            RequestAttackServerRpc();
            ExecuteAttack();
        }

        public void UseSpell()
        {
            
        }

        public void UseUltimate()
        {
            
        }

        [ServerRpc]
        private void RequestAttackServerRpc()
        {
            AttackClientRpc();
        }

        [ClientRpc]
        private void AttackClientRpc()
        {
            if (!IsOwner) Attack();
        }
    }
}