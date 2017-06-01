using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventoryUI : MonoBehaviour
{
    //Enum for the type of inventory. Party: Only shows characters in the party. Trade: Shows characters that can trade with the Party character. Bag: A non-character inventory like a bag or chest
    public enum InventoryType { Party, Trade, Bag };
    public InventoryType inventoryUIType = InventoryType.Party;

    //Reference to the selected character whose invintory will be shown
    [HideInInspector]
    public Inventory selectedCharacterInventory;

    //Static reference to the different Character Inventory UI based on their types
    public static CharacterInventoryUI partyInventory;
    public static CharacterInventoryUI bagInventory;
    public static CharacterInventoryUI tradeInventory;

    //The sprite that's shown on an empty inventory slot
    public Sprite emptySlotSprite;

    //Selected Character Name
    public Text selectedCharacterName;
    //Selected Character weight
    public Text selectedCharacterWeight;
    //Selected Character physical armor
    public Text selectedCharacterArmorPhysical;
    //Selected Character magic armor
    public Text selectedCharacterArmorMagic;

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

    

    //Function called when this game object is created
    private void Awake()
    {
        //Sets the static UI references based on the type
        if(this.inventoryUIType == InventoryType.Party)
        {
            //If there is no static party inventory, this one becomes the static reference
            if(CharacterInventoryUI.partyInventory == null)
            {
                CharacterInventoryUI.partyInventory = this;
            }
            //If a static party inventory reference exists, this is destroyed
            else
            {
                Destroy(this);
            }
        }
        else if(this.inventoryUIType == InventoryType.Bag)
        {
            //If there is no static bag inventory, this one becomes the static reference
            if (CharacterInventoryUI.bagInventory == null)
            {
                CharacterInventoryUI.bagInventory = this;
            }
            //If a static bag inventory reference exists, this is destroyed
            else
            {
                Destroy(this);
            }
        }
        else if(this.inventoryUIType == InventoryType.Trade)
        {
            //If there is no static trade inventory, this one becomes the static reference
            if (CharacterInventoryUI.tradeInventory == null)
            {
                CharacterInventoryUI.tradeInventory = this;
            }
            //If a static trade inventory reference exists, this is destroyed
            else
            {
                Destroy(this);
            }
        }
    }
	
    //Function called when this component is enabled
    private void OnEnable()
    {
        //If there is no selected inventory, this object is immediately disabled
        if(this.selectedCharacterInventory == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        //Updates the images so the UI is accurate
        this.UpdateImages();
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        //Clears this inventory UI's selected inventory
        this.selectedCharacterInventory = null;
    }


    //Function called to change inventory references
    public void ChangeInventory(Inventory newInventoryRef)
    {
        //Gets the reference to the inventory component
        this.selectedCharacterInventory = newInventoryRef;

        //Sets the name of the character in the text slot if this is a character
        if (this.selectedCharacterName != null && newInventoryRef.GetComponent<Character>())
        {
            this.selectedCharacterName.text = newInventoryRef.GetComponent<Character>().firstName + " " + newInventoryRef.GetComponent<Character>().lastName;
        }

        //Updates all of the images in the UI
        this.UpdateImages();
    }


    //Function called to update the inventory images and weight
    public void UpdateImages()
    {
        //Sets the weight text
        if (this.selectedCharacterWeight != null)
        {
            this.selectedCharacterWeight.text = "Weight: " + this.selectedCharacterInventory.currentWeight;
        }

        //Sets the armor text
        if (this.selectedCharacterArmorPhysical != null)
        {
            this.selectedCharacterArmorPhysical.text = "PA: " + this.selectedCharacterInventory.totalPhysicalArmor;
        }
        if (this.selectedCharacterArmorMagic != null)
        {
            this.selectedCharacterArmorMagic.text = "MA: " + this.selectedCharacterInventory.totalMagicArmor;
        }

        //Sets the image of the head item
        if(this.selectedCharacterInventory.helm != null)
        {
            this.head.sprite = this.selectedCharacterInventory.helm.GetComponent<Item>().icon;
            this.head.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.head.sprite = this.emptySlotSprite;
            this.head.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the torso item
        if (this.selectedCharacterInventory.chestPiece != null)
        {
            this.torso.sprite = this.selectedCharacterInventory.chestPiece.GetComponent<Item>().icon;
            this.torso.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.torso.sprite = this.emptySlotSprite;
            this.torso.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the leg item
        if (this.selectedCharacterInventory.leggings != null)
        {
            this.legs.sprite = this.selectedCharacterInventory.leggings.GetComponent<Item>().icon;
            this.legs.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.legs.sprite = this.emptySlotSprite;
            this.legs.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the feet item
        if (this.selectedCharacterInventory.shoes != null)
        {
            this.feet.sprite = this.selectedCharacterInventory.shoes.GetComponent<Item>().icon;
            this.feet.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.feet.sprite = this.emptySlotSprite;
            this.feet.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the hand item
        if (this.selectedCharacterInventory.gloves != null)
        {
            this.hands.sprite = this.selectedCharacterInventory.gloves.GetComponent<Item>().icon;
            this.hands.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.hands.sprite = this.emptySlotSprite;
            this.hands.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the neck item
        if (this.selectedCharacterInventory.necklace != null)
        {
            this.necklace.sprite = this.selectedCharacterInventory.necklace.GetComponent<Item>().icon;
            this.necklace.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.necklace.sprite = this.emptySlotSprite;
            this.necklace.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the cloak item
        if (this.selectedCharacterInventory.cloak != null)
        {
            this.cloak.sprite = this.selectedCharacterInventory.cloak.GetComponent<Item>().icon;
            this.cloak.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.cloak.sprite = this.emptySlotSprite;
            this.cloak.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the ring item
        if (this.selectedCharacterInventory.ring != null)
        {
            this.ring.sprite = this.selectedCharacterInventory.ring.GetComponent<Item>().icon;
            this.ring.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.ring.sprite = this.emptySlotSprite;
            this.ring.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the right hand item
        if (this.selectedCharacterInventory.rightHand != null)
        {
            this.rightHand.sprite = this.selectedCharacterInventory.rightHand.GetComponent<Item>().icon;
            this.rightHand.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.rightHand.sprite = this.emptySlotSprite;
            this.rightHand.GetComponent<InventoryButton>().slotIsEmpty = true;
        }

        //Sets the image of the left hand item
        if (this.selectedCharacterInventory.leftHand != null)
        {
            this.leftHand.sprite = this.selectedCharacterInventory.leftHand.GetComponent<Item>().icon;
            this.leftHand.GetComponent<InventoryButton>().slotIsEmpty = false;
        }
        else
        {
            this.leftHand.sprite = this.emptySlotSprite;
            this.leftHand.GetComponent<InventoryButton>().slotIsEmpty = true;
        }


        //Loops through each of the item slots to set their images
        for(int i = 0; i < this.selectedCharacterInventory.itemSlots.Count; ++i)
        {
            //If the current item slot is empty, the image is disabled
            if(this.selectedCharacterInventory.itemSlots[i] == null)
            {
                this.slotImages[i].sprite = this.emptySlotSprite;
                this.slotImages[i].GetComponent<InventoryButton>().slotIsEmpty = true;
            }
            //If the slot isn't empty, the image is enabled and set to the item's icon
            else
            {
                this.slotImages[i].sprite = this.selectedCharacterInventory.itemSlots[i].GetComponent<Item>().icon;
                this.slotImages[i].GetComponent<InventoryButton>().slotIsEmpty = false;
            }
        }
    }


    //Function called from InventoryButton.cs to return the item that the button displays
    public Item GetItemFromInventoryButton(Image buttonImage_)
    {
        //Looping through each of the slot images to try and find a match
        for(int i = 0; i < this.slotImages.Count; ++i)
        {
            //If a match is found, the item in the character's inventory at the current index is returned
            if(this.slotImages[i] == buttonImage_)
            {
                return this.selectedCharacterInventory.itemSlots[i];
            }
        }

        //Otherwise, we check each item that's currently equipped
        if(this.head == buttonImage_ && this.selectedCharacterInventory.helm != null)
        {
            return this.selectedCharacterInventory.helm.GetComponent<Item>();
        }
        else if(this.torso == buttonImage_ && this.selectedCharacterInventory.chestPiece != null)
        {
            return this.selectedCharacterInventory.chestPiece.GetComponent<Item>();
        }
        else if(this.legs == buttonImage_ && this.selectedCharacterInventory.leggings != null)
        {
            return this.selectedCharacterInventory.leggings.GetComponent<Item>();
        }
        else if(this.feet == buttonImage_ && this.selectedCharacterInventory.shoes != null)
        {
            return this.selectedCharacterInventory.shoes.GetComponent<Item>();
        }
        else if(this.hands == buttonImage_ && this.selectedCharacterInventory.gloves != null)
        {
            return this.selectedCharacterInventory.gloves.GetComponent<Item>();
        }
        else if(this.necklace == buttonImage_ && this.selectedCharacterInventory.necklace != null)
        {
            return this.selectedCharacterInventory.necklace.GetComponent<Item>();
        }
        else if(this.cloak == buttonImage_ && this.selectedCharacterInventory.cloak != null)
        {
            return this.selectedCharacterInventory.cloak.GetComponent<Item>();
        }
        else if(this.ring == buttonImage_ && this.selectedCharacterInventory.ring != null)
        {
            return this.selectedCharacterInventory.ring.GetComponent<Item>();
        }
        else if(this.rightHand == buttonImage_ && this.selectedCharacterInventory.rightHand != null)
        {
            return this.selectedCharacterInventory.rightHand.GetComponent<Item>();
        }
        else if(this.leftHand == buttonImage_ && this.selectedCharacterInventory.leftHand != null)
        {
            return this.selectedCharacterInventory.leftHand.GetComponent<Item>();
        }

        //If nothing is found, there's nothing there
        return null;
    }


    //Function called from InventoryButton.cs to find the correct armor slot of a button
    public Armor.ArmorSlot GetArmorSlotFromImage(Image slotImage_)
    {
        //Checking each of the armor slot images in this UI
        if(this.head == slotImage_)
        {
            return Armor.ArmorSlot.Head;
        }
        else if(this.torso == slotImage_)
        {
            return Armor.ArmorSlot.Torso;
        }
        else if(this.legs == slotImage_)
        {
            return Armor.ArmorSlot.Legs;
        }
        else if(this.feet == slotImage_)
        {
            return Armor.ArmorSlot.Feet;
        }
        else if(this.hands == slotImage_)
        {
            return Armor.ArmorSlot.Hands;
        }
        else if(this.cloak == slotImage_)
        {
            return Armor.ArmorSlot.Cloak;
        }
        else if(this.necklace == slotImage_)
        {
            return Armor.ArmorSlot.Necklace;
        }
        else if(this.ring == slotImage_)
        {
            return Armor.ArmorSlot.Ring;
        }

        //If the image wasn't found, there isn't a slot
        return Armor.ArmorSlot.None;
    }


    //Function called from InventoryButton.cs to find the correct hand slot of a button
    public Inventory.WeaponHand GetWeaponHandSlotFromImage(Image slotImage_)
    {
        //If the image is the same one used for the right hand
        if(slotImage_ == this.rightHand)
        {
            return Inventory.WeaponHand.Right;
        }
        //If the image is the same one used for the left hand
        else
        {
            return Inventory.WeaponHand.Left;
        }
    }
}
