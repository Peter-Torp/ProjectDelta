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
        [SerializeField] float maxSpeed = 6f;

        /*navmesh = where the character can go. Part of Unity AI*/
        NavMeshAgent navMeshAgent;
        Health health;

        /*get the navmesh component*/
        private void Start() {
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
    }
}