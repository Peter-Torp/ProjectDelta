using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
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
        private int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
