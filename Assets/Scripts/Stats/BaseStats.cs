using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 999)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUserModifiers = false;

        public event Action OnLevelUp;

        int currentLevel = 0;
        Health health = null;

        public void Start()
        {
            currentLevel = CalculateLevel();

            Experience experience = GetComponent<Experience>();
            health = GetComponent<Health>();

            if (experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100); //110% if 10% modifier
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                OnLevelUp();

            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUserModifiers) return 0;

            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            float total = 0;

            foreach(IModifierProvider provider in providers)
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUserModifiers) return 0;

            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            float totalPercentage = 0;

            foreach(IModifierProvider provider in providers)
            {
                foreach(float modifier in provider.GetPercentageModifiers(stat))
                {
                    totalPercentage += modifier;
                }
            }
            return totalPercentage;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level < penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel;
        }
    }
}