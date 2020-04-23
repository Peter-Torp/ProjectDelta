using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f; //Time between our fighters attacks
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;  //which weapon?

        Health target;   //We get access to anything we've put in Health, we do this because we know ALL enemies has health

        float timeSinceLastAttack = Mathf.Infinity; //last attack happened a long time ago
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private void Awake() 
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            //AttachWeapon(defaultWeapon);
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {   
            //currentWeaponConfig.ForceInit(); //call this or we wont have a visible weapon
            currentWeapon.ForceInit();
        }

        //Update is ran on every frame
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;  //make sure we can attack as soon the game starts

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);    //1f = fullspeed
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>(); 
            Debug.Log(animator);
            Debug.Log(weapon.Spawn(rightHandTransform, leftHandTransform, animator));
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);

            
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

           // float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, 
                damage);
            }
            else
            {
                //The damage we do to our target is equal to our weapon damage
                target.TakeDamage(gameObject, damage);
                 
                
                //target.TakeDamage(gameObject, currentWeapon.GetDamage());
            }
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            //We calculate the distance between our figther and target from the fighters and targets position and asks if we are in range of our target through weaponRange
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
           
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform))
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage(); // Changes our damage to the damage that our current weapon has equipped

            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus(); 

            }        
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);

        }


    }
}