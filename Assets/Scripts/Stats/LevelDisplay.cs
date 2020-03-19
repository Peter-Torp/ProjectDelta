using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();

        }


        //Display as no decimal and in string format
        private void Update()
        {
            GetComponent<Text>().text = String.Format ("{0:0}", baseStats.GetLevel());

        }
    }
}
