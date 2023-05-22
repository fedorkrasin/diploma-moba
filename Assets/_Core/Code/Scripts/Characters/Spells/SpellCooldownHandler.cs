using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Core.Characters.Spells
{
    public class SpellCooldownHandler : NetworkBehaviour
    {
        private List<SpellCooldownState> _spellsOnCooldown = new();
        
        public event Action<SpellCooldownState> SpellCooldownStarted = delegate { };

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            
            _spellsOnCooldown = new List<SpellCooldownState>();
        }

        private void Update()
        {
            if (!IsOwner) return;

            for (var i = _spellsOnCooldown.Count - 1; i >= 0; i--)
            {
                if (NetworkManager.LocalTime.TimeAsFloat >= _spellsOnCooldown[i].SpellReadyTime)
                {
                    _spellsOnCooldown.RemoveAt(i);
                }
            }
        }

        public bool IsOnCooldown(int spellId)
        {
            for (var i = 0; i < _spellsOnCooldown.Count; i++)
            {
                if (_spellsOnCooldown[i].SpellId == spellId)
                {
                    return true;
                }
            }

            return false;
        }

        public SpellCooldownState GetCooldownState(int spellId)
        {
            return _spellsOnCooldown.Find(spell => spell.SpellId == spellId);
        }
    }
}