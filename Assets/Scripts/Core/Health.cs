using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{

    public class Health : MonoBehaviour
    {
        [SerializeField] float points_of_health = 100f;

        bool is_dead = false;

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
    }

}