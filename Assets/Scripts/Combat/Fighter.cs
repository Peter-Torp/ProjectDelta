using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f; //Time between our fighters attacks
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;  //which weapon?

        Health target;   //We get access to anything we've put in Health, we do this because we know ALL enemies has health

        float timeSinceLastAttack = Mathf.Infinity; //last attack happened a long time ago
        Weapon currentWeapon = null;

        private void Start()
        {   
            if (currentWeapon == null)
            {
                //look for a resource with this type
                EquipWeapon(defaultWeapon);
            }
        }

        //Update is ran on every frame
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;  //make sure we can attack as soon the game starts

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);    //1f = fullspeed
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget() //Just finds out target so we can display our enemies health display
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform); //Makes our figther look straight at the enemy when fighting it
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        /*Bug solved: attack animation is canceled when attack is stopped*/
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null)
            {
                return;
            }

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            }
            else
            {
                //The damage we do to our target is equal to our weapon damage
                target.TakeDamage(gameObject, currentWeapon.GetDamage());
            }
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            //We calculate the distance between our figther and target from the fighters and targets position and asks if we are in range of our target through weaponRange
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetWeaponRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>(); //Gets health from our combatTarget
            return targetToTest != null && !targetToTest.IsDead(); //Checks if our target is not null and is not dead
        }

        public void Attack(GameObject combatTarget)
        {
            //We use our StartAction from our ActionScheduler, which is used to say if we cancel or start an action
            GetComponent<ActionScheduler>().StartAction(this);
            //Sees the health of our combatTarget
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            //We use our stopAttack trigger from our Animator
            StopAttack();
            //Then puts our target to null
            target = null;
        }

        /*Stop the attack when not in combat.
        Bug solved: reset trigger so stop attack attribute is not on when attacking again
        */
        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);

        }
    }
}