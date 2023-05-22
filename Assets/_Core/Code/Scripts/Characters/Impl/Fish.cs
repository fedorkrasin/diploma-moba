using UnityEngine;

namespace Core.Characters.Impl
{
    public class Fish : Character
    {
        public override void ExecuteAttack()
        {
            Debug.Log("тычка пуджа");
        }

        public override void ExecuteUseSpell()
        {
            Debug.Log("Meat hook)");
        }

        public override void ExecuteUseUltimate()
        {
            Debug.Log("FResh meat!!");
        }
    }
}