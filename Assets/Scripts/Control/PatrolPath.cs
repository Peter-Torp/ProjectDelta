using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        /*
            Patrolpath exists only in the scene/sandbox. There it has to be dragged from there to the character AI controller.
        */

        const float waypointGizmoRadius = 0.3f; //gives our spheres a radius

        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i); //Counts up in waypoints

                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius); //Draws spheres for our waypoints in our path
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j)); //Draws a line between our spheres
            }
        }

        //Shows our paths in our waypoints
        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
