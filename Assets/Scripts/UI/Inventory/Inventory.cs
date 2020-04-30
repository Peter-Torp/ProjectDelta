using UnityEngine; 

namespace RPG.Inventory
{

public class Inventory : MonoBehaviour 
{
    

    private bool inventoryEnabled = false;
    public GameObject inventory = null;     //to enable/disable inventory
    private int allSlots;
    private int enabledSlots;
    private GameObject[] slot;
    public GameObject slotHolder = null;


    private void Start() 
    {
        allSlots = 40;
        slot = new GameObject[allSlots];

        for(int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;      //instantiate slots
        }
    }

    private void Update() 
    {

        //Switch inventory on/off with i key
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
        }    
        if(inventoryEnabled == true)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }



    




}

}