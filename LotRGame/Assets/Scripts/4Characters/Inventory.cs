﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //The total weight of all objects in this inventory
    public float currentWeight = 0;

    //The physical armor total
    public int totalPhysicalArmor = 0;
    //The magic armor total
    public int totalMagicArmor = 0;

    //Slots of armor that are currently worn
    public Armor helm = null;
    public Armor chestPiece = null;
    public Armor leggings = null;
    public Armor gloves = null;
    public Armor shoes = null;
    public Armor cloak = null;
    public Armor necklace = null;
    public Armor ring = null;

    //The weapon(s) that are currently being held
    public Weapon rightHand = null;
    public Weapon leftHand = null;

    //The items that are currently being held in this object's storage
    public List<Item> itemSlots = new List<Item>(20);



    //Constructor for this class
    public Inventory(int inventorySize_)
    {
        //Sets the number of inventory slots in our list
        this.itemSlots = new List<Item>(inventorySize_);

        //Sets all item slots to empty
        for(int s = 0; s < this.itemSlots.Count; ++s)
        {
            this.itemSlots[s] = null;
        }
    }


    //Finds the total weight in kilograms of all items in this inventory
    public void FindTotalWeight()
    {
        //The sum that is returned
        float weightSum = 0;

        //Looping through each slot
        for(int s = 0; s < this.itemSlots.Count; ++s)
        {
            if(this.itemSlots[s] != null)
            {
                //Adding the weight of all items in this slot
                weightSum += (this.itemSlots[s].kilogramPerUnit * this.itemSlots[s].currentStackSize);
            }
        }

        //Getting the weight for the helm slot if it isn't empty
        if (this.helm != null)
        {
            weightSum += this.helm.GetComponent<Item>().kilogramPerUnit;
        }

        //Getting the weight for the chest slot if it isn't empty
        if (this.chestPiece != null)
        {
            weightSum += this.chestPiece.GetComponent<Item>().kilogramPerUnit;
        }

        //Getting the weight for the leg slot if it isn't empty
        if (this.leggings != null)
        {
            weightSum += this.leggings.GetComponent<Item>().kilogramPerUnit;
        }

        //Getting the weight for the glove slot if it isn't empty
        if (this.gloves != null)
        {
            weightSum += this.gloves.GetComponent<Item>().kilogramPerUnit;
        }

        //Getting the weight for the shoe slot if it isn't empty
        if (this.shoes != null)
        {
            weightSum += this.shoes.GetComponent<Item>().kilogramPerUnit;
        }

        //Getting the weight for the cloak slot if it isn't empty
        if (this.cloak != null)
        {
            weightSum += this.cloak.GetComponent<Item>().kilogramPerUnit;
        }

        //Getting the weight for the necklace slot if it isn't empty
        if (this.necklace != null)
        {
            weightSum += this.necklace.GetComponent<Item>().kilogramPerUnit;
        }

        //Getting the weight for the ring slot if it isn't empty
        if (this.ring != null)
        {
            weightSum += this.ring.GetComponent<Item>().kilogramPerUnit;
        }


        this.currentWeight = weightSum;
    }


    //Checks to see if there's an empty slot in this inventory and returns the number available
    public int CheckForEmptySlot()
    {
        //The number of empty slots found
        int freeSlots = 0;

        //Looping through each slot to find empty ones
        for(int s = 0; s < this.itemSlots.Count; ++s)
        {
            //If an empty slot is found, the counter goes up
            if(this.itemSlots[s] == null)
            {
                freeSlots += 1;
            }
        }

        return freeSlots;
    }


    //Checks this inventory for a specific instance of an item and returns true of it's found
    public bool CheckForItem(Item itemToSearchFor_)
    {
        //Looping through each slot to look for a null
        for(int s = 0; s < this.itemSlots.Count; ++s)
        {
            //Only checks the slots that aren't empty
            if(this.itemSlots[s] != null)
            {
                //If the item is found, we return true
                if(this.itemSlots[s] == itemToSearchFor_)
                {
                    return true;
                }
                //If the item is of the same type as the one we're looking for, we check the stack
                if(this.itemSlots[s].name == itemToSearchFor_.name)
                {
                    //Looping through each child (stacked item) in the current item slot
                    for(int i = 0; i < this.itemSlots[s].transform.childCount; ++i)
                    {
                        if(this.itemSlots[s].transform.GetChild(i).gameObject == itemToSearchFor_.gameObject)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        //Checking equipped slots
        if(this.helm.gameObject == itemToSearchFor_.gameObject || this.chestPiece.gameObject == itemToSearchFor_.gameObject ||
            this.leggings.gameObject == itemToSearchFor_.gameObject || this.shoes.gameObject == itemToSearchFor_.gameObject ||
            this.gloves.gameObject == itemToSearchFor_.gameObject || this.necklace.gameObject == itemToSearchFor_.gameObject ||
            this.cloak.gameObject == itemToSearchFor_.gameObject || this.ring.gameObject == itemToSearchFor_.gameObject ||
            this.rightHand.gameObject == itemToSearchFor_.gameObject || this.leftHand.gameObject == itemToSearchFor_.gameObject)
        {
            return true;
        }

        //If the item wasn't found, returns false
        return false;
    }


    //Adds an item to this object's inventory. Returns true if there was enough space, false if it's full
    public bool AddItemToInventory(Item itemToAdd_)
    {
        //Checks to make sure the object isn't already in our inventory so we don't add it multiple times
        if(this.CheckForItem(itemToAdd_))
        {
            //If the item was found, we don't need to do anything else
            return true;
        }

        //Looping through each slot to find an empty one
        for(int s = 0; s < this.itemSlots.Count; ++s)
        {
            //If the slot is empty
            if(this.itemSlots[s] == null)
            {
                //Sets the new item to the empty inventory slot
                this.itemSlots[s] = itemToAdd_;
                //Parents the new item to this object's transform
                itemToAdd_.transform.SetParent(this.transform);

                //Update the weight
                this.FindTotalWeight();

                return true;
            }
            //If item in the current slot is the same as the one being added
            else if(this.itemSlots[s].itemNameID == itemToAdd_.itemNameID)
            {
                //Checking to see if the current stack has the maximum number of items
                if(this.itemSlots[s].currentStackSize < this.itemSlots[s].maxStackSize)
                {
                    //If the sum of the current stack and the added stack doesn't overflow
                    if( (this.itemSlots[s].currentStackSize + itemToAdd_.currentStackSize) <= this.itemSlots[s].maxStackSize)
                    {
                        //Looping through each child (stacked item) in the item to add
                        while(itemToAdd_.transform.childCount > 0)
                        {
                            Transform itemChild = itemToAdd_.transform.FindChild(this.itemSlots[s].name);

                            //If for some reason there isn't an available child of the same type but there are still children, we break the loop
                            if(itemChild == null)
                            {
                                break;
                            }

                            //Setting the stacked item to stack to the one in this object's inventory
                            itemChild.transform.SetParent(this.itemSlots[s].transform);
                            //Increasing the number of items in the current inventory stack
                            this.itemSlots[s].currentStackSize += 1;
                            //Decreasing the number of items in the item stack to pick up
                            itemToAdd_.currentStackSize -= 1;
                        }

                        //Once all children in the item stack are added, the parent of the stack is added to this object's inventory
                        itemToAdd_.transform.SetParent(this.itemSlots[s].transform);
                        this.itemSlots[s].currentStackSize += 1;

                        //Update the total weight
                        this.FindTotalWeight();

                        return true;
                    }
                    //If the sum of the current stack and added stack overflows, we add as many as we can
                    else if( (this.itemSlots[s].currentStackSize + itemToAdd_.currentStackSize) > this.itemSlots[s].maxStackSize)
                    {
                        //Looping through each child (stacked item) in the item to add until the current stack is full
                        while(this.itemSlots[s].currentStackSize < this.itemSlots[s].maxStackSize)
                        {
                            Transform itemChild = itemToAdd_.transform.FindChild(this.itemSlots[s].name);

                            //If for some reason there isn't an available child of the same type, we break the loop and add the parent to the inventory
                            if (itemChild == null)
                            {
                                //Making sure that the item's stack size is 1 since there's no items in its stack
                                itemToAdd_.currentStackSize = 1;
                                //Setting the item to stack with the one in this object's inventory
                                itemToAdd_.transform.SetParent(this.itemSlots[s].transform);
                                //Increasing the number of items in the current inventory stack
                                this.itemSlots[s].currentStackSize += 1;

                                //Update the total weight
                                this.FindTotalWeight();

                                return true;
                            }

                            //Setting the stacked item to stack to the one in this object's inventory
                            itemChild.transform.SetParent(this.itemSlots[s].transform);
                            //Increasing the number of items in the current inventory stack
                            this.itemSlots[s].currentStackSize += 1;
                            //Decreasing the number of items in the item stack to pick up
                            itemToAdd_.currentStackSize -= 1;
                        }
                    }
                }
            }
        }

        //Returning false since there wasn't room for the new item
        return false;
    }


    //Adds all items in the target inventory to this object's inventory
    public void AddAllItemsToInventory(Inventory otherInventory_)
    {
        //Adding all of the other inventory's armor and weapon slots to this inventory
        if(this.AddItemToInventory(otherInventory_.helm.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.helm.GetComponent<Item>(), null);
        }

        if (this.AddItemToInventory(otherInventory_.chestPiece.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.chestPiece.GetComponent<Item>(), null);
        }

        if (this.AddItemToInventory(otherInventory_.leggings.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.leggings.GetComponent<Item>(), null);
        }

        if (this.AddItemToInventory(otherInventory_.shoes.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.shoes.GetComponent<Item>(), null);
        }

        if (this.AddItemToInventory(otherInventory_.gloves.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.gloves.GetComponent<Item>(), null);
        }

        if (this.AddItemToInventory(otherInventory_.cloak.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.cloak.GetComponent<Item>(), null);
        }

        if (this.AddItemToInventory(otherInventory_.necklace.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.necklace.GetComponent<Item>(), null);
        }

        if (this.AddItemToInventory(otherInventory_.ring.GetComponent<Item>()))
        {
            otherInventory_.ChangeItem(otherInventory_.ring.GetComponent<Item>(), null);
        }

        //Loops through each slot in the other inventory
        for (int s = 0; s < otherInventory_.itemSlots.Count; ++s)
        {
            //As long as the current slot in the other inventory isn't empty, the item is added to this inventory
            if(otherInventory_.itemSlots[s] != null)
            {
                //If the item could be added to our inventory, it's removed from the other one
                if(this.AddItemToInventory(otherInventory_.itemSlots[s]))
                {
                    otherInventory_.ChangeItem(otherInventory_.itemSlots[s], null);
                }
            }
        }

        //Calculating the new weight for this inventory and the other inventory
        this.FindTotalWeight();
        otherInventory_.FindTotalWeight();
    }


    //Equips a piece of armor to the correct slot
    public bool EquipArmor(Armor armorToEquip_)
    {
        //If true, this armor can be equipped. If false, there wasn't a free inventory slot to put the one that this armor replaces
        bool canEquip = true;

        //Unequipping the armor slot that the equipped one will occupy
        this.UnequipArmor(armorToEquip_.slot);

        //Finding the correct slot to equip the armor
        switch(armorToEquip_.slot)
        {
            case Armor.ArmorSlot.Head:
                if (this.helm != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.helm = armorToEquip_;
                }
                break;

            case Armor.ArmorSlot.Torso:
                if(this.chestPiece != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.chestPiece = armorToEquip_;
                }
                break;

            case Armor.ArmorSlot.Legs:
                if (this.leggings != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.leggings = armorToEquip_;
                }
                break;

            case Armor.ArmorSlot.Hands:
                if(this.gloves != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.gloves = armorToEquip_;
                }
                break;

            case Armor.ArmorSlot.Feet:
                if (this.shoes != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.shoes = armorToEquip_;
                }
                break;

            case Armor.ArmorSlot.Cloak:
                if (this.cloak != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.cloak = armorToEquip_;
                }
                break;

            case Armor.ArmorSlot.Necklace:
                if (this.necklace != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.necklace = armorToEquip_;
                }
                break;

            case Armor.ArmorSlot.Ring:
                if (this.ring != null)
                {
                    canEquip = false;
                }
                else
                {
                    this.ring = armorToEquip_;
                }
                break;
        }

        return canEquip;
    }


    //Unequips the piece of armor in the given slot
    public void UnequipArmor(Armor.ArmorSlot slotToRemove_)
    {
        //Making sure there's a free inventory slot first
        if(this.CheckForEmptySlot() < 1)
        {
            return;
        }

        //Finding the correct slot to equip the armor, adding it to the inventory, and setting the slot to null
        switch (slotToRemove_)
        {
            case Armor.ArmorSlot.Head:
                this.AddItemToInventory(this.helm.GetComponent<Item>());
                this.helm = null;
                break;
            case Armor.ArmorSlot.Torso:
                this.AddItemToInventory(this.chestPiece.GetComponent<Item>());
                this.chestPiece = null;
                break;
            case Armor.ArmorSlot.Legs:
                this.AddItemToInventory(this.leggings.GetComponent<Item>());
                this.leggings = null;
                break;
            case Armor.ArmorSlot.Hands:
                this.AddItemToInventory(this.gloves.GetComponent<Item>());
                this.gloves = null;
                break;
            case Armor.ArmorSlot.Feet:
                this.AddItemToInventory(this.shoes.GetComponent<Item>());
                this.shoes = null;
                break;
            case Armor.ArmorSlot.Cloak:
                this.AddItemToInventory(this.cloak.GetComponent<Item>());
                this.cloak = null;
                break;
            case Armor.ArmorSlot.Necklace:
                this.AddItemToInventory(this.necklace.GetComponent<Item>());
                this.necklace = null;
                break;
            case Armor.ArmorSlot.Ring:
                this.AddItemToInventory(this.ring.GetComponent<Item>());
                this.ring = null;
                break;
        }
    }


    //Equips a weapon to this object's hands
    public bool EquipWeapon(Weapon weaponToEquip_)
    {
        //If true, this weapon was equipped, if false, there wasn't enough inventory space for the unequipped weapons
        bool canEquip = true;

        //Finding the correct slot to equip the weapon
        switch (weaponToEquip_.size)
        {
            case Weapon.WeaponSize.OneHand:
                //If the right hand is empty, this weapon is equipped there
                if(this.rightHand == null)
                {
                    this.rightHand = weaponToEquip_;
                }
                //If the right hand is full and the left isn't, the weapon is equipped in the left hand
                else if(this.leftHand == null && this.rightHand.size != Weapon.WeaponSize.TwoHands)
                {
                    this.leftHand = weaponToEquip_;
                }
                //If both hands are full, this weapon replaces the weapon in the right hand
                else
                {
                    //Unequipping the right hand weapon
                    this.UnequipWeapon(WeaponHand.Right);

                    //Making sure the right hand is empty, since the inventory could be full
                    if (this.rightHand == null)
                    {
                        this.rightHand = weaponToEquip_;
                    }
                    else
                    {
                        canEquip = false;
                    }
                }
                break;
            case Weapon.WeaponSize.TwoHands:
                //Unequipping both weapons
                this.UnequipWeapon(WeaponHand.Both);

                //Making sure both hands are empty, since the inventory could be full
                if (this.rightHand == null && this.leftHand == null)
                {
                    //Two-handed weapons replace any weapons that are being held
                    this.rightHand = weaponToEquip_;
                    this.leftHand = null;
                }
                else
                {
                    canEquip = false;
                }
                break;
        }

        return canEquip;
    }


    //Unequips the weapon in the given slot
    public enum WeaponHand {Right, Left, Both};
    public void UnequipWeapon(WeaponHand handToRemove_)
    {
        switch(handToRemove_)
        {
            case WeaponHand.Right:
                //Making sure there's an equipped weapon and empty slot to put the weapon
                if(this.rightHand != null && this.CheckForEmptySlot() > 0)
                {
                    //Moves the current right hand weapon to the inventory and nulls the hand slot
                    this.AddItemToInventory(this.rightHand.GetComponent<Item>());

                    //If this character is holding a 2-handed weapon, the left hand is freed as well
                    if(this.rightHand.size == Weapon.WeaponSize.TwoHands)
                    {
                        this.leftHand = null;
                    }

                    this.rightHand = null;
                }
                break;
            case WeaponHand.Left:
                //Making sure there's an equipped weapon and empty slot to put the weapon
                if(this.leftHand != null && this.CheckForEmptySlot() > 0)
                {
                    //Moves the current left hand weapon to the inventory and nulls the hand slot
                    this.AddItemToInventory(this.leftHand.GetComponent<Item>());
                    this.leftHand = null;
                }
                break;
            case WeaponHand.Both:
                //Calls this function twice, but with the other hands
                this.UnequipWeapon(WeaponHand.Right);
                this.UnequipWeapon(WeaponHand.Left);
                break;
        }
    }


    //Checks to see if the piece of armor is currently equipped
    public bool IsArmorEquipped(Armor armorToCheck_)
    {
        //Checking the correct armor slot to see if the equipped armor matches
        switch(armorToCheck_.slot)
        {
            case Armor.ArmorSlot.Head:
                if(this.helm == armorToCheck_)
                {
                    return true;
                }
                break;
            case Armor.ArmorSlot.Torso:
                if(this.chestPiece == armorToCheck_)
                {
                    return true;
                }
                break;
            case Armor.ArmorSlot.Legs:
                if(this.leggings == armorToCheck_)
                {
                    return true;
                }
                break;
            case Armor.ArmorSlot.Hands:
                if(this.gloves == armorToCheck_)
                {
                    return true;
                }
                break;
            case Armor.ArmorSlot.Feet:
                if(this.shoes == armorToCheck_)
                {
                    return true;
                }
                break;
            case Armor.ArmorSlot.Cloak:
                if(this.cloak == armorToCheck_)
                {
                    return true;
                }
                break;
            case Armor.ArmorSlot.Necklace:
                if(this.necklace == armorToCheck_)
                {
                    return true;
                }
                break;
            case Armor.ArmorSlot.Ring:
                if(this.ring == armorToCheck_)
                {
                    return true;
                }
                break;
            default:
                return false;
        }

        //If there wasn't a match, the armor wasn't equipped
        return false;
    }


    //Function used to change one item into another. Used for replacing items with old/broken/rotten versions or swapping item slots in inventories
    public void ChangeItem(Item selectedItem_, Item replacementItem_, bool destroySelectedItem_ = false)
    {
        //Checking to see if the selected item is equipped as armor
        if (this.helm.gameObject == selectedItem_.gameObject || this.chestPiece.gameObject == selectedItem_.gameObject ||
            this.leggings.gameObject == selectedItem_.gameObject || this.shoes.gameObject == selectedItem_.gameObject ||
            this.gloves.gameObject == selectedItem_.gameObject || this.necklace.gameObject == selectedItem_.gameObject ||
            this.cloak.gameObject == selectedItem_.gameObject || this.ring.gameObject == selectedItem_.gameObject)
        {
            //Making sure the replacement item is armor AND is the same type of armor
            if(replacementItem_.GetComponent<Armor>() && replacementItem_.GetComponent<Armor>().slot == selectedItem_.GetComponent<Armor>().slot)
            {
                this.EquipArmor(replacementItem_.GetComponent<Armor>());

            }
            //If not, the replacement armor is just added to the inventory and not equipped
            else
            {
                //Making sure there's room in our inventory before doing anything
                if(this.AddItemToInventory(replacementItem_))
                {

                }
            }
        }
        //Checking to see if the selected item is equipped as a weapon
        else if(this.rightHand.gameObject == selectedItem_.gameObject || this.leftHand.gameObject == selectedItem_.gameObject)
        {
            //Making sure the replacement item is a weapon
            if(replacementItem_.GetComponent<Weapon>())
            {

            }
        }
        //Otherwise the item is in the general inventory
        else
        {
            this.AddItemToInventory(replacementItem_);
        }

        //If the item needs to be destroyed after the change, it is
        if(destroySelectedItem_)
        {
            Destroy(selectedItem_.gameObject);
        }

        //Updates the weight
        this.FindTotalWeight();
    }


    //Function used to just change one item in the inventory at the given index to something else and update the weight. Returns the item that was at that index
    public Item ChangeInventoryItemAtIndex(int index_, Item itemToChangeTo_)
    {
        //Making sure the index is within the correct limits of our inventory
        if(index_ < 0 || index_ >= this.itemSlots.Count)
        {
            return null;
        }

        //Getting the reference to the item that will be replaced
        Item returnedItem = this.itemSlots[index_];
        //Setting the item slot to contain the new item
        this.itemSlots[index_] = itemToChangeTo_;
        //Updating this inventory's weight
        this.FindTotalWeight();

        //Returning the original item
        return returnedItem;
    }


    //Function that checks to see if a given item will be able to be added to this inventory
    public bool CanItemBeAddedToInventory(Item itemToAdd_)
    {
        //Int to track the stack size of all similar items found
        uint leftoverStack = itemToAdd_.currentStackSize;

        //Loops through each inventory slot
        for(int s = 0; s < this.itemSlots.Count; ++s)
        {
            //If we find an empty slot, the item can be added
            if(this.itemSlots[s] == null)
            {
                return true;
            }
            //If we find an item of the same type that can be stacked
            else if(this.itemSlots[s].itemNameID == itemToAdd_.itemNameID && itemToAdd_.maxStackSize > 1)
            {
                //If the items can be stacked without overflowing, the item can be added
                if(this.itemSlots[s].currentStackSize + itemToAdd_.currentStackSize <= itemToAdd_.maxStackSize)
                {
                    return true;
                }
                //Checking to see if the leftover stack from overflowing would fit
                else if(this.itemSlots[s].currentStackSize + leftoverStack <= itemToAdd_.maxStackSize)
                {
                    return true;
                }
                //Otherwise we subtract the amount that would be able to stack from the leftovers
                else
                {
                    leftoverStack -= (itemToAdd_.maxStackSize - this.itemSlots[s].currentStackSize);
                }
            }
        }

        //If we haven't found anything yet, the item can't be added
        return false;
    }
}
