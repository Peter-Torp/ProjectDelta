using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 20)] // is our level range
        [SerializeField] int startinglevel = 1; // our starting level
        [SerializeField] CharacterClass characterClass; // calls which class we are from characterclass
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticalEffect = null;
        [SerializeField] bool shouldUseModifiers = false;
        [SerializeField] UnityEvent levelUp;

        public event Action onLevelUp;

        LazyValue<int> currentLevel;

        Experience experience;

        private void Awake() 
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLvl);
        }

        private void Start() 
        {
            currentLevel.ForceInit();

        }

        private void OnEnable() 
        {
            if (experience != null)
            {
                experience.expGainedDoStuff += UpdateLevel; //not calling method, adding to the list of the action 
            }
        }

        private void OnDisable() 
        {
            if (experience != null)
            {
                experience.expGainedDoStuff -= UpdateLevel;  
            }
        }
        
        
        private void UpdateLevel() 
        {
            int newLvl = CalculateLvl();
            if(newLvl > currentLevel.value)
            {
                currentLevel.value = newLvl;
                LevelUpEffect();
                onLevelUp();
            }   
             
        }

        private void LevelUpEffect()
        {
            //play sound
            levelUp.Invoke();
            Instantiate(levelUpParticalEffect, transform);
        }


        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
            //divide by 100 to get the fraction
            // +1 cause we want the fraction
            //so essentially we multiply by 1.1
        }


        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        //Adds the flat weapon damage ontop of the base damage in progression of unarmed
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;        
            }

        /*Check if the player have enough xp to level up. If not return currentLevel*/
        private int CalculateLvl()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startinglevel;    //return starting level if xp = null. Fast exit. 
            
            float currentXP = experience.GetPoints();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for(int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level); 
                if(currentXP < XPToLevelUp )
                {

                    return level;
                }
                
                 
            }
            

            return penultimateLevel + 1;
        }

    
    }

    
}
