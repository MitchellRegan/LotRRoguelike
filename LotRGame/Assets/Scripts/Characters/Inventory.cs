using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //The total weight of all objects in this inventory
    public float currentWeight = 0;

    //The physical armor total
    [HideInInspector]
    public int totalPhysicalArmor = 0;

    //The slashing armor total
    [HideInInspector]
    public int totalSlashingArmor = 0;
    //The stabbing armor total
    [HideInInspector]
    public int totalStabbingArmor = 0;
    //The crushing armor total
    [HideInInspector]
    public int totalCrushingArmor = 0;

    //The arcane armor total
    [HideInInspector]
    public int totalArcaneResist = 0;
    //The holy armor total
    [HideInInspector]
    public int totalHolyResist = 0;
    //The dark armor total
    [HideInInspector]
    public int totalDarkResist = 0;
    //The fire armor total
    [HideInInspector]
    public int totalFireResist = 0;
    //The water armor total
    [HideInInspector]
    public int totalWaterResist = 0;
    //The electric armor total
    [HideInInspector]
    public int totalElectricResist = 0;
    //The wind armor total
    [HideInInspector]
    public int totalWindResist = 0;
    //The nature armor total
    [HideInInspector]
    public int totalNatureResist = 0;
    //The bleed armor total
    [HideInInspector]
    public int totalBleedResist = 0;

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

    //The amount of money that this character is holding
    public int wallet = 0;



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


    //Function called when this object is created
    private void Awake()
    {
        //If this inventory was created with prefabs, we need to create instances of the prefab items
        for(int s = 0; s < this.itemSlots.Count; ++s)
        {
            if(this.itemSlots[s] != null)
            {
                //Creates a new instance of the item and sets this object as the parent
                GameObject newItemInstance = Object.Instantiate(this.itemSlots[s].gameObject, this.transform);
                //Replaces the item reference with the instanced object's reference
                this.itemSlots[s] = newItemInstance.GetComponent<Item>();
            }
        }

        //Checking equipped item slots for prefabs
        if(this.helm != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.helm.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.helm = newItemInstance.GetComponent<Armor>();
        }

        if(this.chestPiece != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.chestPiece.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.chestPiece = newItemInstance.GetComponent<Armor>();
        }

        if (this.leggings != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.leggings.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.leggings = newItemInstance.GetComponent<Armor>();
        }

        if (this.shoes != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.shoes.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.shoes = newItemInstance.GetComponent<Armor>();
        }

        if (this.gloves != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.gloves.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.gloves = newItemInstance.GetComponent<Armor>();
        }

        if (this.necklace != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.necklace.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.necklace = newItemInstance.GetComponent<Armor>();
        }

        if (this.cloak != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.cloak.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.cloak = newItemInstance.GetComponent<Armor>();
        }

        if (this.ring != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.ring.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.ring = newItemInstance.GetComponent<Armor>();
        }

        if (this.rightHand != null)
        {
            //Creates a new instance of the item and sets this object as the parent
            GameObject newItemInstance = Object.Instantiate(this.rightHand.gameObject, this.transform);
            //Replaces the item reference with the instanced object's reference
            this.rightHand = newItemInstance.GetComponent<Weapon>();
        }

        if (this.leftHand != null)
        {
            //If we're trying to equip a 2 handed weapon to the left hand, it has to be cleared
            if(this.leftHand.size == Weapon.WeaponSize.TwoHands)
            {
                this.leftHand = null;
            }
            //Making sure that there isn't a 2 handed weapon already equipped
            else if (this.rightHand == null || this.rightHand.size == Weapon.WeaponSize.OneHand)
            {
                //Creates a new instance of the item and sets this object as the parent
                GameObject newItemInstance = Object.Instantiate(this.leftHand.gameObject, this.transform);
                //Replaces the item reference with the instanced object's reference
                this.leftHand = newItemInstance.GetComponent<Weapon>();
            }
            //If the right hand is holding a 2 handed weapon, this weapon has to be cleared
            else
            {
                this.leftHand = null;
            }
        }

        //Finding the weight
        this.FindArmorStats();
    }


    //Finds the total weight in kilograms of all items in this inventory
    public void FindArmorStats()
    {
        //The sum of the weight that'll be set
        float weightSum = 0;
        //The sum of the physical armor of equipped gear
        int physicalArmorSum = 0;
        int slashingArmorSum = 0;
        int stabbingArmorSum = 0;
        int crushingArmorSum = 0;
        //The sum of the magic resist of equipped gear
        int arcaneResistSum = 0;
        int fireResistSum = 0;
        int waterResistSum = 0;
        int electricResistSum = 0;
        int windResistSum = 0;
        int lightResistSum = 0;
        int darkResistSum = 0;
        int natureResistSum = 0;
        int bleedResistSum = 0;

        //Looping through each inventory slot
        for (int s = 0; s < this.itemSlots.Count; ++s)
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
            physicalArmorSum += this.helm.physicalDefense;
            slashingArmorSum += this.helm.slashingDefense;
            stabbingArmorSum += this.helm.stabbingDefense;
            crushingArmorSum += this.helm.crushingDefense;

            arcaneResistSum += this.helm.arcaneResist;
            fireResistSum += this.helm.fireResist;
            waterResistSum += this.helm.waterResist;
            electricResistSum += this.helm.electricResist;
            windResistSum += this.helm.windResist;
            lightResistSum += this.helm.lightResist;
            darkResistSum += this.helm.darkResist;
            natureResistSum += this.helm.natureResist;

            bleedResistSum += this.helm.bleedDefense;
        }

        //Getting the weight for the chest slot if it isn't empty
        if (this.chestPiece != null)
        {
            weightSum += this.chestPiece.GetComponent<Item>().kilogramPerUnit;
            physicalArmorSum += this.chestPiece.physicalDefense;
            slashingArmorSum += this.chestPiece.slashingDefense;
            stabbingArmorSum += this.chestPiece.stabbingDefense;
            crushingArmorSum += this.chestPiece.crushingDefense;

            arcaneResistSum += this.chestPiece.arcaneResist;
            fireResistSum += this.chestPiece.fireResist;
            waterResistSum += this.chestPiece.waterResist;
            electricResistSum += this.chestPiece.electricResist;
            windResistSum += this.chestPiece.windResist;
            lightResistSum += this.chestPiece.lightResist;
            darkResistSum += this.chestPiece.darkResist;
            natureResistSum += this.chestPiece.natureResist;

            bleedResistSum += this.chestPiece.bleedDefense;
        }

        //Getting the weight for the leg slot if it isn't empty
        if (this.leggings != null)
        {
            weightSum += this.leggings.GetComponent<Item>().kilogramPerUnit;
            physicalArmorSum += this.leggings.physicalDefense;
            slashingArmorSum += this.leggings.slashingDefense;
            stabbingArmorSum += this.leggings.stabbingDefense;
            crushingArmorSum += this.leggings.crushingDefense;

            arcaneResistSum += this.leggings.arcaneResist;
            fireResistSum += this.leggings.fireResist;
            waterResistSum += this.leggings.waterResist;
            electricResistSum += this.leggings.electricResist;
            windResistSum += this.leggings.windResist;
            lightResistSum += this.leggings.lightResist;
            darkResistSum += this.leggings.darkResist;
            natureResistSum += this.leggings.natureResist;

            bleedResistSum += this.leggings.bleedDefense;
        }

        //Getting the weight for the glove slot if it isn't empty
        if (this.gloves != null)
        {
            weightSum += this.gloves.GetComponent<Item>().kilogramPerUnit;
            physicalArmorSum += this.gloves.physicalDefense;
            slashingArmorSum += this.gloves.slashingDefense;
            stabbingArmorSum += this.gloves.stabbingDefense;
            crushingArmorSum += this.gloves.crushingDefense;

            arcaneResistSum += this.gloves.arcaneResist;
            fireResistSum += this.gloves.fireResist;
            waterResistSum += this.gloves.waterResist;
            electricResistSum += this.gloves.electricResist;
            windResistSum += this.gloves.windResist;
            lightResistSum += this.gloves.lightResist;
            darkResistSum += this.gloves.darkResist;
            natureResistSum += this.gloves.natureResist;

            bleedResistSum += this.gloves.bleedDefense;
        }

        //Getting the weight for the shoe slot if it isn't empty
        if (this.shoes != null)
        {
            weightSum += this.shoes.GetComponent<Item>().kilogramPerUnit;
            physicalArmorSum += this.shoes.physicalDefense;
            slashingArmorSum += this.shoes.slashingDefense;
            stabbingArmorSum += this.shoes.stabbingDefense;
            crushingArmorSum += this.shoes.crushingDefense;

            arcaneResistSum += this.shoes.arcaneResist;
            fireResistSum += this.shoes.fireResist;
            waterResistSum += this.shoes.waterResist;
            electricResistSum += this.shoes.electricResist;
            windResistSum += this.shoes.windResist;
            lightResistSum += this.shoes.lightResist;
            darkResistSum += this.shoes.darkResist;
            natureResistSum += this.shoes.natureResist;

            bleedResistSum += this.shoes.bleedDefense;
        }

        //Getting the weight for the cloak slot if it isn't empty
        if (this.cloak != null)
        {
            weightSum += this.cloak.GetComponent<Item>().kilogramPerUnit;
            physicalArmorSum += this.cloak.physicalDefense;
            slashingArmorSum += this.cloak.slashingDefense;
            stabbingArmorSum += this.cloak.stabbingDefense;
            crushingArmorSum += this.cloak.crushingDefense;

            arcaneResistSum += this.cloak.arcaneResist;
            fireResistSum += this.cloak.fireResist;
            waterResistSum += this.cloak.waterResist;
            electricResistSum += this.cloak.electricResist;
            windResistSum += this.cloak.windResist;
            lightResistSum += this.cloak.lightResist;
            darkResistSum += this.cloak.darkResist;
            natureResistSum += this.cloak.natureResist;

            bleedResistSum += this.cloak.bleedDefense;
        }

        //Getting the weight for the necklace slot if it isn't empty
        if (this.necklace != null)
        {
            weightSum += this.necklace.GetComponent<Item>().kilogramPerUnit;
            physicalArmorSum += this.necklace.physicalDefense;
            slashingArmorSum += this.necklace.slashingDefense;
            stabbingArmorSum += this.necklace.stabbingDefense;
            crushingArmorSum += this.necklace.crushingDefense;

            arcaneResistSum += this.necklace.arcaneResist;
            fireResistSum += this.necklace.fireResist;
            waterResistSum += this.necklace.waterResist;
            electricResistSum += this.necklace.electricResist;
            windResistSum += this.necklace.windResist;
            lightResistSum += this.necklace.lightResist;
            darkResistSum += this.necklace.darkResist;
            natureResistSum += this.necklace.natureResist;

            bleedResistSum += this.necklace.bleedDefense;
        }

        //Getting the weight for the ring slot if it isn't empty
        if (this.ring != null)
        {
            weightSum += this.ring.GetComponent<Item>().kilogramPerUnit;
            physicalArmorSum += this.ring.physicalDefense;
            slashingArmorSum += this.ring.slashingDefense;
            stabbingArmorSum += this.ring.stabbingDefense;
            crushingArmorSum += this.ring.crushingDefense;

            arcaneResistSum += this.ring.arcaneResist;
            fireResistSum += this.ring.fireResist;
            waterResistSum += this.ring.waterResist;
            electricResistSum += this.ring.electricResist;
            windResistSum += this.ring.windResist;
            lightResistSum += this.ring.lightResist;
            darkResistSum += this.ring.darkResist;
            natureResistSum += this.ring.natureResist;

            bleedResistSum += this.ring.bleedDefense;
        }

        //Getting the weight for the right hand slot if it isn't empty
        if(this.rightHand != null)
        {
            weightSum += this.rightHand.GetComponent<Item>().kilogramPerUnit;

            //Checking to see if the right hand weapon can also defend
            if(this.rightHand.GetComponent<Armor>())
            {
                Armor rHandArmor = this.rightHand.GetComponent<Armor>();

                physicalArmorSum += rHandArmor.physicalDefense;

                slashingArmorSum += rHandArmor.slashingDefense;
                stabbingArmorSum += rHandArmor.stabbingDefense;
                crushingArmorSum += rHandArmor.crushingDefense;

                arcaneResistSum += rHandArmor.arcaneResist;
                fireResistSum += rHandArmor.fireResist;
                waterResistSum += rHandArmor.waterResist;
                electricResistSum += rHandArmor.electricResist;
                windResistSum += rHandArmor.windResist;
                lightResistSum += rHandArmor.lightResist;
                darkResistSum += rHandArmor.darkResist;
                natureResistSum += rHandArmor.natureResist;

                bleedResistSum += rHandArmor.bleedDefense;
            }
        }

        //Getting the weight for the left hand slot if it isn't empty
        if(this.leftHand != null && this.leftHand != this.rightHand)
        {
            weightSum += this.leftHand.GetComponent<Item>().kilogramPerUnit;

            //Checking to see if the left hand weapon can also defend
            if (this.leftHand.GetComponent<Armor>())
            {
                Armor lHandArmor = this.leftHand.GetComponent<Armor>();

                physicalArmorSum += lHandArmor.physicalDefense;

                slashingArmorSum += lHandArmor.slashingDefense;
                stabbingArmorSum += lHandArmor.stabbingDefense;
                crushingArmorSum += lHandArmor.crushingDefense;

                arcaneResistSum += lHandArmor.arcaneResist;
                fireResistSum += lHandArmor.fireResist;
                waterResistSum += lHandArmor.waterResist;
                electricResistSum += lHandArmor.electricResist;
                windResistSum += lHandArmor.windResist;
                lightResistSum += lHandArmor.lightResist;
                darkResistSum += lHandArmor.darkResist;
                natureResistSum += lHandArmor.natureResist;

                bleedResistSum += lHandArmor.bleedDefense;
            }
        }

        //Setting all of the sums for weight, armor, and resistance
        this.currentWeight = weightSum;
        this.totalPhysicalArmor = physicalArmorSum;
        this.totalSlashingArmor = slashingArmorSum;
        this.totalStabbingArmor = stabbingArmorSum;
        this.totalCrushingArmor = crushingArmorSum;

        this.totalArcaneResist = arcaneResistSum;
        this.totalFireResist = fireResistSum;
        this.totalWaterResist = waterResistSum;
        this.totalElectricResist = electricResistSum;
        this.totalWindResist = windResistSum;
        this.totalHolyResist = lightResistSum;
        this.totalDarkResist = darkResistSum;
        this.totalNatureResist = natureResistSum;

        this.totalBleedResist = bleedResistSum;

        //If the character that this component is on is a player character
        if (CharacterManager.globalReference != null && CharacterManager.globalReference.selectedGroup != null &&
            CharacterManager.globalReference.selectedGroup.charactersInParty.Contains(this.GetComponent<Character>()))
        {
            //Updating the QuestTracker so we know if any fetch quests have been completed
            QuestTracker.globalReference.UpdateFetchQuests();
        }
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
        if(this.helm == itemToSearchFor_ || this.chestPiece == itemToSearchFor_ ||
            this.leggings == itemToSearchFor_ || this.shoes == itemToSearchFor_ ||
            this.gloves == itemToSearchFor_ || this.necklace == itemToSearchFor_ ||
            this.cloak == itemToSearchFor_ || this.ring == itemToSearchFor_ ||
            this.rightHand == itemToSearchFor_ || this.leftHand == itemToSearchFor_)
        {
            return true;
        }

        //If the item wasn't found, returns false
        return false;
    }


    //Adds an item to this object's inventory. Returns true if there was enough space, false if it's full
    public bool AddItemToInventory(Item itemToAdd_)
    {
        //Checks to make sure the specific object instance isn't already in our inventory so we don't add it multiple times
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
                this.FindArmorStats();

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
                        this.FindArmorStats();

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
                                this.FindArmorStats();

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
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Head, null);
        }

        if (this.AddItemToInventory(otherInventory_.chestPiece.GetComponent<Item>()))
        {
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Torso, null);
        }

        if (this.AddItemToInventory(otherInventory_.leggings.GetComponent<Item>()))
        {
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Legs, null);
        }

        if (this.AddItemToInventory(otherInventory_.shoes.GetComponent<Item>()))
        {
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Feet, null);
        }

        if (this.AddItemToInventory(otherInventory_.gloves.GetComponent<Item>()))
        {
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Hands, null);
        }

        if (this.AddItemToInventory(otherInventory_.cloak.GetComponent<Item>()))
        {
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Cloak, null);
        }

        if (this.AddItemToInventory(otherInventory_.necklace.GetComponent<Item>()))
        {
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Necklace, null);
        }

        if (this.AddItemToInventory(otherInventory_.ring.GetComponent<Item>()))
        {
            otherInventory_.ChangeArmorItemAtSlot(Armor.ArmorSlot.Ring, null);
        }

        if(this.AddItemToInventory(otherInventory_.rightHand.GetComponent<Item>()))
        {
            otherInventory_.ChangeWeaponItem(WeaponHand.Right, null);
        }

        if(this.AddItemToInventory(otherInventory_.leftHand.GetComponent<Item>()))
        {
            otherInventory_.ChangeWeaponItem(WeaponHand.Left, null);
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
                    otherInventory_.ChangeInventoryItemAtIndex(s, null);
                }
            }
        }

        //Calculating the new weight for this inventory and the other inventory
        this.FindArmorStats();
        otherInventory_.FindArmorStats();
    }


    //Equips a piece of armor to the correct slot
    public void EquipArmor(int armorInventoryIndex_)
    {
        //Reference to the piece of armor at the given inventory index
        Armor armorToEquip;
        if(this.itemSlots[armorInventoryIndex_] != null && this.itemSlots[armorInventoryIndex_].GetComponent<Armor>())
        {
            armorToEquip = this.itemSlots[armorInventoryIndex_].GetComponent<Armor>();
        }
        else
        {
            return;
        }

        //Finding the correct slot to equip the armor
        switch(armorToEquip.slot)
        {
            case Armor.ArmorSlot.Head:
                if (this.helm != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.helm.GetComponent<Item>();
                    this.helm = armorToEquip;
                }
                else
                {
                    this.helm = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;

            case Armor.ArmorSlot.Torso:
                if(this.chestPiece != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.chestPiece.GetComponent<Item>();
                    this.chestPiece = armorToEquip;
                }
                else
                {
                    this.chestPiece = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;

            case Armor.ArmorSlot.Legs:
                if (this.leggings != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.leggings.GetComponent<Item>();
                    this.leggings = armorToEquip;
                }
                else
                {
                    this.leggings = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;

            case Armor.ArmorSlot.Hands:
                if(this.gloves != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.gloves.GetComponent<Item>();
                    this.gloves = armorToEquip;
                }
                else
                {
                    this.gloves = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;

            case Armor.ArmorSlot.Feet:
                if (this.shoes != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.shoes.GetComponent<Item>();
                    this.shoes = armorToEquip;
                }
                else
                {
                    this.shoes = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;

            case Armor.ArmorSlot.Cloak:
                if (this.cloak != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.cloak.GetComponent<Item>();
                    this.cloak = armorToEquip;
                }
                else
                {
                    this.cloak = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;

            case Armor.ArmorSlot.Necklace:
                if (this.necklace != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.necklace.GetComponent<Item>();
                    this.necklace = armorToEquip;
                }
                else
                {
                    this.necklace = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;

            case Armor.ArmorSlot.Ring:
                if (this.ring != null)
                {
                    this.itemSlots[armorInventoryIndex_] = this.ring.GetComponent<Item>();
                    this.ring = armorToEquip;
                }
                else
                {
                    this.ring = armorToEquip;
                    this.itemSlots[armorInventoryIndex_] = null;
                }
                break;
        }
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
    public void EquipWeapon(int weaponInventoryIndex_)
    {
        //Getting the reference to the weapon at the given inventory index
        Weapon weaponToEquip_;
        if(this.itemSlots[weaponInventoryIndex_] != null && this.itemSlots[weaponInventoryIndex_].GetComponent<Weapon>())
        {
            weaponToEquip_ = this.itemSlots[weaponInventoryIndex_].GetComponent<Weapon>();
        }
        else
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
                    this.itemSlots[weaponInventoryIndex_] = null;
                }
                //If the right hand is full and the left isn't, the weapon is equipped in the left hand
                else if(this.leftHand == null && this.rightHand.size != Weapon.WeaponSize.TwoHands)
                {
                    this.leftHand = weaponToEquip_;
                    this.itemSlots[weaponInventoryIndex_] = null;
                }
                //If both hands are full, this weapon replaces the weapon in the right hand
                else
                {
                    this.itemSlots[weaponInventoryIndex_] = this.rightHand.GetComponent<Item>();
                    this.rightHand = weaponToEquip_;
                }
                break;

            case Weapon.WeaponSize.TwoHands:
                //If both hands are empty, the weapon is equipped in the right hand
                if (this.rightHand == null && this.leftHand == null)
                {
                    this.rightHand = weaponToEquip_;
                    this.itemSlots[weaponInventoryIndex_] = null;
                }
                //If both hands are full, the equipped weapons have to be moved to our inventory first
                else if(this.rightHand != null && this.leftHand != null)
                {
                    //If there's at least 1 empty spot in our inventory for the left hand item
                    if(this.CheckForEmptySlot() > 0)
                    {
                        this.UnequipWeapon(WeaponHand.Left);
                        this.itemSlots[weaponInventoryIndex_] = this.rightHand.GetComponent<Item>();
                        this.rightHand = weaponToEquip_;
                    }
                }
                //If only one hand is full, this weapon is equipped in the right hand and the held weapon replaces it in the inventory
                else
                {
                    //If the right hand is holding a weapon
                    if(this.rightHand != null)
                    {
                        this.itemSlots[weaponInventoryIndex_] = this.rightHand.GetComponent<Item>();
                        this.rightHand = weaponToEquip_;
                    }
                    //If the left hand is holding a weapon
                    else
                    {
                        this.itemSlots[weaponInventoryIndex_] = this.leftHand.GetComponent<Item>();
                        this.rightHand = weaponToEquip_;
                        this.leftHand = null;
                    }
                }
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

        //Finding the new inventory weight
        this.FindArmorStats();
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


    //Function used to just change one item in the inventory at the given index to something else and update the weight
    public void ChangeInventoryItemAtIndex(int index_, Item itemToChangeTo_)
    {
        //Making sure the index is within the correct limits of our inventory
        if(index_ < 0 || index_ >= this.itemSlots.Count)
        {
            return;
        }
        
        //Setting the item slot to contain the new item
        this.itemSlots[index_] = itemToChangeTo_;

        //If the slot isn't empty, this inventory becomes the item's parent
        if(itemToChangeTo_ != null)
        {
            itemToChangeTo_.transform.SetParent(this.transform);
        }

        //Updating this inventory's weight
        this.FindArmorStats();
    }


    //Function used to change the armor at the given slot to the new one
    public void ChangeArmorItemAtSlot(Armor.ArmorSlot slot_, Armor armorToChangeTo_)
    {
        //Making sure the armor to change to actually matches the slot it's being equipped to
        if(armorToChangeTo_!= null && armorToChangeTo_.slot != slot_)
        {
            return;
        }

        //Finding the correct slot to replace
        switch(slot_)
        {
            case Armor.ArmorSlot.Head:
                this.helm = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;

            case Armor.ArmorSlot.Torso:
                this.chestPiece = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;

            case Armor.ArmorSlot.Legs:
                this.leggings = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;

            case Armor.ArmorSlot.Feet:
                this.shoes = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;

            case Armor.ArmorSlot.Hands:
                this.gloves = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;

            case Armor.ArmorSlot.Necklace:
                this.necklace = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;

            case Armor.ArmorSlot.Cloak:
                this.cloak = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;

            case Armor.ArmorSlot.Ring:
                this.ring = armorToChangeTo_;
                if (armorToChangeTo_ != null)
                {
                    armorToChangeTo_.transform.SetParent(this.transform);
                }
                break;
        }

        //Updating this inventory's weight
        this.FindArmorStats();
    }


    //Function used to change the weapon at the given slot to the new one
    public void ChangeWeaponItem(WeaponHand handSlot_, Weapon weaponToChangeTo_)
    {
        if(handSlot_ == WeaponHand.Right)
        {
            this.rightHand = weaponToChangeTo_;

            //If the slot isn't empty, this inventory becomes the item's parent
            if(weaponToChangeTo_ != null)
            {
                weaponToChangeTo_.transform.SetParent(this.transform);
            }
        }
        else if(handSlot_ == WeaponHand.Left)
        {
            this.leftHand = weaponToChangeTo_;

            //If the slot isn't empty, this inventory becomes the item's parent
            if (weaponToChangeTo_ != null)
            {
                weaponToChangeTo_.transform.SetParent(this.transform);
            }
        }

        //Updating this inventory's weight
        this.FindArmorStats();
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
