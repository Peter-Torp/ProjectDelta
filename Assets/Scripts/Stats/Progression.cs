using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression"
    , order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;  

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {

            BuildLookUpTable();

            float[] levels = lookupTable[characterClass][stat];

            //return to default
            if(levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
            
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {

            BuildLookUpTable();

            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUpTable()
        {
            if(lookupTable != null) return; 

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>(); 

                foreach(ProgressionStat progressionStat in progressionClass.stat)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                
                //THE final lookuptable
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stat;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}

