using UnityEngine;


namespace RPG.UI.DamageText
{
public class DamageTextSpawner : MonoBehaviour 
{

    [SerializeField] DamageText damgeTextPrefab = null;
       

    public void Spawn(float damageAmount)
    {
        
        DamageText instance = Instantiate<DamageText>(damgeTextPrefab, transform);
        

        //Debug.Log(damageAmount);
        instance.SetValue(damageAmount);
    
    
    }







}





}