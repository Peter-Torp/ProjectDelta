using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;

        /*navmesh = where the character can go. Part of Unity AI*/
        NavMeshAgent navMeshAgent;

        /*get the navmesh component*/
        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        /*update the mover every frame - method is called: UpdateAnimator()*/
        void Update()
        {
            UpdateAnimator();
        }

        /*ActionScheduler method call - dertermine action is active. Call moveTo method*/
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        /*navmesh is clicked and destination is saved.*/
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
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
    }
}