using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        public Transform target = null;
        [SerializeField] float weaponRange = 2f;

        public void Update()
        {
            if (target == null) return;

            if (!GetIsInRange()) GetComponent<Mover>().MoveTo(target.position);
            else GetComponent<Mover>().Cancel();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            print("Cancelling");
            target = null;
        }
    }
}
