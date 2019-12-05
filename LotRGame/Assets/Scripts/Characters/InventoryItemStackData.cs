using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in Character.cs to store objects for serialization 
[System.Serializable]
public class InventoryItemStackData
{
    //Index where this item is stored in a character's inventory
    public int itemStackIndex = 0;
    //The enum for what type of object this is
    public ObjectType objType;
    //The int for the object's ID number
    public int iDNumber;
    //The number of items in this stack
    public uint numberOfItemsInStack = 1;

    //Constructor for this class
    public InventoryItemStackData(int inventoryIndex_, IDTag objIDTag_, uint stackSize_)
    {
        this.itemStackIndex = inventoryIndex_;
        this.objType = objIDTag_.objType;
        this.iDNumber = objIDTag_.numberID;
        this.numberOfItemsInStack = stackSize_ - 1; //Removing one because the first one is already saved in the index position
    }
}