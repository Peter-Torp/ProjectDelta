using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 20)] // is our level range
        [SerializeField] int startinglevel = 1; // our starting level
        [SerializeField] CharacterClass characterClass; // calls which class we are from characterclass
        [SerializeField] Progression progression = null;
        
        private void Update() 
        {
            if(gameObject.tag == "Player")
            {   
                print(GetLevel());
            }   
             
        }
        public float GetStat(Stat stat)
        {
        return progression.GetStat(stat, characterClass, startinglevel);
        }

        /*Check if the player have enough xp to level up. If not return currentLevel*/
        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>(); 
            float currentXP = experience.GetPoints();

            if(experience == null) return startinglevel;    //return starting level if xp = null. Fast exit. 

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
