using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();

        }


        //Display as no decimal and in string format
        private void Update()
        {
            GetComponent<Text>().text = String.Format ("{0:0}", experience.GetPoints());

        }
    }
}
