using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewardUI : MonoBehaviour
{
    //The Quest class that we're displaying rewards for
    private Quest questToDisplay;
    //The index of the reward that we're displaying
    private int questRewardIndex = 0;

    //The text for the reward name
    public Text rewardNameText;
    //The text for the reward description
    public Text rewardDescriptionText;
    //The text for the reward action distance
    public Text actionDistanceText;
    //The text for the reward action type
    public Text actionTypeText;

    [Space(8)]

    //The list of buttons for each player in the party
    public List<Button> playerPartyButtons;
    //The list of text for each of the player party buttons
    public List<Text> playerButtonText;
    //The game object for the UI screen to give all characters an action
    public GameObject giveAllCharactersActionPanel;
    //The game object for the UI screen to give a random character an action
    public GameObject giveRandomCharacterActionPanel;


    
	//Function called externally to display a quest reward
    public void DisplayQuestRewards(Quest questToDisplay_)
    {
        //Setting the quest that we need to display
        this.questToDisplay = questToDisplay_;
        this.questRewardIndex = 0;

        //If this quest has no rewards, nothing happens
        if(questToDisplay_.itemRewards.Count + questToDisplay_.actionRewards.Count == 0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        //Displaying the reward description
        this.UpdateRewardDescription();
    }


    //Function called from GiveRewardToCharacterAtIndex, AbandonCurrentReward, and DisplayQuestRewards to update the reward description
    private void UpdateRewardDescription()
    {
        //If the current reward is an item
        if(this.questRewardIndex < this.questToDisplay.itemRewards.Count)
        {
            //Getting the item reference
            Item rewardItem = this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem;

            //Setting the reward text to the item
            this.rewardNameText.text = rewardItem.itemNameID + "  x" + this.questToDisplay.itemRewards[this.questRewardIndex].amount;
            this.rewardDescriptionText.text = "";
            this.actionDistanceText.text = "";
            this.actionTypeText.text = "";

            //Looping through all of the party characters to display our buttons
            for (int pc = 0; pc < this.playerPartyButtons.Count; ++pc)
            {
                //If the current player index is null, we hide the player button
                if (pc >= CharacterManager.globalReference.selectedGroup.charactersInParty.Count || CharacterManager.globalReference.selectedGroup.charactersInParty[pc] == null)
                {
                    this.playerPartyButtons[pc].gameObject.SetActive(false);
                    this.playerButtonText[pc].text = "";
                }
                //If the current player party index is not null, we show the player button and change its name
                else
                {
                    //Creating a test object to see if the characters have enough room
                    GameObject testItem = GameObject.Instantiate(this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject) as GameObject;
                    testItem.GetComponent<Item>().currentStackSize += (uint)this.questToDisplay.itemRewards[this.questRewardIndex].amount;

                    this.playerPartyButtons[pc].gameObject.SetActive(true);
                    //If the character at this index has an empty inventory slot we can give it to them
                    if (CharacterManager.globalReference.selectedGroup.charactersInParty[pc].charInventory.CanItemBeAddedToInventory(testItem.GetComponent<Item>()))
                    {
                        this.playerPartyButtons[pc].interactable = true;
                        this.playerButtonText[pc].text = "Give to " + CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName;
                    }
                    //If the character doesn't has an empty inventory slot, we can't give them the item
                    else
                    {
                        Debug.Log("Inventory Remaining: " + CharacterManager.globalReference.selectedGroup.charactersInParty[pc].charInventory.CheckForEmptySlot());
                        this.playerPartyButtons[pc].interactable = false;
                        this.playerButtonText[pc].text = CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName + "'s Inventory is Full";
                    }
                }
            }
        }
        //If the current reward is an action
        else if(this.questRewardIndex < (this.questToDisplay.itemRewards.Count + this.questToDisplay.actionRewards.Count))
        {
            //Getting the action reference
            Action rewardAct = this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardAction;

            //If the action is awarded to every character
            switch(this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardDistribution)
            {
                case QuestActionReward.DistributionType.Everyone:
                    //Setting the reward text to the action
                    this.rewardNameText.text = rewardAct.actionName;
                    this.rewardDescriptionText.text = rewardAct.actionDescription;
                    this.actionDistanceText.text = "Range: " + rewardAct.range;
                    this.actionTypeText.text = rewardAct.type.ToString();

                    //Showing the panel to give all characters the action
                    this.giveAllCharactersActionPanel.gameObject.SetActive(true);
                    break;

                case QuestActionReward.DistributionType.OneRandomCharacter:
                    //Setting the reward text to the action
                    this.rewardNameText.text = rewardAct.actionName;
                    this.rewardDescriptionText.text = rewardAct.actionDescription;
                    this.actionDistanceText.text = "Range: " + rewardAct.range;
                    this.actionTypeText.text = rewardAct.type.ToString();

                    //Showing the panel to give a random character the action
                    this.giveRandomCharacterActionPanel.gameObject.SetActive(true);
                    break;

                case QuestActionReward.DistributionType.PlayerChoice:
                    //Setting the reward text to the action
                    this.rewardNameText.text = rewardAct.actionName;
                    this.rewardDescriptionText.text = rewardAct.actionDescription;
                    this.actionDistanceText.text = "Range: " + rewardAct.range;
                    this.actionTypeText.text = rewardAct.type.ToString();

                    //Looping through all of the party characters to display our buttons
                    for (int pc = 0; pc < this.playerPartyButtons.Count; ++pc)
                    {
                        //If the current player index is null, we hide the player button
                        if (pc >= CharacterManager.globalReference.selectedGroup.charactersInParty.Count || CharacterManager.globalReference.selectedGroup.charactersInParty[pc] == null)
                        {
                            this.playerPartyButtons[pc].gameObject.SetActive(false);
                            this.playerButtonText[pc].text = "";
                        }
                        //If the current player party index is not null, we show the player button and change its name
                        else
                        {
                            this.playerPartyButtons[pc].gameObject.SetActive(true);

                            //If the action that we're giving is a spell, we can give it to any character
                            if (rewardAct.GetType() == typeof(SpellAction))
                            {
                                this.playerPartyButtons[pc].interactable = true;
                                this.playerButtonText[pc].text = "Give to " + CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName;
                            }
                            //if the action isn't a spell we need to check if the character already has the ability
                            else
                            {
                                //If the character already has this ability in their list of default actions, we can't give it to them
                                if (CharacterManager.globalReference.selectedGroup.charactersInParty[pc].charActionList.DoesDefaultListContainAction(rewardAct))
                                {
                                    this.playerPartyButtons[pc].interactable = false;
                                    this.playerButtonText[pc].text = CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName + " Already Knows This";
                                }
                                //If the character doesn't have this action in their default actions list, we can give it to them
                                else
                                {
                                    this.playerPartyButtons[pc].interactable = true;
                                    this.playerButtonText[pc].text = "Give to " + CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName;
                                }
                            }
                        }
                    }
                    break;
            }

            
        }
        //If the current index is beyond all of the reward indexes, we're done with this component
        else
        {
            //We disable this game object until the next time it's needed
            this.gameObject.SetActive(false);
        }
    }


    //Function called from one of our player party buttons to designate which character gets the reward
    public void GiveRewardToCharacterAtIndex(int charIndex_)
    {
        //If the reward is an item
        if (this.questRewardIndex < this.questToDisplay.itemRewards.Count)
        {
            //Looping through for each amount of the item to give
            for (int a = 0; a < this.questToDisplay.itemRewards[this.questRewardIndex].amount; ++a)
            {
                //Creating a new instance of the item
                GameObject rewardObject = GameObject.Instantiate(this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject) as GameObject;
                //Adding the item to the designated character's inventory
                Character charToGive = CharacterManager.globalReference.selectedGroup.charactersInParty[charIndex_];
                charToGive.charInventory.AddItemToInventory(rewardObject.GetComponent<Item>());
            }
        }
        //if the reward is an action
        else
        {
            //Adding the ability to the designated character's default action list
            Character charToGive = CharacterManager.globalReference.selectedGroup.charactersInParty[charIndex_];
            charToGive.charActionList.defaultActions.Add(this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardAction);
        }

        //Increasing the reward index and moving on to the next reward
        this.questRewardIndex += 1;
        this.UpdateRewardDescription();
    }


    //Function called externally to give all of the party characters the reward ability
    public void GiveAbilityToAllPartyCharacters()
    {
        //The action reward for all characters
        Action rewardAct = this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardAction;

        //Looping through all of the characters in the player party
        foreach (Character pc in CharacterManager.globalReference.selectedGroup.charactersInParty)
        {
            //If the action is a spell OR a non-spell that they don't know, we add it to the character's default action list by default
            if (rewardAct.GetType() == typeof(SpellAction) || !pc.charActionList.DoesDefaultListContainAction(rewardAct))
            {
                pc.charActionList.defaultActions.Add(rewardAct);
            }
        }

        //Moving on to the next reward
        this.questRewardIndex += 1;
        this.UpdateRewardDescription();

        //Hiding the panel
        this.giveAllCharactersActionPanel.gameObject.SetActive(false);
    }


    //Function called externally to give a random party character the reward ability
    public void GiveAbilityToRandomCharacter()
    {
        //The action reward for all characters
        Action rewardAct = this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardAction;

        //Making a list of all character slots in the party
        List<int> partyCharIndexes = new List<int>();
        for (int i = 0; i < CharacterManager.globalReference.selectedGroup.charactersInParty.Count; ++i)
        {
            partyCharIndexes.Add(i);
        }

        //Looping until we're out of valid characters to give the action to
        while (partyCharIndexes.Count > 0)
        {
            //Getting a random character index from the party group
            int randIndex = Random.Range(0, partyCharIndexes.Count);
            //Making sure the index isn't empty first
            if (CharacterManager.globalReference.selectedGroup.charactersInParty[randIndex] != null)
            {
                //If the action is a spell OR a non-spell that they don't know, we add it to the character's default action list
                if (rewardAct.GetType() == typeof(SpellAction) ||
                    !CharacterManager.globalReference.selectedGroup.charactersInParty[randIndex].charActionList.DoesDefaultListContainAction(rewardAct))
                {
                    CharacterManager.globalReference.selectedGroup.charactersInParty[randIndex].charActionList.defaultActions.Add(rewardAct);
                    break;
                }
            }
        }

        //Moving on to the next reward
        this.questRewardIndex += 1;
        this.UpdateRewardDescription();

        //Hiding the panel
        this.giveRandomCharacterActionPanel.gameObject.SetActive(false);
    }


    //Function called externally to discard the current quest reward
    public void DiscardCurrentReward()
    {
        //We just move on without giving the reward to anyone
        this.questRewardIndex += 1;
        this.UpdateRewardDescription();
    }
}
