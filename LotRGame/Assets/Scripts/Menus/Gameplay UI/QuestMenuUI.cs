using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestMenuUI : MonoBehaviour
{
    //The static reference for this QuestMenuUI script
    public static QuestMenuUI globalReference;

    //The reference to the default QuestUIPanel.cs object that we duplicate
    public QuestUIPanel originalPanel;

    //The list of QuestUIPanel objects for each quest in our quest log
    private List<QuestUIPanel> questPanels;

    //The reference to the scroll view's content rect transform that is scaled
    public RectTransform scrollContentTransform;

    //The buffer of units from the top and bottom of the scroll view
    public float topBottomScrollHeightBuffer = 7;
    //The buffer of units between panels
    public float bufferBetweenPanels = 2;

    //Index for the panel that's marked to be abandoned
    private int abandonQuestPanelIndex = -1;

    //UnityEvent called to open the confirmation screen to abandon a quest
    public UnityEvent promptAbandonQuestEvent;

    [Space(8)]

    //The reference to the scroll view's content rect transform that is scaled 
    public RectTransform descriptionContentTransform;
    //The text reference for the displayed quest's name
    public Text questNameText;
    //The text reference for the displayed quest's description
    public Text questDescriptionText;
    //The text reference for the displayed quest's objectives
    public Text questObjectivesText;



    //Function called when this object is created
    private void Awake()
    {
        //Setting the global reference
        if(QuestMenuUI.globalReference == null)
        {
            QuestMenuUI.globalReference = this;
        }
        else
        {
            Destroy(this);
        }

        //Initializing our list of QuestUIPanels
        this.questPanels = new List<QuestUIPanel>();
        //Adding our original panel to our list so it's always index 0
        this.questPanels.Add(this.originalPanel);
    }


    //Function called when this object is enabled
    private void OnEnable()
    {
        this.UpdatePanels();
    }


    //Function called from OnEnable and AbandonQuest AtIndex to refresh our QuestUIPanels
    private void UpdatePanels()
    {
        //Looping through all of our QuestUIPanels and deleting their objects unless they're the original
        for(int p = 1; p < this.questPanels.Count; ++p)
        {
            //Deleting the object for the current panel
            Destroy(this.questPanels[p].gameObject);
            this.questPanels[p] = null;
        }
        //Resetting the list of paens so only the original is left
        this.questPanels = new List<QuestUIPanel>() { this.originalPanel };

        //Int to hold the index for the quest we're adding to our quest panels
        int questIndex = -1;
        //Finding all of the quests in our quest log
        foreach(Quest q in QuestTracker.globalReference.questLog)
        {
            questIndex += 1;

            //Variable to hold the current QuestUIPanel for this quest
            QuestUIPanel newQuestPanel;

            //If the index is 0, it's the original panel
            if(questIndex == 0)
            {
                //Making the original panel visible
                this.questPanels[0].gameObject.SetActive(true);
                newQuestPanel = this.questPanels[0];
            }
            //If the index is greater than 0 we make a new panel
            else
            {
                //Creating a new instance of the original panel
                GameObject newPanelObj = GameObject.Instantiate(this.originalPanel.gameObject);
                //Setting the panel's parent transform to the same as the original's
                newPanelObj.transform.SetParent(this.originalPanel.transform.parent);
                newQuestPanel = newPanelObj.GetComponent<QuestUIPanel>();
                //Adding the panel to our list of quest panels
                this.questPanels.Add(newQuestPanel);
            }

            //Setting the index for the panel
            newQuestPanel.panelIndex = questIndex;
        }
    }


    //Function called from UpdatePanels to resize the scroll view content size
    private void ResizeScrollView()
    {
        //Float to hold the total height that the scroll view should be
        float scrollViewHeight = 0;

        //Adding the top/bottom buffer twice (once for the top and once for the bottom)
        scrollViewHeight += (2 * this.topBottomScrollHeightBuffer);

        //Adding the height of the panel multiplied by the number of panels
        float panelHeight = this.originalPanel.GetComponent<RectTransform>().rect.height;
        scrollViewHeight += (this.questPanels.Count * panelHeight);

        //Adding the buffer between panels multiplied by the number of panels - 1
        scrollViewHeight += ((this.questPanels.Count - 1) * this.bufferBetweenPanels);

        //Setting the scroll view height
        this.scrollContentTransform.sizeDelta = new Vector2(0, scrollViewHeight);
    }


    //Function called from QuestPanelUI.cs to prompt abandoning a quest
    public void PromptQuestAbandon(int panelIndex_)
    {
        //Making sure the selected index is valid
        if (panelIndex_ < 0 || panelIndex_ >= this.questPanels.Count)
        {
            return;
        }

        //Checking to make sure the quest can be abandoned
        if(!QuestTracker.globalReference.questLog[panelIndex_].canBeAbandoned)
        {
            return;
        }

        //Setting the index for the quest we'll abandon
        this.abandonQuestPanelIndex = panelIndex_;
        //Invoking the unity event to promp the player confirmation screen
        this.promptAbandonQuestEvent.Invoke();
    }


    //Function called externally to confirm a quest abandon
    public void AbandonSelectedQuest()
    {
        //Telling the QuestTracker to abandon the quest
        QuestTracker.globalReference.AbandonQuestAtIndex(this.abandonQuestPanelIndex);
        //Resetting the abandon quest index
        this.abandonQuestPanelIndex = -1;
        //Updating our panels so that we don't display the abandoned quest anymore
        this.UpdatePanels();
    }


    //Function called externally from QuestUIPanel.cs to display a quest's description
    public void DisplayQuestDescription(int questIndex_)
    {
        //Making sure the selected index is valid
        if (questIndex_ < 0 || questIndex_ >= this.questPanels.Count)
        {
            return;
        }
        
        //Setting the displayed quest's text name
        this.questNameText.text = QuestTracker.globalReference.questLog[questIndex_].questName;

        //Setting the displayed quest's description text
        this.questDescriptionText.text = QuestTracker.globalReference.questLog[questIndex_].questDescription;
        //Setting the description text's rect transform position just below the quest name text
        float descriptionPos = this.questNameText.GetComponent<RectTransform>().localPosition.y;
        descriptionPos += this.questNameText.GetComponent<RectTransform>().rect.height;
        this.questDescriptionText.GetComponent<RectTransform>().localPosition = new Vector3(this.questDescriptionText.GetComponent<RectTransform>().localPosition.x,
                                                                                            descriptionPos,
                                                                                            0);

        //Setting the displayed quest's objective text
        this.questObjectivesText.text = this.GetQuestObjectiveText(QuestTracker.globalReference.questLog[questIndex_]);
        //Setting the objective text's rect transform position just below the quest description text
        descriptionPos += this.questDescriptionText.GetComponent<RectTransform>().rect.height;
        this.questObjectivesText.GetComponent<RectTransform>().localPosition = new Vector3(this.questObjectivesText.GetComponent<RectTransform>().localPosition.x,
                                                                                            descriptionPos,
                                                                                            0);
    }


    //Function called from DisplayQuestDescription to get the text for the quest objectives
    private string GetQuestObjectiveText(Quest quest_)
    {
        //The string that we return
        string objectiveText = "/n";


        //If the quest is failed, we display it
        if(quest_.isQuestFailed)
        {
            objectiveText += "    QUEST FAILED! /n";
        }

        //Looping through each of the quest location destinations
        foreach(QuestTravelDestination loc in quest_.destinationList)
        {
            //Telling the player to visit the location name
            objectiveText += "Travel to " + loc.requiredLocation.locationName + "/n";
        }
        
        //Looping through each of the kill quests
        foreach(QuestKillRequirement kill in quest_.killList)
        {
            //Telling the player to kill a number of enemies
            objectiveText += "Kill " + kill.killableEnemy.firstName + " ";
            if(kill.killableEnemy.lastName != "")
            {
                objectiveText += kill.killableEnemy.lastName + " ";
            }
            objectiveText += " - " + kill.currentKills + "/" + kill.killsRequired + "/n";
        }

        //Looping through each of the fetch quests
        foreach(QuestFetchItems fetch in quest_.fetchList)
        {
            //Telling the player to collect a number of items
            objectiveText += "Collect " + fetch.collectableItem.itemNameID + "  - " + fetch.currentItems + "/" + fetch.itemsRequired + "/n";
        }

        //Looping through each of the escort quests
        foreach(QuestEscortCharacter escort in quest_.escortList)
        {
            //Telling the player to protect a character
            objectiveText += "Protect " + escort.characterToEscort.firstName;
            if(escort.characterToEscort.lastName != "")
            {
                objectiveText += " " + escort.characterToEscort.lastName;
            }
        }

        //Returning the completed objective list text
        return objectiveText;
    }
}
