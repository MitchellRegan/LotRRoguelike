using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    //The ID that lets the inventory know when to stack items
    public string itemNameID = "Item";

    //The icon that is displayed for this item in inventory slots
    public Sprite icon;

    //The maximum amount of this item that can fit in one inventory slot
    public uint maxStackSize = 1;

    //The current amount in this item stack
    public uint currentStackSize = 1;

    //The weight of each individual item in this stack
    public float kilogramPerUnit = 1;
}
