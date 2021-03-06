using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {

        /*
            Reuse of the fighter methods for simular acts and damage.
        */

        [SerializeField] float chaseDistance = 5f;  //distance in which they attack
        [SerializeField] float suspicionTime = 3f; // 3 seconds wait time until guards goes back to position
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float wayPointDwellTime = 4f;
        
        [Range(0,1)]    //be between 0 and 1 (patrolSpeedFraction)
        [SerializeField] float patrolSpeedFraction = 0.2f;  //the patrol speed
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWayPoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWayPointIndex = 0;

        private void Awake() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player"); //the tag of the player

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start() 
        {
            guardPosition.ForceInit();
        }

        /*Check if player is in distance. If so attack*/
        private void Update()
        {

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();

            }
            else
            {
                //fighter.Cancel(); //cancel chase 
                PatrolBehavior(); // Returns a guard to his starting location
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            /*update attributes*/
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWayPoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition.value;
            if(patrolPath != null)
            {
                if(AtWayPoint())
                {
                    timeSinceArrivedAtWayPoint = 0f;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            /*move to another waypoint*/
            if(timeSinceArrivedAtWayPoint > wayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);    
            }
            
        }

        /*whats the current waypoint character is at?*/
        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(currentWayPointIndex); 
        }

        /*Cycle the path - Need a waypoint 0 to work -> cannot start from 1 index*/
        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        /**/
        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        /*What to do when the enemy attack*/
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
           RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

           foreach (RaycastHit hit in hits)
           {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.Aggrevate();
           }
        }

        /*Calculate distance from AI position and player position*/
        private bool IsAggrevated()
        {
           float distanceOfPlayer = Vector3.Distance(player.transform.position, transform.position); //distance between a and b
           return distanceOfPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime; //return true if chasedistance is bigger than the distance to the player.
        }

        //called by Unity. Character selected -> Show Gizmo
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);   //position of character + the chase distance
        }



    }



}