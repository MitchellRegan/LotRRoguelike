using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorPanelUI : MonoBehaviour
{
    //Global static reference for this vendor UI
    public static VendorPanelUI globalReference;

    //The vendor that is currently being displayed
    [HideInInspector]
    public Vendor vendorToDisplay;

    //Reference to the selected party character for vendor interactions
    private Character selectedCharacter;

    //The text for the name of the building
    public Text vendorBuildingText;
    //The text for the type of vendor it is
    public Text vendorTypeText;

    //The text for the name of the vendor
    public Text vendorNameText;

    [Space(8)]

    //The game object that turns on the buy/sell menu
    public GameObject buySellMenuObj;

    //The game object location for where we spawn the selected player character
    public Transform playerCharSpriteLoc;
    private CharacterSpriteBase playerSprite;

    //The default VendorItemSlot object that we duplicate for all of the items the current vendor can sell
    public VendorItemSlot defaultSaleItemSlot;
    //The list of VendorItemSlot objects that the vendor has for sale
    private List<VendorItemSlot> saleItemSlots;

    //The list of vendor slots that display the selected character's inventory items for sale
    public List<VendorItemSlot> playerInventorySlots;



	// Use this for initialization
	private void Start ()
    {
		//If there isn't a global reference for this vendor UI, this becomes the global ref
        if(globalReference == null)
        {
            globalReference = this;
        }
        //If a global reference already exists, we destroy this component
        else
        {
            Destroy(this);
        }
	}


    //Function called when this game object is enabled
    private void OnEnable()
    {
        //Looping through the characters in the player party to select the first one in line
        foreach(Character pc in CharacterManager.globalReference.selectedGroup.charactersInParty)
        {
            //If the current party character isn't null, it becomes the selected character for this UI
            if(pc != null)
            {
                this.selectedCharacter = pc;
                break;
            }
        }

        //Setting the player character sprite
        this.SetPartyCharSprite();
    }


    //Function called from OnEnable and CyclePartyCharacter to set the selected player character sprite
    private void SetPartyCharSprite()
    {
        //Destroying the player sprite that we previously had
        Destroy(this.playerSprite);

        //Creating an instance of the newly selected character's sprite base
        GameObject spriteBase = GameObject.Instantiate(this.selectedCharacter.charSprites.allSprites.spriteBase.gameObject);
        this.playerSprite = spriteBase.GetComponent<CharacterSpriteBase>();

        //Setting the sprite base's position
        this.playerSprite.transform.localPosition = this.playerCharSpriteLoc.localPosition;

        //Setting the sprite base's sprites and making it face left
        this.playerSprite.SetSpriteImages(this.selectedCharacter.charSprites.allSprites, this.selectedCharacter.charInventory);
        this.playerSprite.SetDirectionFacing(DirectionFacing.Left);
    }


    //Function called externally from UI buttons to cycle through the player party characters
    public void CyclePartyCharacter(bool next_)
    {
        //Getting the index for our currently selected party character
        int currentCharIndex = CharacterManager.globalReference.selectedGroup.charactersInParty.IndexOf(this.selectedCharacter);

        //If we cycle through to the next one in line
        if(next_)
        {
            //If the current character is in the last party index, we cycle through to the beginning
            if(currentCharIndex == CharacterManager.globalReference.selectedGroup.charactersInParty.Count - 1)
            {
                currentCharIndex = 0;
            }

            //Looping through all of the party slots until we find a non-null
            for(int c = currentCharIndex; c < CharacterManager.globalReference.selectedGroup.charactersInParty.Count; ++c)
            {
                //If the character at this index isn't null, it becomes our selected character
                if(CharacterManager.globalReference.selectedGroup.charactersInParty[c] != null)
                {
                    this.selectedCharacter = CharacterManager.globalReference.selectedGroup.charactersInParty[c];
                    this.SetPartyCharSprite();
                    break;
                }
                //If we haven't found a new character and the index is about to go out of range, we loop back around to the beginning
                else if(c + 1 >= CharacterManager.globalReference.selectedGroup.charactersInParty.Count)
                {
                    c = -1;
                }
            }
        }
        //If we cycle through to the previous one in line
        else
        {
            //If the current character is in the first party index, we cycle through to the end
            if (currentCharIndex == 0)
            {
                currentCharIndex = CharacterManager.globalReference.selectedGroup.charactersInParty.Count - 1;
            }

            //Looping through all of the party index slots backwards until we find a non-null
            for(int c = currentCharIndex; c > -1; --c)
            {
                //If the character at this index isn't null, it becomes our selected character
                if(CharacterManager.globalReference.selectedGroup.charactersInParty[c] != null)
                {
                    this.selectedCharacter = CharacterManager.globalReference.selectedGroup.charactersInParty[c];
                    this.SetPartyCharSprite();
                    break;
                }
                //If we haven't found a new character and the index is about to go out of range, we loop back around to the end
                else if(c - 1 <= -1)
                {
                    c = CharacterManager.globalReference.selectedGroup.charactersInParty.Count - 1;
                }
            }
        }
    }


	//Function called externally to display the buy/sell UI page
    public void OpenBuySellUI()
    {
        //Turning on the buy/sell menu
        this.buySellMenuObj.SetActive(true);

        //Looping through all of the inventory items in the selected character's inventory
        for(int i = 0; i < this.selectedCharacter.charInventory.itemSlots.Count; ++i)
        {
            //Setting our VendorItemSlot button of the same index to display this item info
            this.playerInventorySlots[i].SetButtonItem(this.selectedCharacter.charInventory.itemSlots[i]);
        }
    }
}
