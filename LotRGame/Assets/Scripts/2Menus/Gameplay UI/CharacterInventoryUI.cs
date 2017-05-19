using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventoryUI : MonoBehaviour
{
    //Reference to the selected character whose invintory will be shown
    [HideInInspector]
    public Inventory selectedCharacterInventory;

    //Selected Character Name
    public Text selectedCharacterName;
    //Selected Character weight
    public Text selectedCharacterWeight;

    //Selected Character equipped items
    public Image head;
    public Image torso;
    public Image legs;
    public Image feet;
    public Image hands;
    public Image necklace;
    public Image cloak;
    public Image ring;
    public Image rightHand;
    public Image leftHand;

    //Selected Character inventory items
    public List<Image> slotImages;



    // Use this for initialization
    void Start ()
    {
		
	}
	
	
    //Function called when this component is enabled
    private void OnEnable()
    {
        //Gets the inventory reference to the selected character at the front of the list
        if(CharacterManager.globalReference.selectedCharacters.Count > 0)
        {
            this.ChangeCharacter(CharacterManager.globalReference.selectedCharacters[0]);
        }
        //Otherwise, gets the reference to the first character in the party
        else
        {
            this.ChangeCharacter(CharacterManager.globalReference.playerParty[0]);
        }
    }


    //Function called to change character references
    public void ChangeCharacter(Character newCharacterRef_)
    {
        //Gets the reference to the inventory component
        this.selectedCharacterInventory = newCharacterRef_.GetComponent<Inventory>();
        //Sets the name of the character in the text slot
        this.selectedCharacterName.text = newCharacterRef_.firstName + " " + newCharacterRef_.lastName;

        //Updates all of the images in the UI
        this.UpdateImages();
    }


    //Function called to update the inventory images and weight
    private void UpdateImages()
    {
        //Sets the weight text
        this.selectedCharacterWeight.text = "Weight: " + this.selectedCharacterInventory.currentWeight;

        //Sets the image of the head item
        if(this.selectedCharacterInventory.helm != null)
        {
            this.head.enabled = true;
            this.head.sprite = this.selectedCharacterInventory.helm.GetComponent<Item>().icon;
        }
        else
        {
            this.head.enabled = false;
        }

        //Sets the image of the torso item
        if (this.selectedCharacterInventory.chestPiece != null)
        {
            this.torso.enabled = true;
            this.torso.sprite = this.selectedCharacterInventory.chestPiece.GetComponent<Item>().icon;
        }
        else
        {
            this.torso.enabled = false;
        }

        //Sets the image of the leg item
        if (this.selectedCharacterInventory.leggings != null)
        {
            this.legs.enabled = true;
            this.legs.sprite = this.selectedCharacterInventory.leggings.GetComponent<Item>().icon;
        }
        else
        {
            this.legs.enabled = false;
        }

        //Sets the image of the feet item
        if (this.selectedCharacterInventory.shoes != null)
        {
            this.feet.enabled = true;
            this.feet.sprite = this.selectedCharacterInventory.shoes.GetComponent<Item>().icon;
        }
        else
        {
            this.feet.enabled = false;
        }

        //Sets the image of the hand item
        if (this.selectedCharacterInventory.gloves != null)
        {
            this.hands.enabled = true;
            this.hands.sprite = this.selectedCharacterInventory.gloves.GetComponent<Item>().icon;
        }
        else
        {
            this.hands.enabled = false;
        }

        //Sets the image of the neck item
        if (this.selectedCharacterInventory.necklace != null)
        {
            this.necklace.enabled = true;
            this.necklace.sprite = this.selectedCharacterInventory.necklace.GetComponent<Item>().icon;
        }
        else
        {
            this.necklace.enabled = false;
        }

        //Sets the image of the cloak item
        if (this.selectedCharacterInventory.cloak != null)
        {
            this.cloak.enabled = true;
            this.cloak.sprite = this.selectedCharacterInventory.cloak.GetComponent<Item>().icon;
        }
        else
        {
            this.cloak.enabled = false;
        }

        //Sets the image of the ring item
        if (this.selectedCharacterInventory.ring != null)
        {
            this.ring.enabled = true;
            this.ring.sprite = this.selectedCharacterInventory.ring.GetComponent<Item>().icon;
        }
        else
        {
            this.ring.enabled = false;
        }

        //Sets the image of the right hand item
        if (this.selectedCharacterInventory.rightHand != null)
        {
            this.rightHand.enabled = true;
            this.rightHand.sprite = this.selectedCharacterInventory.rightHand.GetComponent<Item>().icon;
        }
        else
        {
            this.rightHand.enabled = false;
        }

        //Sets the image of the torso item
        if (this.selectedCharacterInventory.leftHand != null)
        {
            this.leftHand.enabled = true;
            this.leftHand.sprite = this.selectedCharacterInventory.leftHand.GetComponent<Item>().icon;
        }
        else
        {
            this.leftHand.enabled = false;
        }


        //Loops through each of the item slots to set their images
        for(int i = 0; i < this.selectedCharacterInventory.itemSlots.Count; ++i)
        {
            //If the current item slot is empty, the image is disabled
            if(this.selectedCharacterInventory.itemSlots[i] == null)
            {
                this.slotImages[i].enabled = false;
            }
            //If the slot isn't empty, the image is enabled and set to the item's icon
            else
            {
                this.slotImages[i].enabled = true;
                this.slotImages[i].sprite = this.selectedCharacterInventory.itemSlots[i].GetComponent<Item>().icon;
            }
        }
    }
}
