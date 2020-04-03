using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenrationPercentage = 100; //Sets our regen percentage
        [SerializeField] TakeDamageEvent takeDamage; //Is our unity event


        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> //subclass
        {
            //A unity event cant use a float generic through a serializefield
        }

        LazyValue<float> healthPoints; //Sets our Hp to 100

        bool isDead = false;

        private void Awake() //We're initializing lazyvalues, not calling the method just using it
        {
        healthPoints = new LazyValue<float>(GetInitialHealth);
        }
    
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() 
        {
            healthPoints.ForceInit();
        }
        
        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        //Checks if our target or figther is dead or not
        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage " + damage);

            //Calculates the damage we take and makes it stop at 0 so it doesnt drop in minus hp
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            //If our hitpoints reaches 0 well then we die
            if (healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage); // unity event is called
            }

        }


        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health); 
        }


        public float GetPercentage()
        {
            //Here we do the calculation of our percentage health
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public void Die()
        {
            if (isDead) return;

            isDead = true;
            //Here we pull our die animation from our Animator
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();  //cancel action when dead
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>(); //Storing the experience
            if (experience == null) return; // if its null just return

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward)); //Otherwise we gain experience points
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenrationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }


        //Save-----------------------------------------------------


        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            //kill off character if health = 0
            if(healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}