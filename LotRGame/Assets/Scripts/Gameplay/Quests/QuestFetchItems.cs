using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by Quest to define what a fetch quest is
[System.Serializable]
public class QuestFetchItems
{
    //The number of items required to complete this quest
    public int itemsRequired = 1;
    //the number of items currently held for this quest
    [HideInInspector]
    public int currentItems = 0;

    //The list of items that quallify as being collected for this quest
    public Item collectableItem;



    //Function called externally to check if the party characters have enough collectable items
    public void CheckForQuestItems()
    {
        //Int to hold the number of found quest items in this party group's collective inventory
        uint collectedItemsCount = 0;

        //Looping through all of the characters in the currently selected party group
        foreach (Character partyCharacter in CharacterManager.globalReference.selectedGroup.charactersInParty)
        {
            //Looping through each inventory slot to check for the item
            for (int i = 0; i < partyCharacter.charInventory.itemSlots.Count; ++i)
            {
                //Making sure the slot isn't empty
                if (partyCharacter.charInventory.itemSlots[i] != null)
                {
                    //If the item in the current slot matches our collectable item it counts (as well as all of the ones stacked on it)
                    if (partyCharacter.charInventory.itemSlots[i].itemNameID == this.collectableItem.itemNameID)
                    {
                        collectedItemsCount += partyCharacter.charInventory.itemSlots[i].currentStackSize;

                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = this.itemsRequired;
                            return;
                        }
                    }
                }
            }

            //After checking the empty slots, we check weapon items held in hands
            if (this.collectableItem.GetComponent<Weapon>())
            {
                //Checking the right hand to see if it's empty
                if (partyCharacter.charInventory.rightHand != null)
                {
                    //If the item component on the right hand weapon matches our collectable, it counts
                    if (partyCharacter.charInventory.rightHand.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                    {
                        collectedItemsCount += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = this.itemsRequired;
                            return;
                        }
                    }
                }
                //Checking the left hand to see if it's empty
                if (partyCharacter.charInventory.leftHand != null)
                {
                    //If the item component on the left hand weapon matches our collectable, it counts
                    if (partyCharacter.charInventory.leftHand.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                    {
                        collectedItemsCount += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = this.itemsRequired;
                            return;
                        }
                    }
                }
            }


            //After checking the weapons, we check equipped armor
            if (this.collectableItem.GetComponent<Armor>())
            {
                //Checking the helm
                if (partyCharacter.charInventory.helm != null && partyCharacter.charInventory.helm.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
                //Checking the chest
                if (partyCharacter.charInventory.chestPiece != null && partyCharacter.charInventory.chestPiece.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
                //Checking the legs
                if (partyCharacter.charInventory.leggings != null && partyCharacter.charInventory.leggings.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
                //Checking the gloves
                if (partyCharacter.charInventory.gloves != null && partyCharacter.charInventory.gloves.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
                //Checking the shoes
                if (partyCharacter.charInventory.shoes != null && partyCharacter.charInventory.shoes.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
                //Checking the cloak
                if (partyCharacter.charInventory.cloak != null && partyCharacter.charInventory.cloak.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
                //Checking the necklace
                if (partyCharacter.charInventory.necklace != null && partyCharacter.charInventory.necklace.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
                //Checking the helm
                if (partyCharacter.charInventory.ring != null && partyCharacter.charInventory.ring.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
                {
                    collectedItemsCount += 1;
                    //If we've found enough collectable items for this quest, we stop this function
                    if (collectedItemsCount >= this.itemsRequired)
                    {
                        this.currentItems = this.itemsRequired;
                        return;
                    }
                }
            }
        }

        this.currentItems = (int)collectedItemsCount;
    }
}