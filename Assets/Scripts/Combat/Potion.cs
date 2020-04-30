using UnityEngine;
using RPG.Attributes;

namespace RPG.Potion
{

public class Potion : MonoBehaviour {

    GameObject potion;
    float hpToHeal = 0.0f;
    bool healthPotion;

    void PickUp()
    {
        Health health = GameObject.FindWithTag("Player").GetComponent<Health>();

        if(potion == healthPotion)
        {
            
        }
        Destroy(potion);
    }


}
}