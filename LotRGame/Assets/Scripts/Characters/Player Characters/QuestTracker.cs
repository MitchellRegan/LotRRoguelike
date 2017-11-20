using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterManager))]
public class QuestTracker : MonoBehaviour
{
    //The global reference for this component
    public static QuestTracker globalReference;

    //The list of starting quests for characters
    public List<QuestGiver> mainQuests;

    //The list of quests that we're currently tracking
    [HideInInspector]
    public List<Quest> questLog;



    //Function called when this object is created
    private void Awake()
    {
        //Setting the global reference for this component
        if(QuestTracker.globalReference == null)
        {
            QuestTracker.globalReference = this;
        }
        else
        {
            Destroy(this);
        }

        //Initializing our list of quests
        this.questLog = new List<Quest>();
    }
    

    //Function called from CreateTileManager.cs when a new game is started. Gives our character the default quests
    public void AssignMainQuests()
    {
        //Looping through all of the main quests
        foreach (QuestGiver mainQuest in this.mainQuests)
        {
            this.AddQuestToQuestLog(mainQuest.questToGive);
        }
    }


    //Function called externally to start tracking a quest
    public void AddQuestToQuestLog(Quest questToTrack_)
    {
        //Looping through all of the quests in our quest log
        foreach(Quest q in this.questLog)
        {
            //Checking to see if we are already tracking this quest
            if(q.questName == questToTrack_.questName)
            {
                //If we already have the quest, we do nothing
                return;
            }
        }

        //If the quest isn't already in our quest log, we start tracking it
        this.questLog.Add(questToTrack_);
    }


    //Function called externally to complete a quest at the given index
    public void CompleteQuestAtIndex(int questIndex_)
    {
        //Making sure the quest index is valid
        if(questIndex_ < this.questLog.Count && questIndex_ > -1)
        {
            //If the quest has a designated quest turn in location
            if(this.questLog[questIndex_].turnInQuestLoaction != null)
            {
                //Getting the tile that the player party is on
                TileInfo partyTile = CharacterManager.globalReference.selectedGroup.GetComponent<WASDOverworldMovement>().currentTile;
                //If the party tile doesn't have a decoration, it can't be a map location so nothing happens
                if (partyTile.decorationModel == null)
                {
                    return;
                }
                //If the tile has a decoration but it's not a map location, nothing happens
                else if(!partyTile.decorationModel.GetComponent<MapLocation>())
                {
                    return;
                }
                //If the tile has a map location but it isn't the turn in location, nothing happens
                else if(partyTile.decorationModel.GetComponent<MapLocation>().locationName != this.questLog[questIndex_].turnInQuestLoaction.locationName)
                {
                    return;
                }
            }
            Debug.Log("QuestTracker.CompleteQuestAtIndex, " + this.questLog[questIndex_].questName + " is completed!");
            Debug.Log("Need to implement quest rewards");
        }
    }


    //Function called externally to abandon a quest at the given index
    public void AbandonQuestAtIndex(int questIndex_)
    {
        //Making sure the quest index is valid
        if (questIndex_ < this.questLog.Count && questIndex_ > -1)
        {
            //Making sure the quest can be abandoned (some plot specific quests can't be dropped)
            if (this.questLog[questIndex_].canBeAbandoned)
            {
                Debug.Log("QuestTracker.AbandonQuestAtIndex, " + this.questLog[questIndex_].questName + " is abandoned!");
            }
        }
    }


    //Function called on time advance to update our quest timers
    public void UpdateQuestTimers()
    {
        //Getting the amount of time that has passed on this time advance
        int timePassed = TimePanelUI.globalReference.hoursAdvancedPerUpdate;

        //Looping through all of the quests in our quest log
        foreach(Quest q in this.questLog)
        {
            //Advancing time for each quest
            q.UpdateTimePassed(timePassed);
        }
    }


