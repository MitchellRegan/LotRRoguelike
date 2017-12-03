using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpener : MonoBehaviour
{
    //Static reference to this component
    public static InventoryOpener globalReference;

    //The reference to the Inventory UI screen for selected party members
    public GameObject partyInventoryUIObject;
    //The reference to the Inventory UI screen for bag/chest objects
    public GameObject bagInventoryUIObject;
    //The reference to the Inventory UI screen for trade characters
    public GameObject tradeInventoryUIObject;

    [Space(8)]

    //The Image that displays the icon for items that are being moved
    public Image dragIconImage;



    //Function called when this game object is created
    private void Awake()
    {
        //If there isn't already a global reference, this object becomes it
        if(InventoryOpener.globalReference == null)
        {
            InventoryOpener.globalReference = this;
        }
        //If a global reference already exists, this object is destroyed
        else
        {
            Destroy(this);
        }

        //Setting the static references for the CharacterInventoryUI.cs static references
        CharacterInventoryUI.partyInventory = this.partyInventoryUIObject.GetComponent<CharacterInventoryUI>();
        CharacterInventoryUI.bagInventory = this.bagInventoryUIObject.GetComponent<CharacterInventoryUI>();
        CharacterInventoryUI.tradeInventory = this.tradeInventoryUIObject.GetComponent<CharacterInventoryUI>();
    }
	

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

        //If the player inventory UI screen is already showing
        if (this.partyInventoryUIObject.activeSelf)
        {
            //If the displayed character in the party inventory screen is this character, all of the inventory UIs are disabled
            if(CharacterInventoryUI.partyInventory.selectedCharacterInventory.gameObject == CharacterManager.globalReference.playerParty[partyCharacterIndex_].gameObject)
            {
                this.partyInventoryUIObject.SetActive(false);
                this.tradeInventoryUIObject.SetActive(false);
                this.bagInventoryUIObject.SetActive(false);
            }
            //If the displayed character ISN'T this character, this character is displayed in the trade character inventory screen
            else
            {
                //If the trade inventory screen isn't showing
                if (!this.tradeInventoryUIObject.activeSelf)
                {
                    //If the bag/chest inventory is showing, it needs to be disabled first
                    if (this.bagInventoryUIObject.activeSelf)
                    {
                        this.bagInventoryUIObject.SetActive(false);
                    }

                    //Sets the global reference to the trade character inventory to the selected character's inventory component
                    this.tradeInventoryUIObject.GetComponent<CharacterInventoryUI>().selectedCharacterInventory = CharacterManager.globalReference.playerParty[partyCharacterIndex_].charInventory;
                    //Makes sure the trade Inventory UI screen is opened
                    this.tradeInventoryUIObject.SetActive(true);
                }
                //If the trade inventory screen is showing
                else
                {
                    //If the displayed character in the trade inventory screen is this character, the trade inventory UI is disabled
                    if(CharacterInventoryUI.tradeInventory.selectedCharacterInventory.gameObject == CharacterManager.globalReference.playerParty[partyCharacterIndex_].gameObject)
                    {
                        this.tradeInventoryUIObject.SetActive(false);
                    }
                    //If the displayed character in the trade inventory screen is NOT this character, the trade inventory UI will display this character
                    else
                    {
                        //Sets the global reference to the trade character inventory to the selected character's inventory component
                        this.tradeInventoryUIObject.GetComponent<CharacterInventoryUI>().selectedCharacterInventory = CharacterManager.globalReference.playerParty[partyCharacterIndex_].charInventory;
                        //Refreshes the trade inventory UI to display this character's inventory
                        this.tradeInventoryUIObject.GetComponent<CharacterInventoryUI>().UpdateImages();
                    }
                }
            }
        }
        //If the player inventory screen isn't showing
        else
        {
            //Sets the global reference to the trade character inventory to the selected character's inventory component
            this.partyInventoryUIObject.GetComponent<CharacterInventoryUI>().selectedCharacterInventory = CharacterManager.globalReference.playerParty[partyCharacterIndex_].charInventory;
            //Makes sure the trade Inventory UI screen is opened
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

        //Sets the party inventory to show the first character in the player party list's inventory
        foreach (Character selectedChar in CharacterManager.globalReference.selectedGroup.charactersInParty)
        {
            CharacterInventoryUI.partyInventory.selectedCharacterInventory = selectedChar.charInventory;
            break;
        }
        
        //Shows the party inventory UI screen
        this.partyInventoryUIObject.SetActive(true);
    }
}
