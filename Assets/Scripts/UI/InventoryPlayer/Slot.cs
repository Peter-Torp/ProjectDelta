using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.InventoryPlayer
{
public class Slot : MonoBehaviour
{
    public GameObject slotItem;
    public bool empty; 
    public Sprite icon;
    public string type;
    public int id; 
    public string description;


    public void UpdateSlot()
    {
        this.GetComponent<Image>().sprite = icon;
    }


}

}
