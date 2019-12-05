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

    //The list of give all items buttons for each player in the party
    public List<Button> giveAllItemsButtons;
    //The list of give one item buttons for each player in the party
    public List<Button> giveOneItemButtons;
    //The list of text objects for the item buttons
    public List<Text> playerButtonItemText;

    [Space(8)]

    //The list of give action buttons for each player in the party
    public List<Button> giveActionButtons;
    //The list of text for each of the player party buttons
    public List<Text> playerButtonActionText;

    [Space(8)]

    //The game object for displaying all of the buttons to give action rewards
    public GameObject actionRewardButtonPanel;
    //The game object for displaying all of the buttons to give item rewards
    public GameObject itemRewardButtonPanel;
    //The game object for the UI screen to give all characters an action
    public GameObject giveAllCharactersActionPanel;
    //The game object for the UI screen to give a random character an action
    public GameObject giveRandomCharacterActionPanel;

    //Int to hold the current amount of item rewards to give
    private int currentItemRewards = 0;


    
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

            //Setting the number of items to give away
            this.currentItemRewards = this.questToDisplay.itemRewards[this.questRewardIndex].amount;

            //Displaying the item buttons
            this.itemRewardButtonPanel.gameObject.SetActive(true);
            this.actionRewardButtonPanel.gameObject.SetActive(false);

            //Setting the reward text to the item
            this.rewardNameText.text = rewardItem.itemNameID + "  x" + this.questToDisplay.itemRewards[this.questRewardIndex].amount;
            this.rewardDescriptionText.text = "";
            this.actionDistanceText.text = "";
            this.actionTypeText.text = "";

            //Looping through all of the party characters to display our buttons
            for (int pc = 0; pc < this.giveOneItemButtons.Count; ++pc)
            {
                //If the current player index is null, we hide the player button
                if (pc >= CharacterManager.globalReference.selectedGroup.charactersInParty.Count || CharacterManager.globalReference.selectedGroup.charactersInParty[pc] == null)
                {
                    this.giveOneItemButtons[pc].gameObject.SetActive(false);
                    this.giveAllItemsButtons[pc].gameObject.SetActive(false);
                    this.playerButtonItemText[pc].text = "";
                }
                //If the current player party index is not null, we show the player button and change its name
                else
                {
                    //Creating a test object to see if the characters have enough room
                    GameObject testItem = GameObject.Instantiate(this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject) as GameObject;
                    testItem.GetComponent<Item>().currentStackSize += (uint)this.questToDisplay.itemRewards[this.questRewardIndex].amount;

                    this.giveOneItemButtons[pc].gameObject.SetActive(true);
                    this.giveAllItemsButtons[pc].gameObject.SetActive(true);

                    //If the character at this index has an empty inventory slot we can give it to them
                    if (CharacterManager.globalReference.selectedGroup.charactersInParty[pc].charInventory.CanItemBeAddedToInventory(testItem.GetComponent<Item>()))
                    {
                        this.giveOneItemButtons[pc].interactable = true;
                        this.giveAllItemsButtons[pc].interactable = true;
                        this.playerButtonItemText[pc].text = CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName;
                    }
                    //If the character doesn't has an empty inventory slot, we can't give them the item
                    else
                    {
                        this.giveOneItemButtons[pc].gameObject.SetActive(false);
                        this.giveAllItemsButtons[pc].gameObject.SetActive(false);
                        this.playerButtonItemText[pc].text = CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName + "'s Inventory is Full";
                    }

                    //Destroying the test object
                    Destroy(testItem);
                }
            }
        }
        //If the current reward is an action
        else if(this.questRewardIndex < (this.questToDisplay.itemRewards.Count + this.questToDisplay.actionRewards.Count))
        {
            //Getting the action reference
            Action rewardAct = this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardAction;

            //Displaying the item buttons
            this.itemRewardButtonPanel.gameObject.SetActive(false);
            this.actionRewardButtonPanel.gameObject.SetActive(true);

            //If the action is awarded to every character
            switch (this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardDistribution)
            {
                case DistributionType.Everyone:
                    //Setting the reward text to the action
                    this.rewardNameText.text = rewardAct.actionName;
                    this.rewardDescriptionText.text = rewardAct.actionDescription;
                    this.actionDistanceText.text = "Range: " + rewardAct.range;
                    this.actionTypeText.text = rewardAct.type.ToString();

                    //Showing the panel to give all characters the action
                    this.giveAllCharactersActionPanel.gameObject.SetActive(true);
                    break;

                case DistributionType.OneRandomCharacter:
                    //Setting the reward text to the action
                    this.rewardNameText.text = rewardAct.actionName;
                    this.rewardDescriptionText.text = rewardAct.actionDescription;
                    this.actionDistanceText.text = "Range: " + rewardAct.range;
                    this.actionTypeText.text = rewardAct.type.ToString();

                    //Showing the panel to give a random character the action
                    this.giveRandomCharacterActionPanel.gameObject.SetActive(true);
                    break;

                case DistributionType.PlayerChoice:
                    //Setting the reward text to the action
                    this.rewardNameText.text = rewardAct.actionName;
                    this.rewardDescriptionText.text = rewardAct.actionDescription;
                    this.actionDistanceText.text = "Range: " + rewardAct.range;
                    this.actionTypeText.text = rewardAct.type.ToString();

                    //Looping through all of the party characters to display our buttons
                    for (int pc = 0; pc < this.giveActionButtons.Count; ++pc)
                    {
                        //If the current player index is null, we hide the player button
                        if (pc >= CharacterManager.globalReference.selectedGroup.charactersInParty.Count || CharacterManager.globalReference.selectedGroup.charactersInParty[pc] == null)
                        {
                            this.giveActionButtons[pc].gameObject.SetActive(false);
                            this.playerButtonActionText[pc].text = "";
                        }
                        //If the current player party index is not null, we show the player button and change its name
                        else
                        {
                            this.giveActionButtons[pc].gameObject.SetActive(true);

                            //If the action that we're giving is a spell, we can give it to any character
                            if (rewardAct.GetType() == typeof(SpellAction))
                            {
                                this.giveActionButtons[pc].interactable = true;
                                this.playerButtonActionText[pc].text = "Give to " + CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName;
                            }
                            //if the action isn't a spell we need to check if the character already has the ability
                            else
                            {
                                //If the character already has this ability in their list of default actions, we can't give it to them
                                if (CharacterManager.globalReference.selectedGroup.charactersInParty[pc].charActionList.DoesDefaultListContainAction(rewardAct))
                                {
                                    this.giveActionButtons[pc].interactable = false;
                                    this.playerButtonActionText[pc].text = CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName + " Already Knows This";
                                }
                                //If the character doesn't have this action in their default actions list, we can give it to them
                                else
                                {
                                    this.giveActionButtons[pc].interactable = true;
                                    this.playerButtonActionText[pc].text = "Give to " + CharacterManager.globalReference.selectedGroup.charactersInParty[pc].firstName;
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
        //Reference to the character that we're giving the reward to
        Character charToGive = CharacterManager.globalReference.selectedGroup.charactersInParty[charIndex_];

        //If the reward is an item
        if (this.questRewardIndex < this.questToDisplay.itemRewards.Count)
        {
            //Item to test if the character has enough inventory space for the whole stack
            GameObject testItem = GameObject.Instantiate(this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject) as GameObject;

            //Looping through for each of the remaining items to give
            int stackSize = this.currentItemRewards;
            for (int a = 0; a < stackSize; ++a)
            {
                //Making sure this item can be added to the character's inventory
                if (charToGive.charInventory.CanItemBeAddedToInventory(testItem.GetComponent<Item>()))
                {
                    //Creating a new instance of the item
                    GameObject rewardObject = GameObject.Instantiate(this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject) as GameObject;
                    //Setting the item's prefab root
                    rewardObject.GetComponent<Item>().itemPrefabRoot = this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject;
                    //Adding the item to the designated character's inventory
                    charToGive.charInventory.AddItemToInventory(rewardObject.GetComponent<Item>());
                    //Removing 1 from our current item rewards
                    this.currentItemRewards -= 1;
                }
                //If there's not enough room for the item and we still have some left, we stop this loop
                else
                {
                    //Updating the text to say how many of the item are left
                    this.rewardNameText.text = this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.itemNameID + "  x" + this.currentItemRewards;
                    //Hiding the buttons that give this character items
                    this.giveOneItemButtons[charIndex_].gameObject.SetActive(false);
                    this.giveAllItemsButtons[charIndex_].gameObject.SetActive(false);
                    this.playerButtonItemText[charIndex_].text = CharacterManager.globalReference.selectedGroup.charactersInParty[charIndex_].firstName + "'s Inventory is Full";
                    //Destroying the test item once we're done
                    Destroy(testItem);
                    //Breaking out of this function so we don't go to the next reward yet
                    return;
                }
            }

            //Destroying the test item once we're done
            Destroy(testItem);
        }
        //If the reward is an action
        else
        {
            //Adding the ability to the designated character's default action list
            charToGive.charActionList.defaultActions.Add(this.questToDisplay.actionRewards[this.questRewardIndex - this.questToDisplay.itemRewards.Count].rewardAction);
        }

        //Increasing the reward index and moving on to the next reward
        this.questRewardIndex += 1;
        this.UpdateRewardDescription();
    }


    //Function called from one of our player party buttons to designate which character gets 1 of the reward items
    public void GiveOneRewardItemToCharAtIndex(int charIndex_)
    {
        //Creating a new instance of the item
        GameObject rewardObject = UnityEditor.PrefabUtility.InstantiatePrefab(this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject) as GameObject;
        //Setting the item's prefab root
        rewardObject.GetComponent<Item>().itemPrefabRoot = this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.gameObject;
        //Adding the item to the designated character's inventory
        Character charToGive = CharacterManager.globalReference.selectedGroup.charactersInParty[charIndex_];
        charToGive.charInventory.AddItemToInventory(rewardObject.GetComponent<Item>());

        //Reducing the current number of remaining item rewards
        this.currentItemRewards -= 1;

        //Updating the text to say how many of the item are left
        this.rewardNameText.text = this.questToDisplay.itemRewards[this.questRewardIndex].rewardItem.itemNameID + "  x" + this.currentItemRewards;

        //If we're out of items for this reward, we move to the next reward
        if(this.currentItemRewards < 1)
        {
            this.questRewardIndex += 1;
            this.UpdateRewardDescription();
            return;
        }
        //If this character's inventory is now full
        if(!charToGive.charInventory.CanItemBeAddedToInventory(rewardObject.GetComponent<Item>()))
        {
            //Hiding the buttons that give this character items
            this.giveOneItemButtons[charIndex_].gameObject.SetActive(false);
            this.giveAllItemsButtons[charIndex_].gameObject.SetActive(false);
            this.playerButtonItemText[charIndex_].text = CharacterManager.globalReference.selectedGroup.charactersInParty[charIndex_].firstName + "'s Inventory is Full";
        }
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
