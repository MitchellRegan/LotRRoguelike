using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used to save quest data
[System.Serializable]
public class QuestSaveData
{
    //Variables for the quest's basic info
    public string questName;
    public string questDescription;
    public bool isQuestFailed;
    public bool canBeAbandoned;
    public int questHours;
    public int currentHours;
    public bool failOnTimeReached;
    public string turnInLocationObj;

    //Variables for travel destination quests
    public List<string> travelDestinations;
    public List<bool> destinationTraveledTo;
    //Variables for kill quests
    public List<string> killTargets;
    public List<int> requiredKillAmount;
    public List<int> currentKillAmount;
    //Variables for fetch quests
    public List<string> fetchItems;
    public List<int> requiredItemAmount;
    public List<int> currentItemAmount;
    //Variables for escort quests
    public List<string> escortCharacters;
    public List<bool> areEscortsDead;
    //Variables for item rewards
    public List<string> itemRewards;
    public List<int> itemRewardAmounts;
    //Variables for action rewards
    public List<string> actionRewards;
    public List<DistributionType> actionDistributionTypes;


    //Constructor for this class
    public QuestSaveData(Quest ourQuest_)
    {
        //Saving the basic quest info
        this.questName = ourQuest_.questName;
        this.questDescription = ourQuest_.questDescription;
        this.isQuestFailed = ourQuest_.isQuestFailed;
        this.canBeAbandoned = ourQuest_.canBeAbandoned;
        this.questHours = ourQuest_.questTimeHours;
        this.currentHours = ourQuest_.currentHours;
        this.failOnTimeReached = ourQuest_.failOnTimeReached;

        //Serializing the turn in location if it exists
        if (ourQuest_.turnInQuestLoaction != null)
        {
            PrefabIDTagData turnInLocData = new PrefabIDTagData(ourQuest_.turnInQuestLoaction.GetComponent<IDTag>());
            this.turnInLocationObj = JsonUtility.ToJson(turnInLocData, true);
        }
        else
        {
            this.turnInLocationObj = "";
        }

        //Looping through all of our travel destinations
        this.travelDestinations = new List<string>();
        this.destinationTraveledTo = new List<bool>();
        foreach (QuestTravelDestination td in ourQuest_.destinationList)
        {
            PrefabIDTagData destinationLocData = new PrefabIDTagData(td.requiredLocation.GetComponent<IDTag>());
            this.travelDestinations.Add(JsonUtility.ToJson(destinationLocData, true));
            this.destinationTraveledTo.Add(td.locationVisited);
        }

        //Looping through all of our kill requirements
        this.killTargets = new List<string>();
        this.requiredKillAmount = new List<int>();
        this.currentKillAmount = new List<int>();
        foreach (QuestKillRequirement kr in ourQuest_.killList)
        {
            GameObject targetPrefab = UnityEditor.PrefabUtility.FindPrefabRoot(kr.killableEnemy.gameObject);
            this.killTargets.Add(JsonUtility.ToJson(new GameObjectSerializationWrapper(targetPrefab), true));
            this.requiredKillAmount.Add(kr.killsRequired);
            this.currentKillAmount.Add(kr.currentKills);
        }

        //Looping through all of our fetch lists
        this.fetchItems = new List<string>();
        this.requiredItemAmount = new List<int>();
        this.currentItemAmount = new List<int>();
        foreach (QuestFetchItems fi in ourQuest_.fetchList)
        {
            PrefabIDTagData itemTagData = new PrefabIDTagData(fi.collectableItem.GetComponent<IDTag>());
            this.fetchItems.Add(JsonUtility.ToJson(itemTagData, true));
            this.requiredItemAmount.Add(fi.itemsRequired);
            this.currentItemAmount.Add(fi.currentItems);
        }

        //Looping through all of our escort lists
        this.escortCharacters = new List<string>();
        this.areEscortsDead = new List<bool>();
        foreach (QuestEscortCharacter ec in ourQuest_.escortList)
        {
            GameObject characterPrefab = UnityEditor.PrefabUtility.FindPrefabRoot(ec.characterToEscort.gameObject);
            this.escortCharacters.Add(JsonUtility.ToJson(new GameObjectSerializationWrapper(characterPrefab), true));
            this.areEscortsDead.Add(ec.isCharacterDead);
        }

        //Looping through all of our reward items
        this.itemRewards = new List<string>();
        this.itemRewardAmounts = new List<int>();
        foreach (QuestItemReward ir in ourQuest_.itemRewards)
        {
            PrefabIDTagData itemTagData = new PrefabIDTagData(ir.rewardItem.GetComponent<IDTag>());
            this.itemRewards.Add(JsonUtility.ToJson(itemTagData, true));
            this.itemRewardAmounts.Add(ir.amount);
        }

        //Looping through all of our reward actions
        this.actionRewards = new List<string>();
        this.actionDistributionTypes = new List<DistributionType>();
        foreach (QuestActionReward ar in ourQuest_.actionRewards)
        {
            this.actionRewards.Add(JsonUtility.ToJson(new PrefabIDTagData(ar.rewardAction.GetComponent<IDTag>()), true));
            this.actionDistributionTypes.Add(ar.rewardDistribution);
        }
    }
}
