using System;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 0.666f;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;

        [SerializeField] Weapon defaultWeapon = null;

        Health target = null;
        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon = null;


        public void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SeupDefaultWeapon);

        }

        private Weapon SeupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);

            return defaultWeapon;
        }

        public void Start()
        {
            currentWeapon.ForceInit();
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

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

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // This triggers Hit()
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetIsInRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        void Hit() // Called from Unity
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            print("Damage dealth: " + damage);

            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            } else
            {
                target.TakeDamange(gameObject, damage);
            }

        }

        void Shoot() // Called from Unity
        {
            Hit();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }

            Health health = combatTarget.GetComponent<Health>();

            if (health != null && !combatTarget.GetComponent<Health>().isDead()) return true;

            return false;
        }

        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = UnityEngine.Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }
    }

}
