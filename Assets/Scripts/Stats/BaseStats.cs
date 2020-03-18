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
    
        public float GetHealth()
        {
        return progression.GetHealth(characterClass, startinglevel);
        }
    
        public float GetExperienceReward()
        {
            return 10;
        }
    
    }

    
}