    //Function called from WASDOverworldMovement.cs to check if the tile the party is on is a destination to visit
    public void CheckTravelDestinations(TileInfo currentTile_)
    {
        //Looping through all of the quests in our quest log
        foreach (Quest q in this.questLog)
        {
            //Looping through each of the travel quests in the current quest
            foreach (QuestTravelDestination travelQuest in q.destinationList)
            {
                //Checking our current tile to see if it's a travel destination
                travelQuest.CheckTileForDestination(currentTile_);
            }
        }
    }


    //Function called from CombatManager.cs to check if any kill quest enemy was defeated
    public void UpdateKillQuests(Character killedEnemy_)
    {
        Debug.Log("Checking kill quest target");
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


    //Function called from CombatManager.cs to check if any escort quest character was killed
    public void CheckForDeadEscortCharacter(Character killedPartyCharacter_)
    {
        //Looping through all of the quests in our quest log
        foreach(Quest q in this.questLog)
        {
            //Looping through each of the escort quests in the current quest
            foreach(QuestEscortCharacter escortQuest in q.escortList)
            {
                //Checking the escort to see if the quest has failed
                escortQuest.CheckIfEscortCharacterDied(killedPartyCharacter_);

                //If the escort character is dead, the quest is failed
                if(escortQuest.isCharacterDead)
                {
                    q.isQuestFailed = true;
                }
            }
        }
    }
}


//Class used by QuestTracker.cs that defines what a quest is and stores player progress
[System.Serializable]
public class Quest
{
    //The name of this quest
    public string questName;
    //The description for this quest
    public string questDescription;

    //Bool that determines if this quest has been failed
    [HideInInspector]
    public bool isQuestFailed = false;

    //Bool that determines if this quest can be abandoned
    public bool canBeAbandoned = true;

    //The hour requirement for this quest. If failOnTimeReached is true, this is a countdown timer before failure. Otherwise it's a requirement to reach this time
    public int questTimeHours = 0;
    //The current number of hours that the escourt character has survived
    [HideInInspector]
    public int currentHours = 0;

    //Bool for if this quest fails when the quest time hours are reached
    public bool failOnTimeReached = true;

    //Map location where the player can turn in the quest. If null, it can be turned in anywhere
    public MapLocation turnInQuestLoaction = null;


    //The list of travel quests that are involved with this quest (go to location x)
    public List<QuestTravelDestination> destinationList;

    //The list of kill quests that are involved with this quest (kill X number of this enemy)
    public List<QuestKillRequirement> killList;

    //The list of fetch quests that are involved with this quest (get X number of this item)
    public List<QuestFetchItems> fetchList;

    //The list of escort quests that are involved with this quest (keep person X alive)
    public List<QuestEscortCharacter> escortList;



    //Function called externally to update the amount of time that's passed for our timer
    public void UpdateTimePassed(int timePassed_)
    {
        //If our escourt time is already at the time required, nothing happens
        if (this.questTimeHours == 0 || this.currentHours >= this.questTimeHours)
        {
            return;
        }

        //Updating the current hours
        this.currentHours += timePassed_;

        //If the current hours are greater than the required amount, we cap it off
        if (this.currentHours >= this.questTimeHours)
        {
            this.currentHours = this.questTimeHours;

            //If this quest fails when the timer is reached, we fail this quest
            if (this.failOnTimeReached)
            {
                this.isQuestFailed = true;
            }
        }
    }
}


//Class used by Quest to define what a travel quest is
[System.Serializable]
public class QuestTravelDestination
{
    //The map locatios that is required to visit for this quest
    public MapLocation requiredLocation;
    //Bool that determines if this location has been visited
    [HideInInspector]
    public bool locationVisited = false;


    
    //Function called externally to check if a visited tile has one of our required locations
    public void CheckTileForDestination(TileInfo visitedTile_)
    {
        //If we've already visited the location, nothing happens
        if(this.locationVisited)
        {
            return;
        }

        //Checking to make sure the tile and the decoration object aren't null (just in case)
        if (visitedTile_ != null && visitedTile_.decorationModel != null)
        {
            //Checking to see if the decoration model is a map location
            if (visitedTile_.decorationModel.GetComponent<MapLocation>())
            {
                //If we find a match, we remove the location from our required list and mark it as being visited
                if (visitedTile_.decorationModel.GetComponent<MapLocation>().locationName == this.requiredLocation.locationName)
                {
                    this.locationVisited = true;
                }
            }
        }
    }
}


//Class used by Quest to define what a kill quest is
[System.Serializable]
public class QuestKillRequirement
{
    //The number of kills required to complete this quest
    public int killsRequired = 1;
    //the number of currently completed kills for this quest
    [HideInInspector]
    public int currentKills = 0;

