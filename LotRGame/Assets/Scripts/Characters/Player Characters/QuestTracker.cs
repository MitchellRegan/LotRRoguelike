﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    //The list of names of quests we've completed
    [HideInInspector]
    public List<string> completedQuestNames;

    //Delegate event that listens for TimePassingEVT events
    private DelegateEvent<EVTData> advanceTimeListener;



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
        this.completedQuestNames = new List<string>();

        //Setting the event delegate
        this.advanceTimeListener = new DelegateEvent<EVTData>(this.UpdateQuestTimers);
    }


    //Telling the EventManager.cs to listen for time passing events
    private void OnEnable()
    {
        EventManager.StartListening(TimePassedEVT.eventNum, this.advanceTimeListener);
    }


    //Telling the EventManager.cs to stop listening for time passing events
    private void OnDisable()
    {
        EventManager.StopListening(TimePassedEVT.eventNum, this.advanceTimeListener);
    }
    

    //Function called from CreateTileManager.cs when a new game is started. Gives our character the default quests
    public void AssignMainQuests()
    {
        //Looping through all of the main quests
        foreach (QuestGiver mainQuest in this.mainQuests)
        {
            //If the quest has a quest item, we add the item to the first character's inventory
            if (mainQuest.GetComponent<Item>())
            {
                //Looping through all of the characters in the character party until we find one
                foreach(Character c in CharacterManager.globalReference.selectedGroup.charactersInParty)
                {
                    //If the current character isn't null
                    if(c != null)
                    {
                        //Creating a new instance of the item
                        GameObject questI = GameObject.Instantiate(mainQuest.gameObject) as GameObject;
                        Item questItem = questI.GetComponent<Item>();
                        questItem.itemPrefabRoot = mainQuest.gameObject;

                        //Adding the item to the character's inventory
                        c.charInventory.AddItemToInventory(questItem);

                        //Breaking this loop because we don't need to give the item to every character
                        break;
                    }
                }
            }
            //If the quest doesn't have an item, we just add it to the quest log
            else
            {
                this.AddQuestToQuestLog(mainQuest.questToGive);
            }
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

        //If the quest isn't already in our quest log, we create a new version of this quest
        Quest duplicateQuest = new Quest();
        //Duplicating the quest name, description, and timer variables
        duplicateQuest.questName = questToTrack_.questName;
        duplicateQuest.questDescription = questToTrack_.questDescription;
        duplicateQuest.isQuestFailed = false;
        duplicateQuest.canBeAbandoned = questToTrack_.canBeAbandoned;
        duplicateQuest.questTimeHours = questToTrack_.questTimeHours;
        duplicateQuest.currentHours = 0;
        duplicateQuest.failOnTimeReached = questToTrack_.failOnTimeReached;
        duplicateQuest.turnInQuestLoaction = questToTrack_.turnInQuestLoaction;
        //Duplicating all of the travel destinations if they exist
        duplicateQuest.destinationList = new List<QuestTravelDestination>();
        if (questToTrack_.destinationList.Count > 0)
        {
            foreach(QuestTravelDestination loc in questToTrack_.destinationList)
            {
                QuestTravelDestination dupLoc = new QuestTravelDestination();
                dupLoc.requiredLocation = loc.requiredLocation;
                dupLoc.locationVisited = false;
                duplicateQuest.destinationList.Add(dupLoc);
            }
        }
        //Duplicating all of the kill lists if they exist
        duplicateQuest.killList = new List<QuestKillRequirement>();
        if (questToTrack_.killList.Count > 0)
        {
            foreach(QuestKillRequirement kill in questToTrack_.killList)
            {
                QuestKillRequirement dupKill = new QuestKillRequirement();
                dupKill.killableEnemy = kill.killableEnemy;
                dupKill.killsRequired = kill.killsRequired;
                dupKill.currentKills = 0;
                duplicateQuest.killList.Add(dupKill);
            }
        }
        //Duplicating all of the fetch items if they exist
        duplicateQuest.fetchList = new List<QuestFetchItems>();
        if (questToTrack_.fetchList.Count > 0)
        {
            foreach(QuestFetchItems collectable in questToTrack_.fetchList)
            {
                QuestFetchItems dupCollect = new QuestFetchItems();
                dupCollect.collectableItem = collectable.collectableItem;
                dupCollect.itemsRequired = collectable.itemsRequired;
                dupCollect.currentItems = 0;
                duplicateQuest.fetchList.Add(dupCollect);
            }
        }
        //Duplicating all of the escort characters if they exist
        duplicateQuest.escortList = new List<QuestEscortCharacter>();
        if (questToTrack_.escortList.Count > 0)
        {
            foreach(QuestEscortCharacter esc in questToTrack_.escortList)
            {
                QuestEscortCharacter dupEsc = new QuestEscortCharacter();
                dupEsc.characterToEscort = esc.characterToEscort;
                dupEsc.isCharacterDead = false;
                duplicateQuest.escortList.Add(dupEsc);
            }
        }
        //Duplicating all of the item rewards if they exist
        duplicateQuest.itemRewards = new List<QuestItemReward>();
        if(questToTrack_.itemRewards.Count > 0)
        {
            foreach(QuestItemReward ir in questToTrack_.itemRewards)
            {
                QuestItemReward dupIR = new QuestItemReward();
                dupIR.rewardItem = ir.rewardItem;
                dupIR.amount = ir.amount;
                duplicateQuest.itemRewards.Add(dupIR);
            }
        }
        //Duplicating all of the action rewards if they exist
        duplicateQuest.actionRewards = new List<QuestActionReward>();
        if(questToTrack_.actionRewards.Count > 0)
        {
            foreach(QuestActionReward ar in questToTrack_.actionRewards)
            {
                QuestActionReward dupAR = new QuestActionReward();
                dupAR.rewardAction = ar.rewardAction;
                dupAR.rewardDistribution = ar.rewardDistribution;
                duplicateQuest.actionRewards.Add(dupAR);
            }
        }

        //Adding the duplicated quest to our quest log
        this.questLog.Add(duplicateQuest);
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

            //Adding this quest to our list of completed quests
            this.completedQuestNames.Add(this.questLog[questIndex_].questName);

            //Removing this quest from our quest log
            this.questLog.RemoveAt(questIndex_);
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
                //Removing this quest from our quest log
                this.questLog.RemoveAt(questIndex_);
            }
        }
    }


    //Function called on time advance to update our quest timers
    public void UpdateQuestTimers(EVTData data_)
    {
        //Making sure the data passed isn't null
        if (data_.timePassed != null)
        {
            //Getting the amount of time that has passed on this time advance
            int timePassed = data_.timePassed.timePassed;

            //Looping through all of the quests in our quest log
            foreach (Quest q in this.questLog)
            {
                //Advancing time for each quest
                q.UpdateTimePassed(timePassed);
            }
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


    //Function called externally to check if a specific quest is in our quest log
    public bool IsQuestInQuestLog(Quest questToCheck_)
    {
        //Looping through all of the quests in our quest log
        foreach(Quest q in this.questLog)
        {
            //If the current quest has the same name as the one we're looking for
            if(q.questName == questToCheck_.questName)
            {
                //We've found the quest and return true
                return true;
            }
        }

        //If we make it through all of our quests without finding it, we don't have it
        return false;
    }


    //Function called from SaveLoadManager.cs to load in saved quest data
    public void LoadQuestLogData(List<string> qSave_)
    {
        //Looping through each quest save data class that was loaded
        foreach(string loadQuest in qSave_)
        {
            //De-serializing the loaded quest string to a QuestSaveData class
            QuestSaveData questData = JsonUtility.FromJson(loadQuest, typeof(QuestSaveData)) as QuestSaveData;
            //Creating a new quest to hold the laoded data
            Quest q = new Quest();

            //Setting all of the basic quest info
            q.questName = questData.questName;
            q.questDescription = questData.questDescription;
            q.isQuestFailed = questData.isQuestFailed;
            q.canBeAbandoned = questData.canBeAbandoned;
            q.questTimeHours = questData.questHours;
            q.currentHours = questData.currentHours;
            q.failOnTimeReached = questData.failOnTimeReached;
            if (questData.turnInLocationObj == "")
            {
                q.turnInQuestLoaction = null;
            }
            else
            {
                PrefabIDTagData turnInLocData = JsonUtility.FromJson(questData.turnInLocationObj, typeof(PrefabIDTagData)) as PrefabIDTagData;
                GameObject turnInLocObj = IDManager.globalReference.GetPrefabFromID(turnInLocData.objType, turnInLocData.iDNumber);
                q.turnInQuestLoaction = turnInLocObj.GetComponent<MapLocation>();
            }

            //Looping through all of our travel destinations
            q.destinationList = new List<QuestTravelDestination>();
            for(int td = 0; td < questData.travelDestinations.Count; ++td)
            {
                //Getting a new quest destination class to add to our quest's list
                QuestTravelDestination questDestination = new QuestTravelDestination();

                //Getting the map location for this destination
                string loadLoc = questData.travelDestinations[td];
                PrefabIDTagData mapLocData = JsonUtility.FromJson(loadLoc, typeof(PrefabIDTagData)) as PrefabIDTagData;
                GameObject mapLocObj = IDManager.globalReference.GetPrefabFromID(mapLocData.objType, mapLocData.iDNumber);

                questDestination.requiredLocation = mapLocObj.GetComponent<MapLocation>();
                questDestination.locationVisited = questData.destinationTraveledTo[td];

                //Adding the quest destination to our quest
                q.destinationList.Add(questDestination);
            }

            //Looping through all of our kill lists
            q.killList = new List<QuestKillRequirement>();
            for(int kr = 0; kr < questData.killTargets.Count; ++kr)
            {
                //Getting a new quest kill class to add to our quest's list
                QuestKillRequirement questKill = new QuestKillRequirement();

                //Getting the character to kill for this requirement
                string loadKill = questData.killTargets[kr];
                GameObjectSerializationWrapper targetObj = JsonUtility.FromJson(loadKill, typeof(GameObjectSerializationWrapper)) as GameObjectSerializationWrapper;

                questKill.killableEnemy = targetObj.objToSave.GetComponent<Character>();
                questKill.killsRequired = questData.requiredKillAmount[kr];
                questKill.currentKills = questData.currentKillAmount[kr];

                //Adding the kill requirement to our quest
                q.killList.Add(questKill);
            }

            //Looping through all of our fetch items
            q.fetchList = new List<QuestFetchItems>();
            for(int fi = 0; fi < questData.fetchItems.Count; ++fi)
            {
                //Getting a new fetch item class to add to our quest's list
                QuestFetchItems questFetch = new QuestFetchItems();

                //Getting the item to collect for this requirement
                string loadItem = questData.fetchItems[fi];
                PrefabIDTagData itemData = JsonUtility.FromJson(loadItem, typeof(PrefabIDTagData)) as PrefabIDTagData;
                GameObject itemObj = IDManager.globalReference.GetPrefabFromID(itemData.objType, itemData.iDNumber);

                questFetch.collectableItem = itemObj.GetComponent<Item>();
                questFetch.itemsRequired = questData.requiredItemAmount[fi];
                questFetch.currentItems = questData.currentItemAmount[fi];

                //Adding the item requirement to our quest
                q.fetchList.Add(questFetch);
            }

            //Looping through all of our escort characters
            q.escortList = new List<QuestEscortCharacter>();
            for(int ec = 0; ec < questData.escortCharacters.Count; ++ec)
            {
                //Getting a new escort class to add to our quest's list
                QuestEscortCharacter questEscort = new QuestEscortCharacter();

                //Getting the character to escort for this requirement
                string loadChar = questData.escortCharacters[ec];
                GameObjectSerializationWrapper charObj = JsonUtility.FromJson(loadChar, typeof(GameObjectSerializationWrapper)) as GameObjectSerializationWrapper;

                questEscort.characterToEscort = charObj.objToSave.GetComponent<Character>();
                questEscort.isCharacterDead = questData.areEscortsDead[ec];

                //Adding the escort requirement to our quest
                q.escortList.Add(questEscort);
            }

            //Looping through all of our item rewards
            q.itemRewards = new List<QuestItemReward>();
            for(int ir = 0; ir < questData.itemRewards.Count; ++ir)
            {
                //Getting a new item reward class to add to our quest's list
                QuestItemReward itemReward = new QuestItemReward();

                //Getting the item that is awarded
                string loadItem = questData.itemRewards[ir];
                PrefabIDTagData itemData = JsonUtility.FromJson(loadItem, typeof(PrefabIDTagData)) as PrefabIDTagData;
                GameObject itemObj = IDManager.globalReference.GetPrefabFromID(itemData.objType, itemData.iDNumber);

                itemReward.rewardItem = itemObj.GetComponent<Item>();
                itemReward.amount = questData.itemRewardAmounts[ir];

                //Adding the item reward to our quest
                q.itemRewards.Add(itemReward);
            }

            //Looping through all of our action rewards
            q.actionRewards = new List<QuestActionReward>();
            for(int ar = 0; ar < questData.actionRewards.Count; ++ar)
            {
                //Getting a new action reward class to add to our quest's list
                QuestActionReward actionReward = new QuestActionReward();

                //Getting the action object that is awarded
                string loadAction = questData.actionRewards[ar];
                PrefabIDTagData actionData = JsonUtility.FromJson(loadAction, typeof(PrefabIDTagData)) as PrefabIDTagData;
                GameObject actionObj = IDManager.globalReference.GetPrefabFromID(actionData.objType, actionData.iDNumber);

                actionReward.rewardAction = actionObj.GetComponent<Action>();
                actionReward.rewardDistribution = questData.actionDistributionTypes[ar];

                //Adding the action reward to our quests
                q.actionRewards.Add(actionReward);
            }


            //Adding the loaded quest to our quest log
            this.questLog.Add(q);
        }
    }
}

