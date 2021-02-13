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
        [SerializeField] float points_of_health = 100f;

        bool is_dead = false;

        private void Start()
        {
            points_of_health = GetComponent<BaseStats>().GetHealth();
        }

        public bool isDead()
        {
            return is_dead;
        }

        public void TakeDamange(float damage_taken)
        {
            points_of_health = Mathf.Max(points_of_health - damage_taken, 0);

            if (!is_dead && points_of_health <= 0)
            {
                Die();
            }
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
    }

}