using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
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