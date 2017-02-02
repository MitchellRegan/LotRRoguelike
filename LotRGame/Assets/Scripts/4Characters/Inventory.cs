using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //The total weight of all objects in this inventory
    public float currentWeight = 0;

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

        //If the item wasn't found, returns false
        return false;
    }


    //Adds an item to this object's inventory
    public void AddItemToInventory(Item itemToAdd_)
    {
        //Checks to make sure the object isn't already in our inventory so we don't add it multiple times
        if(this.CheckForItem(itemToAdd_))
        {
            //If the item was found, we don't need to do anything else
            return;
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

                return;
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

                        return;
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

                                return;
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

        //Calculating the new weight 
        this.FindTotalWeight();
    }


    //Adds all items in the target inventory to this object's inventory
    public void AddAllItemsToInventory(Inventory otherInventory_)
    {
        //Loops through each slot in the other inventory
        for(int s = 0; s < otherInventory_.itemSlots.Count; ++s)
        {
            //As long as the current slot in the other inventory isn't empty, the item is added to this inventory
            if(otherInventory_.itemSlots[s] != null)
            {
                this.AddItemToInventory(otherInventory_.itemSlots[s]);
            }
        }

        //Calculating the new weight for this inventory and the other inventory
        this.FindTotalWeight();
        otherInventory_.FindTotalWeight();
    }


    //Swaps the slots of the two items
    public void SwapItemSlots(int index1_, int index2_)
    {
        //If either index is out of range of the item slot list OR they have the same index, we can't swap
        if(index1_ < 0 || index1_ >= this.itemSlots.Count || index2_ < 0 || index2_ > this.itemSlots.Count || index1_ == index2_)
        {
            return;
        }

        //Setting a placeholder to hold the reference to the item at index 1
        Item placeholder = this.itemSlots[index1_];
        //Setting the reference at index 1 to the item of index 2
        this.itemSlots[index1_] = this.itemSlots[index2_];
        //Setting the reference at index 2 to the placeholder item
        this.itemSlots[index2_] = placeholder;
    }


    //Equips a piece of armor to the correct slot
    public void EquipArmor(Armor armorToEquip_)
    {
        //If the armor isn't in our inventory, we can't equip it
        if(!this.CheckForItem(armorToEquip_.GetComponent<Item>()))
        {
            return;
        }

        //Finding the correct slot to equip the armor
        switch(armorToEquip_.slot)
        {
            case Armor.ArmorSlot.Head:
                this.helm = armorToEquip_;
                break;
            case Armor.ArmorSlot.Torso:
                this.chestPiece = armorToEquip_;
                break;
            case Armor.ArmorSlot.Legs:
                this.leggings = armorToEquip_;
                break;
            case Armor.ArmorSlot.Hands:
                this.gloves = armorToEquip_;
                break;
            case Armor.ArmorSlot.Feet:
                this.shoes = armorToEquip_;
                break;
            case Armor.ArmorSlot.Cloak:
                this.cloak = armorToEquip_;
                break;
            case Armor.ArmorSlot.Necklace:
                this.necklace = armorToEquip_;
                break;
            case Armor.ArmorSlot.Ring:
                this.ring = armorToEquip_;
                break;
        }
    }


    //Unequips the piece of armor in the given slot
    public void UnequipArmor(Armor.ArmorSlot slotToRemove_)
    {
        //Finding the correct slot to equip the armor
        switch (slotToRemove_)
        {
            case Armor.ArmorSlot.Head:
                this.helm = null;
                break;
            case Armor.ArmorSlot.Torso:
                this.chestPiece = null;
                break;
            case Armor.ArmorSlot.Legs:
                this.leggings = null;
                break;
            case Armor.ArmorSlot.Hands:
                this.gloves = null;
                break;
            case Armor.ArmorSlot.Feet:
                this.shoes = null;
                break;
            case Armor.ArmorSlot.Cloak:
                this.cloak = null;
                break;
            case Armor.ArmorSlot.Necklace:
                this.necklace = null;
                break;
            case Armor.ArmorSlot.Ring:
                this.ring = null;
                break;
        }
    }


    //Equips a weapon to this object's hands
    public void EquipWeapon(Weapon weaponToEquip_)
    {
        //If the weapon isn't in our inventory, we can't equip it
        if (!this.CheckForItem(weaponToEquip_.GetComponent<Item>()))
        {
            return;
        }

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
                    this.rightHand = weaponToEquip_;
                }
                break;
            case Weapon.WeaponSize.TwoHands:
                //Two-handed weapons replace any weapons that are being held
                this.rightHand = weaponToEquip_;
                this.leftHand = null;
                break;
        }
    }


    //Unequips the weapon in the given slot
    public enum WeaponHand {Right, Left, Both};
    public void UnequipWeapon(WeaponHand handToRemove_)
    {
        switch(handToRemove_)
        {
            case WeaponHand.Right:
                this.rightHand = null;
                break;
            case WeaponHand.Left:
                this.leftHand = null;
                //If the left hand is holding a 2 handed weapon, it must be unequipped from both hands
                if(this.rightHand.size == Weapon.WeaponSize.TwoHands)
                {
                    this.rightHand = null;
                }
                break;
            case WeaponHand.Both:
                this.leftHand = null;
                this.rightHand = null;
                break;
        }
    }
}
