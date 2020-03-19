using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
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
