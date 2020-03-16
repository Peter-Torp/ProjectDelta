using System;
using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat 
{

//Make our own meny
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]

/*
This class is used to change the weapon behavior when shifting between different weapons
*/
public class Weapon : ScriptableObject 
{

    [SerializeField] AnimatorOverrideController animatorOverride = null;
    [SerializeField] GameObject equippedPrefab = null;
    [SerializeField] float weaponDamage = 5f; //Our damage the weapon does
    [SerializeField] float weaponRange = 2f; //Our WeaponRange
    [SerializeField] bool isRightHanded = true;
    [SerializeField] Projectile projectile = null;

    const string weaponName = "Weapon";


    public void Spawn(Transform rightHand, Transform leftHand, Animator animator)   
    {
        DestroyOldWeapon(rightHand, leftHand); 

        if(equippedPrefab != null)
            {
                Transform handTransform = GetTransorm(rightHand, leftHand);

                Instantiate(equippedPrefab, handTransform);
                animator.runtimeAnimatorController = animatorOverride;
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile,
                GetTransorm(rightHand, leftHand).position,
                Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }

    public float GetDamage()
    {
        return weaponDamage;
    }
    public float GetWeaponRange()
    {
        return weaponRange;
    }




}
}