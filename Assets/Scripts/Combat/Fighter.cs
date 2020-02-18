using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;    //distance to target.
        [SerializeField] float timeBetweenAttacks = 1f;        //throttling attack
        [SerializeField] float weaponDamage = 5f;


        Transform target;
        float timeSinceLastAttack = 0;  //figure out when player attacked the last time


        private void Update()       //always updating -> Monobehavior
        {   
            timeSinceLastAttack += Time.deltaTime; 

            if (target == null) return;

            /*If the target is not null move to the target*/
            if (target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior(); //if close to target -> attack
            }
        }

        private void AttackBehavior()
        {
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
                //this will trigger Hit event
            }
              

        }

        //animation event
        void Hit() 
        {
            Health healthComponent = target.GetComponent<Health>(); 
            healthComponent.TakeDamage(weaponDamage);
        }


        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange; //if weaponragne is higher than 2f -> moveto
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;    //transform = position of target.
        }




        public void Cancel()    //cancel target and end combat. 
        {
            target = null;
        }


        

        
    }
}
