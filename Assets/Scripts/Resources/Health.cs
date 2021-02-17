using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;

        float points_of_health = -1f;
        
        bool is_dead = false;

        private void Start()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
            if (points_of_health < 0)
            {
                points_of_health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }


        public bool isDead()
        {
            return is_dead;
        }

        public void TakeDamange(GameObject instigator, float damage_taken)
        {
            points_of_health = Mathf.Max(points_of_health - damage_taken, 0);

            if (!is_dead && points_of_health <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return points_of_health;
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
            return 100 * (points_of_health / GetComponent<BaseStats>().GetStat(Stat.Health));
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
            return points_of_health;
        }

        public void RestoreState(object state)
        {
            points_of_health = (float)state;

            if (points_of_health == 0)
            {
                Die();
            }

        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage / 100;

            points_of_health = Mathf.Max(points_of_health, regenHealthPoints);
        }
    }

}