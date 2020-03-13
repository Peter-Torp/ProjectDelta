using RPG.Core;
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


    public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
        if(equippedPrefab != null)
            {
                Transform handTransform = GetTransorm(rightHand, leftHand);

                Instantiate(equippedPrefab, handTransform);
                animator.runtimeAnimatorController = animatorOverride;
            }
            if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
        }
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