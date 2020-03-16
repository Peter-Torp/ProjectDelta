using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f; //Sets our Hp to 100

        bool isDead = false;

        //bug = enemy is reset sometimes + circular dependency
        private void Start() 
        { 
             healthPoints = GetComponent<BaseStats>().GetHealth();   
        }

        //Checks if our target or figther is dead or not
        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage(float damage)
        {
            //Calculates the damage we take and makes it stop at 0 so it doesnt drop in minus hp
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            //If our hitpoints reaches 0 well then we die
            if (healthPoints == 0)
            {
                Die();
            }

        }

        public void Die()
        {
            if (isDead) return;

            isDead = true;
            //Here we pull our die animation from our Animator
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();  //cancel action when dead
        }


        //Save-----------------------------------------------------


        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            //kill off character if health = 0
            if(healthPoints == 0)
            {
                Die();
            }
        }
    }
}