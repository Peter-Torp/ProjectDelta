using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;

        NavMeshAgent navMesthAgent;

        private void Start()
        {
            navMesthAgent = GetComponent<NavMeshAgent>();
        }


        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMesthAgent.destination = destination;
            navMesthAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMesthAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMesthAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }


    }
}