    //The enemy that quallifies as completing a kill for this quest
    public Character killableEnemy;



    //Function called externally to check if a killed character quallifies for this kill quest
    public void CheckKill(Character killedCharacter_)
    {
        //If we're already at our limit for required kills, nothing happens
        if(this.currentKills >= this.killsRequired)
        {
            return;
        }

        //If the killed character matches, the kill counts
        if (killedCharacter_.firstName == this.killableEnemy.firstName && killedCharacter_.lastName == this.killableEnemy.lastName &&
            killedCharacter_.sex == this.killableEnemy.sex && killedCharacter_.charRaceTypes.race == this.killableEnemy.charRaceTypes.race)
        {
            this.currentKills += 1;
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
    public Item collectableItem;



    //Function called externally to check if the party characters have enough collectable items
    public void CheckForQuestItems()
    {
        //Int to hold the number of found quest items in this party group's collective inventory
        uint collectedItemsCount = 0;

        //Looping through all of the characters in the currently selected party group
        foreach(Character partyCharacter in CharacterManager.globalReference.selectedGroup.charactersInParty)
        {
            //Looping through each inventory slot to check for the item
            for (int i = 0; i < partyCharacter.charInventory.itemSlots.Count; ++i)
            {
                //Making sure the slot isn't empty
                if(partyCharacter.charInventory.itemSlots[i] != null)
                {
                    //If the item in the current slot matches our collectable item it counts (as well as all of the ones stacked on it)
                    if(partyCharacter.charInventory.itemSlots[i].itemNameID == this.collectableItem.itemNameID)
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
            if(this.collectableItem.GetComponent<Weapon>())
            {
                //Checking the right hand to see if it's empty
                if(partyCharacter.charInventory.rightHand != null)
                {
                    //If the item component on the right hand weapon matches our collectable, it counts
                    if(partyCharacter.charInventory.rightHand.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                    if (partyCharacter.charInventory.leftHand.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
            if(this.collectableItem.GetComponent<Armor>())
            {
                //Checking the helm
                if (partyCharacter.charInventory.helm != null && partyCharacter.charInventory.helm.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                if (partyCharacter.charInventory.chestPiece != null && partyCharacter.charInventory.chestPiece.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                if (partyCharacter.charInventory.leggings != null && partyCharacter.charInventory.leggings.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                if (partyCharacter.charInventory.gloves != null && partyCharacter.charInventory.gloves.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                if (partyCharacter.charInventory.shoes != null && partyCharacter.charInventory.shoes.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                if (partyCharacter.charInventory.cloak != null && partyCharacter.charInventory.cloak.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                if (partyCharacter.charInventory.necklace != null && partyCharacter.charInventory.necklace.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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
                if (partyCharacter.charInventory.ring != null && partyCharacter.charInventory.ring.GetComponent<Item>().itemNameID == this.collectableItem.itemNameID)
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


//Class used by Quest to define what an escort quest is
[System.Serializable]
public class QuestEscortCharacter
{
    //The character that needs to be escorted
    public Character characterToEscort;
    //Bool that determines if this escourt character has died
    [HideInInspector]
    public bool isCharacterDead = false;

    
    //Function called externally to check if one of our escort characters was killed
    public void CheckIfEscortCharacterDied(Character deadCharacter_)
    {
        //If the dead character is the one we're escorting, the quest is failed
        if(this.characterToEscort == deadCharacter_)
        {
            this.isCharacterDead = true;
        }
    }
}