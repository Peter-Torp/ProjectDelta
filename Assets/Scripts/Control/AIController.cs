using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {

        /*
            Reuse of the fighter methods for simular acts and damage.
        */

        [SerializeField] float chaseDistance = 5f;  //distance in which they attack
        [SerializeField] float suspicionTime = 3f; // 3 seconds wait time until guards goes back to position

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start() 
        {
            fighter = GetComponent<Fighter>();  
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player"); //the tag of the player

            guardPosition = transform.position;
        }

        /*Check if player is in distance. If so attack*/
        private void Update()
        {

            if(InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();

            }
            else
            {
                //fighter.Cancel(); //cancel chase 
                GuardBehaviour(); // Returns a guard to his starting location
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        /*Calculate distance from AI position and player position*/
        private bool InAttackRangeOfPlayer()
        {
           float distanceOfPlayer = Vector3.Distance(player.transform.position, transform.position); //distance between a and b
           return distanceOfPlayer < chaseDistance; //return true if chasedistance is bigger than the distance to the player.
        }

        //called by Unity. Character selected -> Show Gizmo
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);   //position of character + the chase distance
        }



    }



}