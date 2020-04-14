using System;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float raycastRadius = 1f;
       
        private void Awake() 
        {
            health = GetComponent<Health>();    
        }

        private void Update()   //update every frame
        {
            /*
                But with interact with UI!!!!!*******
                //if (InteractWithUI()) return;
            */

            if(health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }


        /*Bug*/
        private bool InteractWithUI()
        {
            //Debug.Log("Ye");
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                    
                }

            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            //Get all hits
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            //sort by distance 
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            //sort the hits
            Array.Sort(distances, hits);
            //return
            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
           bool hasHit = RaycastNavMesh(out target); //Raycasting to the navmesh
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false; //cannot interract with movement if we cant interract with target

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);   //move to where the mouse is clicked
                }
                SetCursor(CursorType.Movement);
                
                return true;
                
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            //Raycast to terrain
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            //finding nearest navmesh point
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, 
                maxNavMeshProjectionDistance,
                NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            
            target = navMeshHit.position;
            
            return true;
        }

        /*Set the cursor to different events*/
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);   //return the value where the mouse is clicked through camera
        }
    }
}