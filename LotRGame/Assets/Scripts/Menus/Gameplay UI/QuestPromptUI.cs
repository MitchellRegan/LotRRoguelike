using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPromptUI : MonoBehaviour
{
    //The Quest that we're prompting to give the player
    [HideInInspector]
    public Quest questToPrompt;
    //The UI panel that's turned on and off
    public GameObject promptUI;

    //The reference to the scroll view's content rect transform that is scaled 
    public RectTransform descriptionContentTransform;
    //The text reference for the displayed quest's name
    public Text questNameText;
    //The text reference for the displayed quest's description
    public Text questDescriptionText;
    //The text reference for the displayed quest's objectives
    public Text questObjectivesText;
    //The text reference for the displayed quest's rewards
    public Text questRewardText;
    //The gap in space between the text for quest descriptions
    public float textDescriptionGap = 40;

    [Space(8)]
    
    //The button that allows the player to decline the quest
    public Button declineQuestButton;

    //Delegate event called from the EventManager.cs to display a quest
    private DelegateEvent<EVTData> displayPromptEVT;



    //Function called when this object is created
    private void Awake()
    {
        //Setting our delegate event
        this.displayPromptEVT = new DelegateEvent<EVTData>(this.DisplayQuestPrompt);
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        //Telling the Event Manager to listen for our delegate
        EventManager.StartListening(PromptQuestEVT.eventName, this.displayPromptEVT);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        //Telling the Event Manager to stop listening for our delegate
        EventManager.StopListening(PromptQuestEVT.eventName, this.displayPromptEVT);
    }


    //Function called from the EventManager.cs to display this prompt UI for a given quest
    public void DisplayQuestPrompt(EVTData data_)
    {
        //If the quest giver is null, nothing happens
        if(data_.promptQuest == null || data_.promptQuest.questToPrompt == null)
        {
            return;
        }

        //Making sure the quest isn't one that we've already completed
        if(QuestTracker.globalReference.completedQuestNames.Contains(this.questToPrompt.questName))
        {
            return;
        }

        //Displaying the prompt
        this.promptUI.SetActive(true);

        //Setting the given quest as our own to display
        this.questToPrompt = data_.promptQuest.questToPrompt;

        //Setting the displayed quest's text name
        this.questNameText.text = this.questToPrompt.questName;
        //Setting the displayed quest's description text
        this.questDescriptionText.text = this.questToPrompt.questDescription;
        //Setting the displayed quest's objective text
        this.questObjectivesText.text = this.GetQuestObjectiveText(this.questToPrompt);
        //Setting the displayed quest's reward text
        this.questRewardText.text = this.GetQuestRewardText(this.questToPrompt);

        //Refreshing the canvases so the ContentSizeFitter components on the text resizes the text box sizes
        Canvas.ForceUpdateCanvases();

        //Setting the description text's rect transform position just below the quest name text
        float descriptionPos = this.questNameText.GetComponent<RectTransform>().localPosition.y;
        descriptionPos -= this.questNameText.GetComponent<RectTransform>().rect.height;
        this.questDescriptionText.GetComponent<RectTransform>().localPosition = new Vector3(this.questDescriptionText.GetComponent<RectTransform>().localPosition.x,
                                                                                            descriptionPos,
                                                                                            0);

        //Setting the objective text's rect transform position just below the quest description text
        descriptionPos -= this.questDescriptionText.GetComponent<RectTransform>().rect.height + this.textDescriptionGap;
        this.questObjectivesText.GetComponent<RectTransform>().localPosition = new Vector3(this.questObjectivesText.GetComponent<RectTransform>().localPosition.x,
                                                                                            descriptionPos,
                                                                                            0);

        //Setting the reward text's rect transform position just below the quest objective text
        descriptionPos -= this.questObjectivesText.GetComponent<RectTransform>().rect.height + this.textDescriptionGap;
        this.questRewardText.GetComponent<RectTransform>().localPosition = new Vector3(this.questRewardText.GetComponent<RectTransform>().localPosition.x,
                                                                                       descriptionPos,
                                                                                       0);

        //Resizing the discription content view size
        float contentSize = 0;
        contentSize += this.questNameText.GetComponent<RectTransform>().rect.height;
        contentSize += this.questDescriptionText.GetComponent<RectTransform>().rect.height;
        contentSize += this.textDescriptionGap;
        contentSize += this.questObjectivesText.GetComponent<RectTransform>().rect.height;
        contentSize += this.textDescriptionGap;
        contentSize += this.questRewardText.GetComponent<RectTransform>().rect.height;
        this.descriptionContentTransform.sizeDelta = new Vector2(0, contentSize);

        //If the quest can't be abandoned, the quest can't be declined
        if(!this.questToPrompt.canBeAbandoned)
        {
            this.declineQuestButton.interactable = false;
        }
        //If the quest can be abandoned, the player can choose to decline it
        else
        {
            this.declineQuestButton.interactable = true;
        }
    }


    //Function called from DisplayQuestDescription to get the text for the quest objectives
    private string GetQuestObjectiveText(Quest quest_)
    {
        //The string that we return
        string objectiveText = "\n";


        //If the quest is failed, we display it
        if (quest_.isQuestFailed)
        {
            objectiveText += "    QUEST FAILED! \n \n";
        }

        //Setting the quest timer if the timer is required
        if (quest_.questTimeHours > 0)
        {
            //If the timer is counting down to failure
            if (quest_.failOnTimeReached)
            {
                objectiveText += "Time remaining  - " + (quest_.questTimeHours - quest_.currentHours);
                if (quest_.questTimeHours - quest_.currentHours == 1)
                {
                    objectiveText += " hour";
                }
                else
                {
                    objectiveText += " hours";
                }
                if (quest_.currentHours == quest_.questTimeHours)
                {
                    objectiveText += " - FAILED";
                }
            }
            //If the timer is counting up to survive x hours
            else
            {
                objectiveText += "Survive  = " + quest_.currentHours + "/" + quest_.questTimeHours + " hours";
            }
            objectiveText += "\n";
        }

        //Looping through each of the quest location destinations
        if (quest_.destinationList.Count > 0)
        {
            foreach (QuestTravelDestination loc in quest_.destinationList)
            {
                //Telling the player to visit the location name
                objectiveText += "Travel to " + loc.requiredLocation.locationName;
                if (loc.locationVisited)
                {
                    objectiveText += "  - Completed";
                }
                objectiveText += "\n";
            }
        }

        //Looping through each of the kill quests
        if (quest_.killList.Count > 0)
        {
            foreach (QuestKillRequirement kill in quest_.killList)
            {
                //Telling the player to kill a number of enemies
                objectiveText += "Kill " + kill.killableEnemy.firstName + " ";
                if (kill.killableEnemy.lastName != "")
                {
                    objectiveText += kill.killableEnemy.lastName + " ";
                }
                objectiveText += " - " + kill.currentKills + "/" + kill.killsRequired + "\n";
            }
        }

        //Looping through each of the fetch quests
        if (quest_.fetchList.Count > 0)
        {
            foreach (QuestFetchItems fetch in quest_.fetchList)
            {
                //Telling the player to collect a number of items
                objectiveText += "Collect " + fetch.collectableItem.itemNameID + "  - " + fetch.currentItems + "/" + fetch.itemsRequired + "\n";
            }
        }

        //Looping through each of the escort quests
        if (quest_.escortList.Count > 0)
        {
            foreach (QuestEscortCharacter escort in quest_.escortList)
            {
                //Telling the player to protect a character
                objectiveText += "Protect " + escort.characterToEscort.firstName;
                if (escort.characterToEscort.lastName != "")
                {
                    objectiveText += " " + escort.characterToEscort.lastName;
                }
                if (escort.isCharacterDead)
                {
                    objectiveText += "  - FAILED";
                }
                objectiveText += "\n";
            }
        }

        //Returning the completed objective list text
        return objectiveText;
    }


    //Function called from DisplayQuestDescription to get the text for the quest rewards
    private string GetQuestRewardText(Quest quest_)
    {
        //If there are no rewards, nothing happens
        if (quest_.itemRewards.Count == 0 && quest_.actionRewards.Count == 0)
        {
            return "";
        }

        //The string that we return
        string rewardText = "\n";
        rewardText += "    Rewards:\n";

        //Looping through each of the item rewards
        if (quest_.itemRewards.Count > 0)
        {
            foreach (QuestItemReward ir in quest_.itemRewards)
            {
                rewardText += "Item  - " + ir.rewardItem.itemNameID + "  x" + ir.amount + "\n";
            }
        }

        //Looping through each of the action rewards 
        if (quest_.actionRewards.Count > 0)
        {
            foreach (QuestActionReward ar in quest_.actionRewards)
            {
                //If the action is a move action
                if (ar.rewardAction.GetType() == typeof(MoveAction))
                {
                    rewardText += "Move Action  - " + ar.rewardAction.actionName;
                }
                //If the action is a spell action
                else if (ar.rewardAction.GetType() == typeof(SpellAction))
                {
                    rewardText += "Spell Action  - " + ar.rewardAction.actionName;
                }
                //If the action is an attack action
                else if (ar.rewardAction.GetType() == typeof(AttackAction))
                {
                    rewardText += "Attack Action  - " + ar.rewardAction.actionName;
                }

                //Adding a description so the player knows how the ability is distributed
                switch (ar.rewardDistribution)
                {
                    case QuestActionReward.DistributionType.Everyone:
                        rewardText += "(All Party Members)\n";
                        break;

                    case QuestActionReward.DistributionType.OneRandomCharacter:
                        rewardText += "(Random Party Member)\n";
                        break;

                    case QuestActionReward.DistributionType.PlayerChoice:
                        rewardText += "\n";
                        break;
                }
            }
        }

        //Returning the completed reward list text
        return rewardText;
    }


    //Function called externally to accept the quest and add it to the player's quest log
    public void AcceptQuest()
    {
        //Adding the quest to the player's quest log
        QuestTracker.globalReference.AddQuestToQuestLog(this.questToPrompt);

        //Clearing our UI info and hiding the prompt
        this.ClearPromptUI();
    }


    //Function called externally to decline the quest
    public void DeclineQuest()
    {
        //Clearing our UI info and hiding the prompt
        this.ClearPromptUI();
    }


    //Function called from AcceptQuest and DeclineQuest to clear the UI
    private void ClearPromptUI()
    {
        //Clearing all of the text info for the quest
        this.questNameText.text = "";
        this.questDescriptionText.text = "";
        this.questObjectivesText.text = "";
        this.questRewardText.text = "";

        //Deactivating our prompt game object
        this.promptUI.SetActive(false);
    }
}
