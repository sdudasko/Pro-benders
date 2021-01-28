using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        public void TakeDamange(float damageTaken)
        {
            health = Mathf.Max(health - damageTaken, 0);
        }
    }

}