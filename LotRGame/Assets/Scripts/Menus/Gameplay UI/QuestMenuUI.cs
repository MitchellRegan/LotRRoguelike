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

    //Index for the panel that's being displayed
    private int displayedQuestPanelIndex = -1;

    [Space(8)]

    //The reference to the scroll view's content rect transform that is scaled 
    public RectTransform descriptionContentTransform;
    //The text reference for the displayed quest's name
    public Text questNameText;
    //The text reference for the displayed quest's description
    public Text questDescriptionText;
    //The text reference for the displayed quest's objectives
    public Text questObjectivesText;
    //The gap in space between the text for quest descriptions
    public float textDescriptionGap = 40;

    [Space(8)]

    //The button that allows the player to turn in quests
    public Button turnInQuestButton;
    //The button that allows the player to abandon quests
    public Button abandonQuestButton;



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
        this.UpdatePanels(true);
    }


    //Function called externally whenever time advances to update the description
    public void UpdateDescriptionOnTimeAdvance()
    {
        //If our current index is invalid or the description content is hidden, nothing happens
        if(this.displayedQuestPanelIndex < 0 || this.displayedQuestPanelIndex >= this.questPanels.Count || !this.descriptionContentTransform.gameObject.activeInHierarchy)
        {
            return;
        }

        //We update the quest description to show the change in time
        this.DisplayQuestDescription(this.displayedQuestPanelIndex);
    }


    //Function called from OnEnable and AbandonQuest AtIndex to refresh our QuestUIPanels
    private void UpdatePanels(bool hideDescription_ = false)
    {
        //If we hide our description
        if (hideDescription_)
        {
            //Resetting our selected quest index
            this.displayedQuestPanelIndex = -1;
            //Hiding our quest description
            this.HideQuestDescription();
        }

        //Looping through all of our QuestUIPanels and deleting their objects unless they're the original
        for(int p = 1; p < this.questPanels.Count; ++p)
        {
            //Deleting the object for the current panel
            Destroy(this.questPanels[p].gameObject);
            this.questPanels[p] = null;
        }
        //Resetting the list of paens so only the original is left
        this.questPanels = new List<QuestUIPanel>() { this.originalPanel };
        this.originalPanel.gameObject.SetActive(false);

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

            //Setting the text for the panel
            newQuestPanel.questNameText.text = q.questName;

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
    

    //Function called externally to confirm a quest abandon
    public void AbandonSelectedQuest()
    {
        //Telling the QuestTracker to abandon the quest
        QuestTracker.globalReference.AbandonQuestAtIndex(this.displayedQuestPanelIndex);
        //Resetting the abandon quest index
        this.displayedQuestPanelIndex = -1;
        //Updating our panels so that we don't display the abandoned quest anymore
        this.UpdatePanels(true);
    }


    //Function called externally to turn in the selected quest
    public void TurnInSelectedQuest()
    {
        //Telling the QuestTracker to turn in the quest
        QuestTracker.globalReference.CompleteQuestAtIndex(this.displayedQuestPanelIndex);
        //Resetting the quest index
        this.displayedQuestPanelIndex = -1;
        //Updating our panels so that we don't display the completed quest anymore
        this.UpdatePanels(true);
    }


    //Function called from UpdatePanels to clear the quest description
    private void HideQuestDescription()
    {
        //Hiding the abandon quest and turn in quest buttons
        this.abandonQuestButton.gameObject.SetActive(false);
        this.turnInQuestButton.gameObject.SetActive(false);

        //Hiding the quest text
        this.questNameText.text = "";
        this.questDescriptionText.text = "";
        this.questObjectivesText.text = "";
        this.descriptionContentTransform.gameObject.SetActive(false);
    }


    //Function called externally from QuestUIPanel.cs to display a quest's description
    public void DisplayQuestDescription(int questIndex_)
    {
        //Making sure the selected index is valid
        if (questIndex_ < 0 || questIndex_ >= this.questPanels.Count)
        {
            return;
        }

        //Setting our selected quest index
        this.displayedQuestPanelIndex = questIndex_;

        //Displaying the description scroll view
        this.descriptionContentTransform.gameObject.SetActive(true);

        //Getting the quest reference
        Quest displayedQuest = QuestTracker.globalReference.questLog[questIndex_];

        //Setting the displayed quest's text name
        this.questNameText.text = displayedQuest.questName;
        //Setting the displayed quest's description text
        this.questDescriptionText.text = displayedQuest.questDescription;
        //Setting the displayed quest's objective text
        this.questObjectivesText.text = this.GetQuestObjectiveText(displayedQuest);

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

        //Resizing the discription content view size
        float contentSize = 0;
        contentSize += this.questNameText.GetComponent<RectTransform>().rect.height;
        contentSize += this.questDescriptionText.GetComponent<RectTransform>().rect.height;
        contentSize += this.textDescriptionGap;
        contentSize += this.questObjectivesText.GetComponent<RectTransform>().rect.height;
        this.descriptionContentTransform.sizeDelta = new Vector2(0, contentSize);
        

        //If the quest is abandonable
        if(displayedQuest.canBeAbandoned)
        {
            //We allow the abandon quest button to be clicked
            this.abandonQuestButton.gameObject.SetActive(true);
        }
        //If the quest is NOT able to be abandoned
        else
        {
            //We disable the abandon quest button
            this.abandonQuestButton.gameObject.SetActive(false);
        }


        //If the displayed quest is failed, it can't be turned in
        if(displayedQuest.isQuestFailed)
        {
            this.turnInQuestButton.gameObject.SetActive(false);
            return;
        }
        //If the quest has a required turn in location
        if(displayedQuest.turnInQuestLoaction != null)
        {
            //Getting the tile that the player party is on
            TileInfo partyTile = CharacterManager.globalReference.selectedGroup.GetComponent<WASDOverworldMovement>().currentTile;
            //If the party tile doesn't have a decoration, it can't be a map location so nothing happens
            if (partyTile.decorationModel == null)
            {
                this.turnInQuestButton.gameObject.SetActive(false);
                return;
            }
            //If the tile has a decoration but it's not a map location, nothing happens
            else if (!partyTile.decorationModel.GetComponent<MapLocation>())
            {
                this.turnInQuestButton.gameObject.SetActive(false);
                return;
            }
            //If the tile has a map location but it isn't the turn in location, nothing happens
            else if (partyTile.decorationModel.GetComponent<MapLocation>().locationName != displayedQuest.turnInQuestLoaction.locationName)
            {
                this.turnInQuestButton.gameObject.SetActive(false);
                return;
            }
        }
        //If the quest has a timer
        if(displayedQuest.questTimeHours > 0)
        {
            //If the quest doesn't fail when the timer is reached and the player hasn't lasted as long as is required
            if(!displayedQuest.failOnTimeReached && displayedQuest.currentHours != displayedQuest.questTimeHours)
            {
                this.turnInQuestButton.gameObject.SetActive(false);
                return;
            }
        }
        //If the quest has target travel destinations
        if(displayedQuest.destinationList.Count > 0)
        {
            //If any of the destinations haven't been visited, the quest can't be turned in
            foreach(QuestTravelDestination loc in displayedQuest.destinationList)
            {
                if(!loc.locationVisited)
                {
                    this.turnInQuestButton.gameObject.SetActive(false);
                    return;
                }
            }
        }
        //If the quest has a kill list
        if(displayedQuest.killList.Count > 0)
        {
            //If any of the kill lists haven't been completed, the quest can't be turned in
            foreach(QuestKillRequirement kill in displayedQuest.killList)
            {
                if(kill.currentKills < kill.killsRequired)
                {
                    this.turnInQuestButton.gameObject.SetActive(false);
                    return;
                }
            }
        }
        //If the quest has a fetch list
        if(displayedQuest.fetchList.Count > 0)
        {
            //If any of the item lists haven't been completed, the quest can't be turned in
            foreach(QuestFetchItems collectable in displayedQuest.fetchList)
            {
                if(collectable.currentItems < collectable.itemsRequired)
                {
                    this.turnInQuestButton.gameObject.SetActive(false);
                    return;
                }
            }
        }
        //If the quest has an escort list
        if(displayedQuest.escortList.Count > 0)
        {
            //If any of the escort characters are dead, the quest can't be turned in
            foreach(QuestEscortCharacter escort in displayedQuest.escortList)
            {
                if(escort.isCharacterDead)
                {
                    this.turnInQuestButton.gameObject.SetActive(false);
                    return;
                }
            }
        }

        //If we make it this far, the quest can be turned in
        this.turnInQuestButton.gameObject.SetActive(true);
    }


    //Function called from DisplayQuestDescription to get the text for the quest objectives
    private string GetQuestObjectiveText(Quest quest_)
    {
        //The string that we return
        string objectiveText = "\n";


        //If the quest is failed, we display it
        if(quest_.isQuestFailed)
        {
            objectiveText += "    QUEST FAILED! \n \n";
        }

        //Setting the quest timer if the timer is required
        if(quest_.questTimeHours > 0)
        {
            //If the timer is counting down to failure
            if(quest_.failOnTimeReached)
            {
                objectiveText += "Time remaining  - " + (quest_.questTimeHours - quest_.currentHours);
                if(quest_.questTimeHours - quest_.currentHours == 1)
                {
                    objectiveText += " hour";
                }
                else
                {
                    objectiveText += " hours";
                }
                if(quest_.currentHours == quest_.questTimeHours)
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
}
