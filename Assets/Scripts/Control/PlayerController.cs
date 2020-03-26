using System;
using RPG.Combat;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        enum CursorType
        {
            None, 
            Movement, 
            Combat
        }

        [System.Serializable]
        struct cursorMapping
        {
            public CursorType type; 

        }

        [SerializeField] cursorMapping[] cursorMappings = null;

        private void Awake() 
        {
            health = GetComponent<Health>();    
        }

        private void Update()   //update every frame
        {
            if(health.IsDead()) return; 
            
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                
                if(target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue; //continues within our foreach loop
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);  //attack if in range
                }
                SetCursor(CursorType.Combat); 

                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);  
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);   //move to where the mouse is clicked
                }
                SetCursor(CursorType.Movement);
                
                return true;
                
            }
            return false;
        }

        /*Set the cursor to different events*/
        private void SetCursor(CursorType type)
        {
            cursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode .Auto);
        }

        private cursorMapping GetCursorMapping(CursorType type)
        {

        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);   //return the value where the mouse is clicked through camera
        }
    }
}