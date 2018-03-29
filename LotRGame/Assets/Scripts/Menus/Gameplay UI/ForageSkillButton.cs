﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForageSkillButton : MonoBehaviour
{
    //The number of hours that pass when this skill is used
    [Range(0, 9)]
    public int hoursPassed = 3;



    //Function called externally from UI buttons to roll for foraging skills
    public void ForagingSkillCheck()
    {
        //Telling the time panel how many hours will pass while foraging
        TimePanelUI.globalReference.AdvanceTime(this.hoursPassed);

        //Getting the reference to the foraging resources on the tile that the player party is currently on
        List<ResourceBlock> forageResources = PartyGroup.group1.GetComponent<WASDOverworldMovement>().currentTile.getForagingResources();

        //If the tile has no foraging resource blocks, nothing happens
        if(forageResources.Count == 0)
        {
            return;
        }

        //Int to hold the highest skill roll
        int highestRoll = 0;

        //Looping through each player character in the party
        for (int pc = 0; pc < PartyGroup.group1.charactersInParty.Count; ++pc)
        {
            //Making sure the current character slot isn't null
            if (PartyGroup.group1.charactersInParty[pc] != null)
            {
                //Rolling to see what the current character's skill check is
                int skillCheck = PartyGroup.group1.charactersInParty[pc].charSkills.survivalist;
                skillCheck += PartyGroup.group1.charactersInParty[pc].charSkills.survivalistMod;
                skillCheck += Random.Range(1, 100);

                //If the current skill check is higher than the current highest, this check becomes the new highest
                if (skillCheck > highestRoll)
                {
                    highestRoll = skillCheck;
                }
            }
        }

        Debug.Log("This is where we need to check foraging results. Highest roll: " + highestRoll);

        //Looping through this tile's foraging resource blocks to see what the highest success is
        int currentHighestIndex = -1;
        for (int f = 0; f < forageResources.Count; ++f)
        {
            //If the current encounter's skill check is less than or equal to the best roll
            if (forageResources[f].skillCheck <= highestRoll)
            {
                //If the current resource's skill is higher than the current highest resource's, it becomes the new highest
                if (forageResources[f].skillCheck > forageResources[currentHighestIndex].skillCheck)
                {
                    currentHighestIndex = f;
                }
            }
        }

        //If the current highest index is greater than -1, the player has succeeded and we reward them with loot
        if (currentHighestIndex > -1)
        {
            //Getting the reference to the bag inventory so we can open it in the UI
            Inventory lootInventory = InventoryOpener.globalReference.bagInventory;

            //Looping through each slot in the loot inventory and clearing it
            for (int s = 0; s < lootInventory.itemSlots.Count; ++s)
            {
                //If the current slot has something in it, we destroy the object
                if (lootInventory.itemSlots[s] != null)
                {
                    Destroy(lootInventory.itemSlots[s].gameObject);
                }

                //Setting the slot to null
                lootInventory.itemSlots[s] = null;
            }

            //Looping through each item in the foraging resource block
            for (int r = 0; r < forageResources[currentHighestIndex].resources.Count; ++r)
            {
                //Looping through to add the correct number of the gathered resource
                for (int n = 0; n < forageResources[currentHighestIndex].resourceQuantities[r]; ++n)
                {
                    //Instantiating a new version of the resource object
                    GameObject newFishResource = GameObject.Instantiate(forageResources[currentHighestIndex].resources[r].gameObject);
                    lootInventory.AddItemToInventory(newFishResource.GetComponent<Item>());
                }
            }

            //Activating the loot inventory's UI
            InventoryOpener.globalReference.bagInventoryUIObject.SetActive(true);
        }
    }
}