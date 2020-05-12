using System;
using RPG.Core;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat 
{

//Make our own meny
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

/*
This class is used to change the weapon behavior when shifting between different weapons
*/
public class WeaponConfig : ScriptableObject 
{

    [SerializeField] AnimatorOverrideController animatorOverride = null;
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] float weaponDamage = 5f; //Our damage the weapon does
    [SerializeField] float percentageBonus = 0;
    [SerializeField] float weaponRange = 2f; //Our WeaponRange
    [SerializeField] bool isRightHanded = true;
    [SerializeField] Projectile projectile = null;
    [SerializeField] string type = null;
    [SerializeField] int id = 0;
    [SerializeField] string description = null;
    [SerializeField] Sprite weaponIcon;

    //bool pickedup????

    const string weaponName = "Weapon";


    /*
        Place weapon in player hand. Override the animator with new. 
    */
    public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)   
    {
        DestroyOldWeapon(rightHand, leftHand); 
        Weapon weapon = null;

        if(equippedPrefab != null)
            {
                Transform handTransform = GetTransorm(rightHand, leftHand);

                weapon = Instantiate(equippedPrefab, handTransform);
                //animator.runtimeAnimatorController = animatorOverride;
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            //reset the override controller correctly
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
            
    }

        /*Check wether a player carries a weapon. If yes destroy the current weapon when picking up a new weapon*/
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform currentWeapon = rightHand.Find(weaponName);
            if(currentWeapon == null)
            {
                currentWeapon = leftHand.Find(weaponName);
            } 
            if(currentWeapon == null)
            {
                return;
            }

            currentWeapon.name = "Destroyed";   //no confusion of the destroyed and the pickupped weapon 
            Destroy(currentWeapon.gameObject);

        }

        private Transform GetTransorm(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, 
        GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile,
                GetTransorm(rightHand, leftHand).position,
                Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

    public float GetDamage()
    {
        return weaponDamage;
    }
   
    public float GetWeaponRange()
    {
        return weaponRange;
    }

    public float GetPercentageBonus()
    {
        return percentageBonus;
    }




}
}