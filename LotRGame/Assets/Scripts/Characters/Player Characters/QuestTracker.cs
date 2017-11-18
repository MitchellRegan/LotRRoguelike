using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterManager))]
public class QuestTracker : MonoBehaviour
{
    //The list of quests that we're currently tracking
    [HideInInspector]
    public List<Quest> questLog;


	
    //Function called from CombatManager.cs to check if any kill quest enemy was defeated
    public void UpdateKillQuests(Character killedEnemy_)
    {
        //Looping through all of our quests in the quest log
        foreach(Quest q in this.questLog)
        {
            //Looping through all of the kill quests in the current quest
            foreach(QuestKillRequirement killQuest in q.killList)
            {
                //Telling the kill quest to see if the killed enemy counts
                killQuest.CheckKill(killedEnemy_);
            }
        }
    }
    

    //Function called from Inventory.cs to check if any fetch quest items have been picked up
    public void UpdateFetchQuests()
    {
        //Looping through all of the quests in our quest log
        foreach(Quest q in this.questLog)
        {
            //Looping through each of the fetch quests in the current quest
            foreach(QuestFetchItems fetchQuest in q.fetchList)
            {
                //Checking the collective character inventories for the quest items
                fetchQuest.CheckForQuestItems();
            }
        }
    }


    //Function called from WASDOverworldMovement.cs to check if the tile the party is on is a destination to visit
    public void CheckTravelDestinations(TileInfo currentTile_)
    {
        //Looping through all of the quests in our quest log
        foreach(Quest q in this.questLog)
        {
            //Looping through each of the travel quests in the current quest
            foreach(QuestTravelDestination travelQuest in q.destinationList)
            {
                //Checking our current tile to see if it's a travel destination
                travelQuest.CheckTileForDestination(currentTile_);
            }
        }
    }
}

//Go to place (call from WASDOverworldMovement)
//Escort? Get character added to party who has to survive?

//Class used by QuestTracker.cs that defines what a quest is and stores player progress
[System.Serializable]
public class Quest
{
    //The name of this quest
    public string questName;
    //The description for this quest
    public string questDescription;

    //The list of kill quests that are involved with this quest (kill X number of this enemy)
    public List<QuestKillRequirement> killList;

    //The list of fetch quests that are involved with this quest (get X number of this item)
    public List<QuestFetchItems> fetchList;

    //The list of travel quests that are involved with this quest (go to location x)
    public List<QuestTravelDestination> destinationList;
}


//Class used by Quest to define what a kill quest is
public class QuestKillRequirement
{
    //The number of kills required to complete this quest
    public int killsRequired = 1;
    //the number of currently completed kills for this quest
    [HideInInspector]
    public int currentKills = 0;

    //The types of enemies that quallify as completing a kill for this quest
    public List<Character> killableEnemies;
    //The types of races that quallify as completing a kill for this quest
    public List<RaceTypes.Races> killableRaces;
    //The list of subtypes that quallify as completing a kill for this quest
    public List<RaceTypes.Subtypes> killableTypes;



    //Constructor for this class to initialize our lists
    public QuestKillRequirement()
    {
        this.killableEnemies = new List<Character>();
        this.killableRaces = new List<RaceTypes.Races>();
        this.killableTypes = new List<RaceTypes.Subtypes>();
    }


