using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    //The reference to the Inventory UI screen for selected party members
    public GameObject partyInventoryUIObject;
    //The reference to the Inventory UI screen for bag/chest objects
    public GameObject bagInventoryUIObject;
    //The reference to the Inventory UI screen for trade characters
    public GameObject tradeInventoryUIObject;


	
    //Function called from the Character List Panel buttons. Sets the party inventory to the character at the given index
    public void OpenPartyInventoryAtIndex(int partyCharacterIndex_)
    {
        //Makes sure the character index is within the bounds of the party size and that it isn't for an empty slot
        if(CharacterManager.globalReference.maxPartySize <= partyCharacterIndex_ && partyCharacterIndex_ < 0)
        {
            return;
        }
        else if(CharacterManager.globalReference.playerParty[partyCharacterIndex_] == null)
        {
            return;
        }

        //If the given character is already showing in the player inventory UI screen
        if (this.partyInventoryUIObject.activeSelf && CharacterInventoryUI.partyInventory.selectedCharacterInventory.gameObject == CharacterManager.globalReference.playerParty[partyCharacterIndex_].gameObject)
        {
            //Disables the party inventory UI screen
            this.partyInventoryUIObject.SetActive(false);
        }
        //If the given character is not already showing on the player inventory UI screen
        else
        {
            //Sets the global reference to the player party inventory to the selected character's inventory component
            CharacterInventoryUI.partyInventory.selectedCharacterInventory = CharacterManager.globalReference.playerParty[partyCharacterIndex_].GetComponent<Inventory>();

            //Makes sure the party Inventory UI screen is opened
            this.partyInventoryUIObject.SetActive(true);
        }
    }


    //Function called externally to display a given object's inventory. Determines which inventory screen to open
    public void OpenObjectsInventory(GameObject objectInventoryToShow_)
    {
        //If the given object doesn't have an inventory component, nothing happens
        if (!objectInventoryToShow_.GetComponent<Inventory>())
        {
            return;
        }

        //If the game object is a character
        if (objectInventoryToShow_.GetComponent<Character>())
        {
            //If this character is in the player party
            if (CharacterManager.globalReference.playerParty.Contains(objectInventoryToShow_.GetComponent<Character>()))
            {
                //If the party inventory is already showing
                if(this.partyInventoryUIObject.activeSelf)
                {
                    //If the character that's being shown isn't this character, this character is a trade character
                    if(CharacterInventoryUI.partyInventory.selectedCharacterInventory.gameObject != objectInventoryToShow_)
                    {
                        //Sets the global reference to the trade inventory to the given character's inventory component
                        CharacterInventoryUI.tradeInventory.selectedCharacterInventory = objectInventoryToShow_.GetComponent<Inventory>();
                        //Makes sure the trade inventory UI screen is opened
                        this.tradeInventoryUIObject.SetActive(true);
                    }
                    //If the character that's being shown IS this character, nothing happens
                }
                //If the party inventory is not active, this character becomes the party inventory character
                else
                {
                    //Sets the global reference to the player party inventory to the selected character's inventory component
                    CharacterInventoryUI.partyInventory.selectedCharacterInventory = objectInventoryToShow_.GetComponent<Inventory>();

                    //Makes sure the party Inventory UI screen is opened
                    this.partyInventoryUIObject.SetActive(true);
                }
            }
            //If this character is not in the party, they're a trade character
            else
            {
                //Sets the global reference to the trade inventory to the given character's inventory component
                CharacterInventoryUI.tradeInventory.selectedCharacterInventory = objectInventoryToShow_.GetComponent<Inventory>();
                //Makes sure the trade inventory UI screen is opened
                this.tradeInventoryUIObject.SetActive(true);
            }
        }
        //If this game object is an inventory without a character, it's a bag/chest
        else
        {
            //Sets the global reference to the bag inventory to the given object's inventory component
            CharacterInventoryUI.bagInventory.selectedCharacterInventory = objectInventoryToShow_.GetComponent<Inventory>();
            //Makes sure the bag inventory UI screen is opened
            this.bagInventoryUIObject.SetActive(true);
        }
    }


    //Function called externally to display the selected character's inventory. If no character is selected, opens the first party character's inventory instead
    public void OpenSelectedCharacterInventory()
    {
        //If the party inventory UI screen is already showing, it's disabled
        if(this.partyInventoryUIObject.activeSelf)
        {
            //Disables the party inventory UI screen and ends this function
            this.partyInventoryUIObject.SetActive(false);
            return;
        }

        //If the selected characters list has at least one character in it
        if(CharacterManager.globalReference.selectedCharacters.Count > 0)
        {
            //Sets the party inventory to show the first character in the selection list's inventory
            CharacterInventoryUI.partyInventory.selectedCharacterInventory = CharacterManager.globalReference.selectedCharacters[0].GetComponent<Inventory>();
            //Shows the party inventory UI screen
            this.partyInventoryUIObject.SetActive(true);
        }
        //If no character is currently selected
        else
        {
            //Sets the party inventory to show the first character in the player party list's inventory
            CharacterInventoryUI.partyInventory.selectedCharacterInventory = CharacterManager.globalReference.playerParty[0].GetComponent<Inventory>();
            //Shows the party inventory UI screen
            this.partyInventoryUIObject.SetActive(true);
        }
    }
}
