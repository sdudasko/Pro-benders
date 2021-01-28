using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 0.666f;

        Health target = null;
        float timeSinceLastAttack = 0;

        [SerializeField] float weaponDamage = 5;

        public void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.isDead()) return;

            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // This triggers Hit()
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        void Hit() // Called from Unity
        {
            if (target) target.TakeDamange(weaponDamage);
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) { return false; }

            Health health = combatTarget.GetComponent<Health>();

            if (health != null && !combatTarget.GetComponent<Health>().isDead()) return true;

            return false;
        }
    }

}
