using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        public TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        LazyValue<float> points_of_health;
        
        bool is_dead = false;

        private void Awake()
        {
            points_of_health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            points_of_health.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        public bool isDead()
        {
            return is_dead;
        }

        public void TakeDamange(GameObject instigator, float damage_taken)
        {
            points_of_health.value = Mathf.Max(points_of_health.value - damage_taken, 0);

            takeDamage.Invoke(damage_taken);

            if (!is_dead && points_of_health.value <= 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }

        public void Heal(float healthToRestore)
        {
            points_of_health.value = Mathf.Min(points_of_health.value + healthToRestore, GetMaxHealthPoints());
        }

        public float GetHealthPoints()
        {
            return points_of_health.value;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        internal float GetFraction()
        {
            return points_of_health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (is_dead) return;

            is_dead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return points_of_health.value;
        }

        public void RestoreState(object state)
        {
            points_of_health.value = (float)state;

            if (points_of_health.value == 0)
            {
                Die();
            }

        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage / 100;

            points_of_health.value = Mathf.Max(points_of_health.value, regenHealthPoints);
        }
    }

}