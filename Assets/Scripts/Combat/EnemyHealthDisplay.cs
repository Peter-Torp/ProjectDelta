using RPG.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter; 

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>(); // Storing fighter value in our Awake method

        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A"; //Prints the text for our Enemy health
                return;
            }
            Health health = fighter.GetTarget(); // gives us access to our health component

            GetComponent<Text>().text = String.Format("{0:0.0}%", health.GetPercentage()); //puts the text with 1 decimal

        }
    }
}