    //Function called externally to check if a killed character quallifies for this kill quest
    public void CheckKill(Character killedCharacter_)
    {
        //If we're already at our limit for required kills, nothing happens
        if(this.currentKills >= this.killsRequired)
        {
            return;
        }

        //If there's no required enemy, required race, or required subtype, the kill counts automatically
        if(this.killableEnemies.Count == 0 && this.killableRaces.Count == 0 && this.killableTypes.Count == 0)
        {
            this.currentKills += 1;
        }
        //If there are required enemies
        else if(this.killableEnemies.Count > 0)
        {
            //Looping through each type of killable enemy to see if any match
            foreach(Character quallifiedKill in this.killableEnemies)
            {
                //If the killed character matches, the kill counts
                if(killedCharacter_.firstName == quallifiedKill.firstName && killedCharacter_.lastName == quallifiedKill.lastName &&
                    killedCharacter_.sex == quallifiedKill.sex && killedCharacter_.charRaceTypes.race == quallifiedKill.charRaceTypes.race)
                {
                    this.currentKills += 1;
                    break;
                }
            }
        }
        //If there are no required enemies and no required subtypes
        else if(this.killableEnemies.Count == 0 && this.killableRaces.Count > 0 && this.killableTypes.Count == 0)
        {
            //If the killed race matches, the kill counts
            if(this.killableRaces.Contains(killedCharacter_.charRaceTypes.race))
            {
                this.currentKills += 1;
            }
        }
        //If there are no required enemies and no required races
        else if (this.killableEnemies.Count == 0 && this.killableRaces.Count == 0 && this.killableTypes.Count > 0)
        {
            //If the killed character has no subtypes, the kill doesn't count
            if(killedCharacter_.charRaceTypes.subtypeList.Count == 0)
            {
                return;
            }

            //Looping though all of the killed character subtypes
            foreach(RaceTypes.Subtypes type in killedCharacter_.charRaceTypes.subtypeList)
            {
                //If the killed subtype matches, the kill counts
                if (this.killableTypes.Contains(type))
                {
                    this.currentKills += 1;
                }
            }
        }
        //If there are no required enemies but both races and subtypes are required
        else if (this.killableEnemies.Count == 0 && this.killableRaces.Count > 0 && this.killableTypes.Count > 0)
        {
            //If the killed character has no subtypes, the kill doesn't count
            if (killedCharacter_.charRaceTypes.subtypeList.Count == 0)
            {
                return;
            }

            ////If the killed race matches
            if(this.killableRaces.Contains(killedCharacter_.charRaceTypes.race))
            {
                //Looping though all of the killed character subtypes
                foreach (RaceTypes.Subtypes type in killedCharacter_.charRaceTypes.subtypeList)
                {
                    //If the killed subtype matches, the kill counts
                    if (this.killableTypes.Contains(type))
                    {
                        this.currentKills += 1;
                    }
                }
            }
        }
    }
}


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
    public List<Item> collectableItems;



    //Constructor for this class to initialize our list
    public QuestFetchItems()
    {
        this.collectableItems = new List<Item>();
    }


    //Function called externally to check if the party characters have enough collectable items
    public void CheckForQuestItems()
    {
        //Int to hold the number of found quest items in this party group's collective inventory
        uint collectedItemsCount = 0;

        //Looping through all of the characters in the currently selected party group
        foreach(Character partyCharacter in CharacterManager.globalReference.selectedGroup.charactersInParty)
        {
            //Looping through each of the collectable items for this quest
            foreach (Item collectable in this.collectableItems)
            {
                //Looping through each inventory slot to check for the item
                for (int i = 0; i < partyCharacter.charInventory.itemSlots.Count; ++i)
                {
                    //Making sure the slot isn't empty
                    if(partyCharacter.charInventory.itemSlots[i] != null)
                    {
                        //If the item in the current slot matches our collectable item it counts (as well as all of the ones stacked on it)
                        if(partyCharacter.charInventory.itemSlots[i].itemNameID == collectable.itemNameID)
                        {
                            collectedItemsCount += partyCharacter.charInventory.itemSlots[i].currentStackSize;
                            
                            //If we've found enough collectable items for this quest, we stop this function
                            if(collectedItemsCount >= this.itemsRequired)
                            {
                                this.currentItems = checked((int)collectedItemsCount);
                                return;
                            }
                        }
                    }
                }

                //After checking the empty slots, we check weapon items held in hands
                if(collectable.GetComponent<Weapon>())
                {
                    //Checking the right hand to see if it's empty
                    if(partyCharacter.charInventory.rightHand != null)
                    {
                        //If the item component on the right hand weapon matches our collectable, it counts
                        if(partyCharacter.charInventory.rightHand.GetComponent<Item>().itemNameID == collectable.itemNameID)
                        {
                            this.currentItems += 1;
                            //If we've found enough collectable items for this quest, we stop this function
                            if (collectedItemsCount >= this.itemsRequired)
                            {
                                this.currentItems = checked((int)collectedItemsCount);
                                return;
                            }
                        }
                    }
                    //Checking the left hand to see if it's empty
                    if (partyCharacter.charInventory.leftHand != null)
                    {
                        //If the item component on the left hand weapon matches our collectable, it counts
                        if (partyCharacter.charInventory.leftHand.GetComponent<Item>().itemNameID == collectable.itemNameID)
                        {
                            this.currentItems += 1;
                            //If we've found enough collectable items for this quest, we stop this function
                            if (collectedItemsCount >= this.itemsRequired)
                            {
                                this.currentItems = checked((int)collectedItemsCount);
                                return;
                            }
                        }
                    }
                }


                //After checking the weapons, we check equipped armor
                if(collectable.GetComponent<Armor>())
                {
                    //Checking the helm
                    if (partyCharacter.charInventory.helm != null && partyCharacter.charInventory.helm.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                    //Checking the chest
                    if (partyCharacter.charInventory.chestPiece != null && partyCharacter.charInventory.chestPiece.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                    //Checking the legs
                    if (partyCharacter.charInventory.leggings != null && partyCharacter.charInventory.leggings.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                    //Checking the gloves
                    if (partyCharacter.charInventory.gloves != null && partyCharacter.charInventory.gloves.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                    //Checking the shoes
                    if (partyCharacter.charInventory.shoes != null && partyCharacter.charInventory.shoes.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                    //Checking the cloak
                    if (partyCharacter.charInventory.cloak != null && partyCharacter.charInventory.cloak.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                    //Checking the necklace
                    if (partyCharacter.charInventory.necklace != null && partyCharacter.charInventory.necklace.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                    //Checking the helm
                    if (partyCharacter.charInventory.ring != null && partyCharacter.charInventory.ring.GetComponent<Item>().itemNameID == collectable.itemNameID)
                    {
                        this.currentItems += 1;
                        //If we've found enough collectable items for this quest, we stop this function
                        if (collectedItemsCount >= this.itemsRequired)
                        {
                            this.currentItems = checked((int)collectedItemsCount);
                            return;
                        }
                    }
                }
            }
        }
    }
}


//Class used by Quest to define what a travel quest is
[System.Serializable]
public class QuestTravelDestination
{
    //The list of map locations that are required to visit for this quest
    public List<MapLocation> requiredLocations;
    //The list of map locations that have been visited
    [HideInInspector]
    public List<MapLocation> visitedLocations;



    //Constructor for this class to initiate our lists
    public QuestTravelDestination()
    {
        this.requiredLocations = new List<MapLocation>();
        this.visitedLocations = new List<MapLocation>();
    }


    //Function called externally to check if a visited tile has one of our required locations
    public void CheckTileForDestination(TileInfo visitedTile_)
    {
        //If there are no more map locations to visit, nothing happens
        if(this.requiredLocations.Count == 0)
        {
            return;
        }

        //Checking to make sure the tile and the decoration object aren't null (just in case)
        if(visitedTile_ != null && visitedTile_.decorationModel != null)
        {
            //Checking to see if the decoration model is a map location
            if(visitedTile_.decorationModel.GetComponent<MapLocation>())
            {
                //Looping through all of our required locations to see if any of them match
                foreach(MapLocation destination in this.requiredLocations)
                {
                    //If we find a match, we remove the location from our required list and mark it as being visited
                    if(visitedTile_.decorationModel.GetComponent<MapLocation>().locationName == destination.locationName)
                    {
                        this.visitedLocations.Add(destination);
                        this.requiredLocations.Remove(destination);
                        break;
                    }
                }
            }
        }
    }
}