using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Inventory
{
public class Slot : MonoBehaviour
{
    public GameObject slotItem;
    public bool empty; 
    public Texture2D icon;
    public string type;
    public int id;
    public string description;
}

}
