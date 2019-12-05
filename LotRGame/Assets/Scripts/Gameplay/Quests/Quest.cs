using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    //The list of item rewards for completing this quest
    public List<QuestItemReward> itemRewards;
    //The list of action rewards for completing this quest
    public List<QuestActionReward> actionRewards;
    //The perk reward for completing this quest
    public Perk perkReward;


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