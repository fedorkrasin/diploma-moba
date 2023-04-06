using UnityEngine;

namespace Core.Characters.Impl
{
    public class Pudge : Character
    {
        public override void Attack()
        {
            Debug.Log("тычка пуджа");
        }

        public override void UseSpell()
        {
            Debug.Log("Meat hook)");
        }

        public override void UseUltimate()
        {
            Debug.Log("FResh meat!!");
        }
    }
}