using UnityEngine;

namespace Core.Characters.Impl
{
    public class Timbersaw : ICharacter
    {
        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void UseSpell()
        {
            Debug.Log(this);
        }

        public void UseUltimate()
        {
            throw new System.NotImplementedException();
        }
    }
}