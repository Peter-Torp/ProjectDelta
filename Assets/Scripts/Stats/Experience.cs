using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        //Actions
        public event Action expGainedDoStuff;       //= predefined delegate




        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            expGainedDoStuff();
        }

        public float GetPoints()
        {
            return experiencePoints;
        }


        /*--------Save methods---------*/
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;   
        }

    }
}
