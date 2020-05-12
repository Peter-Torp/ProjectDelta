using UnityEngine; 

namespace RPG.InventoryPlayer
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
            //instantiate slots
            slot[i] = slotHolder.transform.GetChild(i).gameObject;      
 
            //check if slot is empty
            if(slot[i].GetComponent<Slot>() == null)
            slot[i].GetComponent<Slot>().empty = true;
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


    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject; 
            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.id, item.type, item.description, item.icon);  
        }    
    }


    /*
        Add item to inventory and check if the slot can hold it or check the next one. 
        Slot attributes is assigned as the item attributes.
        Item is then inactive until used and the slot is now set to not empty.
    */
    void AddItem(GameObject item, int itemId, string itemType, string itemDescription, Sprite itemIcon)
    {
        //Check slot for items before adding 
        for(int i = 0; i < allSlots; i++)
        {
            if(slot[i].GetComponent<Slot>().empty)
            {
                //Add item to list
                item.GetComponent<Item>().pickedUp = true;
                
                slot[i].GetComponent<Slot>().slotItem = item;
                slot[i].GetComponent<Slot>().icon = itemIcon;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().id = itemId;
                slot[i].GetComponent<Slot>().description = itemDescription; 

                item.transform.parent = slot[i].transform;  
                item.SetActive(false); //disable until equiped

                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false; 

            }

            return;
        }
    }


    




}

}