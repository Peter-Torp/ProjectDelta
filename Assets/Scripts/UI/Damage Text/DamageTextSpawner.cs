using UnityEngine;


namespace RPG.UI.DamageText
{
public class DamageTextSpawner : MonoBehaviour 
{

    [SerializeField] DamageText damgeTextPrefab = null;

    private void Start() 
    {
        Spawn(11);
    }

    public void Spawn(float damageAmount)
    {
        DamageText instance = Instantiate<DamageText>(damgeTextPrefab, transform);
    
    
    }







}





}