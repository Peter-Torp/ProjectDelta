using UnityEngine;

public class Health : MonoBehaviour {
    
    [SerializeField] float health = 100f;


    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0); //health cannot go below zero. Health - damage or 0.
        print(health); 
    }


}