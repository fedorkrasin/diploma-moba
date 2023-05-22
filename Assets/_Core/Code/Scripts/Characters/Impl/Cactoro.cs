using System;
using System.Collections;
using Core.Player.Systems;
using UnityEngine;

namespace Core.Characters.Impl
{
    public class Cactoro : Character
    {
        [SerializeField] private Collider _attackCollider;
        
        public override void ExecuteAttack()
        {
            Debug.Log("тычка тимбера");
            // transform.position += transform.forward * 4; // TODO: ninja attack
            StartCoroutine(Attack());

            IEnumerator Attack()
            {
                _attackCollider.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                _attackCollider.gameObject.SetActive(false);
            }
        }

        public override void ExecuteUseSpell()
        {
            Debug.Log("Chainsaw");
        }

        public override void ExecuteUseUltimate()
        {
            Debug.Log("Chakram");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<HealthSystem>(out var enemy))
            {
                enemy.Damage(20);
            }
        }
    }
}