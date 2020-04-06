using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40f;

        /*navmesh = where the character can go. Part of Unity AI*/
        NavMeshAgent navMeshAgent;
        Health health;

        /*get the navmesh component*/
        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        /*update the mover every frame - method is called: UpdateAnimator()*/
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();    //disable navmesh agent is player is dead
            UpdateAnimator();
        }

        /*ActionScheduler method call - dertermine action is active. Call moveTo method*/
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            //Navmesh is a class
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            //return true if found
            if (!hasPath) return false;
            //Makes it so we cant walk ontop of roofs = navmesh is not connected
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        /*navmesh is clicked and destination is saved.*/
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);   //have to be between 0 and 1. 
            navMeshAgent.isStopped = false;
        }

        /*cancel movement*/
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        /*move forward with prefab velocity. Is updated everyframe*/
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            //corners = vector3 corners in navmesh calculation
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i],
                    path.corners[i + 1]);
            }

            return 0;
        }

        //add more states if necessary
        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }


        /*object = all C# objects.
        Have to be marked as serializable*/
        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }
        
        /*Restore the state of the character. Incl position and the last rotation angle*/
        /*Stop the navmeshagent from manipulating poistion during restore*/
        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;   
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}