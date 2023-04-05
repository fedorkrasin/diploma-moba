using UnityEngine;

namespace Core.Characters.Impl
{
    public class Pudge : ICharacter
    {
        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void UseSpell()
        {
            Debug.Log("PUDGE!");
        }

        public void UseUltimate()
        {
            throw new System.NotImplementedException();
        }
    }
}