using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
{ 
    
    [SerializeField] float speed = 1;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null; 
    [SerializeField] float maxLifeTime = 5f;
    [SerializeField] GameObject[] destoyOnHit = null;
    [SerializeField] float lifeAfterImpact = 2; //maybe some special effect
    Health target = null;
    GameObject instigator = null;
    float damage = 0;
    [SerializeField] UnityEvent onHit;
    //arrow launch sound will be invoke when projectile is created.

    private void Start() 
    {
        transform.LookAt(GetAimLocation()); //not homing projectile
    }

    void Update()
    {
        if (target == null) return;

        if(isHoming && !target.IsDead())
        {
        transform.LookAt(GetAimLocation()); //to look at a target/homing projectile
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime); //to move to our target
    }

    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        this.target = target;
        this.damage = damage;
            this.instigator = instigator;

        //stop moving on forever
        Destroy(gameObject, maxLifeTime);
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
        if(target.IsDead()) return; //continue projectile path if target is dead
        target.TakeDamage(instigator, damage);

        //prop
        speed = 0;

        onHit.Invoke();

        if(hitEffect != null)
        {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        } 

        foreach(GameObject toDestroy in destoyOnHit)
        {
            Destroy(toDestroy);
        }

        Destroy(gameObject, lifeAfterImpact);
        
        
    }
  }
}