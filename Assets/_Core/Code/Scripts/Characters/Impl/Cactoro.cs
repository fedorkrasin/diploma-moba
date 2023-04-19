using UnityEngine;

namespace Core.Characters.Impl
{
    public class Cactoro : Character
    {
        public override void Attack()
        {
            Debug.Log("тычка тимбера");
        }

        public override void UseSpell()
        {
            Debug.Log("Chainsaw");
        }

        public override void UseUltimate()
        {
            Debug.Log("Chakram");
        }
    }
}