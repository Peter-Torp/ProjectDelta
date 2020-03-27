using UnityEngine;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{

    [RequireComponent(typeof(Health))] // Binds Health Component to CombatTarget. Requires this attribute to exist. 
   
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false; //continues within our foreach loop
            }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);  //attack if in range
            }
            return true;
        }
    }
}