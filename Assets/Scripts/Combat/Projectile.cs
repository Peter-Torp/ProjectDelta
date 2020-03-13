using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ 
    
    [SerializeField] float speed = 1;
    Health target = null;
    float damage = 0;

    void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation()); //to look at a target
        transform.Translate(Vector3.forward * speed * Time.deltaTime); //to move to our target
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
