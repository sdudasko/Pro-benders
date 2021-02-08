using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float range = 2f;
        [SerializeField] float damage = 5f;

        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public float GetRange()
        {
            return range;
        }

        public float GetDamage()
        {
            return damage;
        }

    }
}