using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {   //switch between movement and combat
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            //print("Nothing to do");
        }

        /*When clicking on a target -> Attack*/
        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();   //get the taget component when clicked on
                if (target == null) continue;

                if (Input.GetMouseButtonDown(0))    //left MB
                {   //get the figter object and attack a target.
                    GetComponent<Fighter>().Attack(target);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast((Ray)GetMouseRay(), out hit);

            if (hasHit)
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;       //if player reaches edge of the world. -> Print("Nothing to do")
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
