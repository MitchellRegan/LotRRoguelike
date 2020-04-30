using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventoryUI : MonoBehaviour
{
    //Enum for the type of inventory. Party: Only shows characters in the party. Trade: Shows characters that can trade with the Party character. Bag: A non-character inventory like a bag or chest
    public InventoryType inventoryUIType = InventoryType.Party;

    //Reference to the selected character whose invintory will be shown
    [HideInInspector]
    public Inventory selectedCharacterInventory;

    //Static reference to the different Character Inventory UI based on their types
    public static CharacterInventoryUI partyInventory;
    public static CharacterInventoryUI bagInventory;
    public static CharacterInventoryUI tradeInventory;

    //The image that displays this character's combat appearance
    public GameObject playerSpriteLoc;
    //The direction that the character sprite base is currently looking in the inventory screen
    private DirectionFacing spriteDirection = DirectionFacing.Down;

    //Selected Character's Name
    public Text selectedCharacterName;
    //Selected Character's weight
    public Text selectedCharacterWeight;
    //Selected Character's physical armor
    public Text selectedCharacterArmorSlashing;
    public Text selectedCharacterArmorStabbing;
    public Text selectedCharacterArmorCrushing;
    //Selected Character's money
    public Text selectedCharacterWallet;

    //Selected Character magic resist
    public Text selectedCharacterResistArcane;
    public Text selectedCharacterResistFire;
    public Text selectedCharacterResistWater;
    public Text selectedCharacterResistElectric;
    public Text selectedCharacterResistWind;
    public Text selectedCharacterResistHoly;
    public Text selectedCharacterResistDark;
    public Text selectedCharacterResistNature;

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
    
	

    //Function called when this component is enabled
    private void OnEnable()
    {
        //If this inventory UI is for the bag/loot inventory
        if(this.inventoryUIType == InventoryType.Bag)
        {
            this.selectedCharacterInventory = InventoryOpener.globalReference.bagInventory;
        }
        //If there is no selected inventory, this object is immediately disabled
        else if(this.selectedCharacterInventory == null)
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
        //Sets the character image
        if(this.playerSpriteLoc != null)
        {
            //Finding any child objects on this object to remove a previous player image
            List<GameObject> allChildren = new List<GameObject>();
            for(int c = 0; c < this.playerSpriteLoc.transform.childCount; ++c)
            {
                allChildren.Add(this.playerSpriteLoc.transform.GetChild(c).gameObject);
            }
            //Deleting each child object
            for(int d = 0; d < allChildren.Count; ++d)
            {
                Destroy(allChildren[d].gameObject);
            }

            //Creating a new sprite base for the selected character
            GameObject newSpriteBase = GameObject.Instantiate(this.selectedCharacterInventory.GetComponent<Character>().charSprites.allSprites.spriteBase.gameObject);

            //Parenting the sprite base to our sprite location object
            newSpriteBase.transform.SetParent(this.playerSpriteLoc.transform);
            newSpriteBase.transform.localPosition = new Vector3();
            newSpriteBase.transform.localScale = new Vector3(newSpriteBase.transform.localScale.x * this.playerSpriteLoc.transform.localScale.x * 1.2f,
                                                            newSpriteBase.transform.localScale.y * this.playerSpriteLoc.transform.localScale.y * 1.2f,
                                                            newSpriteBase.transform.localScale.z * this.playerSpriteLoc.transform.localScale.z);

            //Getting the reference to the sprite base component
            CharacterSpriteBase charSprite = newSpriteBase.GetComponent<CharacterSpriteBase>();
            //Setting the character sprite base's sprites
            Character selectedCharacter = this.selectedCharacterInventory.GetComponent<Character>();
            charSprite.SetSpriteImages(selectedCharacter.charSprites.allSprites, selectedCharacter.charInventory);
            charSprite.SetDirectionFacing(this.spriteDirection);
        }

        //Sets the character name
        if(this.selectedCharacterName != null)
        {
            Character characterReference = this.selectedCharacterInventory.GetComponent<Character>();
            this.selectedCharacterName.text = characterReference.firstName + " " + characterReference.lastName;
        }

        //Sets the weight text
        if (this.selectedCharacterWeight != null)
        {
            this.selectedCharacterWeight.text = "Weight: " + this.selectedCharacterInventory.currentWeight;
        }

        //Sets the money text
        if(this.selectedCharacterWallet != null)
        {
            this.selectedCharacterWallet.text = "" + this.selectedCharacterInventory.wallet + " $";
        }

        //Sets the armor text
        if (this.selectedCharacterArmorSlashing != null)
        {
            this.selectedCharacterArmorSlashing.text = "" + this.selectedCharacterInventory.totalSlashingArmor;
        }
        if (this.selectedCharacterArmorStabbing != null)
        {
            this.selectedCharacterArmorStabbing.text = "" + this.selectedCharacterInventory.totalStabbingArmor;
        }
        if (this.selectedCharacterArmorCrushing != null)
        {
            this.selectedCharacterArmorCrushing.text = "" + this.selectedCharacterInventory.totalCrushingArmor;
        }
        //Sets magic resist texts
        if (this.selectedCharacterResistArcane != null)
        {
            this.selectedCharacterResistArcane.text = "" + this.selectedCharacterInventory.totalArcaneResist;
        }
        if (this.selectedCharacterResistFire != null)
        {
            this.selectedCharacterResistFire.text = "" + this.selectedCharacterInventory.totalFireResist;
        }
        if (this.selectedCharacterResistWater != null)
        {
            this.selectedCharacterResistWater.text = "" + this.selectedCharacterInventory.totalWaterResist;
        }
        if (this.selectedCharacterResistElectric != null)
        {
            this.selectedCharacterResistElectric.text = "" + this.selectedCharacterInventory.totalElectricResist;
        }
        if (this.selectedCharacterResistWind != null)
        {
            this.selectedCharacterResistWind.text = "" + this.selectedCharacterInventory.totalWindResist;
        }
        if (this.selectedCharacterResistHoly != null)
        {
            this.selectedCharacterResistHoly.text = "" + this.selectedCharacterInventory.totalHolyResist;
        }
        if (this.selectedCharacterResistDark != null)
        {
            this.selectedCharacterResistDark.text = "" + this.selectedCharacterInventory.totalDarkResist;
        }
        if (this.selectedCharacterResistNature != null)
        {
            this.selectedCharacterResistNature.text = "" + this.selectedCharacterInventory.totalNatureResist;
        }

        //If this inventory displays a character and their armor
        if (this.inventoryUIType != InventoryType.Bag)
        {
            //Sets the image of the head item
            if (this.selectedCharacterInventory.helm != null)
            {
                this.head.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.helm.GetComponent<Item>().icon);
                this.head.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.head.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.head.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the torso item
            if (this.selectedCharacterInventory.chestPiece != null)
            {
                this.torso.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.chestPiece.GetComponent<Item>().icon);
                this.torso.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.torso.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.torso.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the leg item
            if (this.selectedCharacterInventory.leggings != null)
            {
                this.legs.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.leggings.GetComponent<Item>().icon);
                this.legs.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.legs.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.legs.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the feet item
            if (this.selectedCharacterInventory.shoes != null)
            {
                this.feet.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.shoes.GetComponent<Item>().icon);
                this.feet.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.feet.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.feet.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the glove item
            if (this.selectedCharacterInventory.gloves != null)
            {
                this.hands.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.gloves.GetComponent<Item>().icon);
                this.hands.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.hands.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.hands.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the neck item
            if (this.selectedCharacterInventory.necklace != null)
            {
                this.necklace.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.necklace.GetComponent<Item>().icon);
                this.necklace.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.necklace.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.necklace.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the cloak item
            if (this.selectedCharacterInventory.cloak != null)
            {
                this.cloak.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.cloak.GetComponent<Item>().icon);
                this.cloak.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.cloak.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.cloak.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the ring item
            if (this.selectedCharacterInventory.ring != null)
            {
                this.ring.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.ring.GetComponent<Item>().icon);
                this.ring.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.ring.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.ring.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the right hand item
            if (this.selectedCharacterInventory.rightHand != null)
            {
                this.rightHand.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.rightHand.GetComponent<Item>().icon);
                this.rightHand.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.rightHand.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.rightHand.GetComponent<InventoryButton>().slotIsEmpty = true;
            }

            //Sets the image of the left hand item
            if (this.selectedCharacterInventory.leftHand != null)
            {
                this.leftHand.GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.leftHand.GetComponent<Item>().icon);
                this.leftHand.GetComponent<InventoryButton>().slotIsEmpty = false;
            }
            else
            {
                this.leftHand.GetComponent<InventoryButton>().SetButtonIcon(null);
                this.leftHand.GetComponent<InventoryButton>().slotIsEmpty = true;
            }
        }

        //Loops through each of the item slots to set their images
        for(int i = 0; i < this.selectedCharacterInventory.itemSlots.Count; ++i)
        {
            //If the current item slot is empty, the image is disabled
            if(this.selectedCharacterInventory.itemSlots[i] == null)
            {
                this.slotImages[i].GetComponent<InventoryButton>().SetButtonIcon(null);
                this.slotImages[i].GetComponent<InventoryButton>().slotIsEmpty = true;
            }
            //If the slot isn't empty, the image is enabled and set to the item's icon
            else
            {
                this.slotImages[i].GetComponent<InventoryButton>().SetButtonIcon(this.selectedCharacterInventory.itemSlots[i].GetComponent<Item>().icon);
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
    public ArmorSlot GetArmorSlotFromImage(Image slotImage_)
    {
        //Checking each of the armor slot images in this UI
        if(this.head == slotImage_)
        {
            return ArmorSlot.Head;
        }
        else if(this.torso == slotImage_)
        {
            return ArmorSlot.Torso;
        }
        else if(this.legs == slotImage_)
        {
            return ArmorSlot.Legs;
        }
        else if(this.feet == slotImage_)
        {
            return ArmorSlot.Feet;
        }
        else if(this.hands == slotImage_)
        {
            return ArmorSlot.Hands;
        }
        else if(this.cloak == slotImage_)
        {
            return ArmorSlot.Cloak;
        }
        else if(this.necklace == slotImage_)
        {
            return ArmorSlot.Necklace;
        }
        else if(this.ring == slotImage_)
        {
            return ArmorSlot.Ring;
        }

        //If the image wasn't found, there isn't a slot
        return ArmorSlot.None;
    }


    //Function called from InventoryButton.cs to find the correct hand slot of a button
    public CharacterHands GetWeaponHandSlotFromImage(Image slotImage_)
    {
        //If the image is the same one used for the right hand
        if(slotImage_ == this.rightHand)
        {
            return CharacterHands.Right;
        }
        //If the image is the same one used for the left hand
        else
        {
            return CharacterHands.Left;
        }
    }


    //Function called externally from the UI buttons. Cycles through the player party to change the displayed player character
    public void CycleThroughParty(bool cycleForward_)
    {
        //Finding the index of our current character in the party
        int currentCharIndex = CharacterManager.globalReference.playerParty.IndexOf(this.selectedCharacterInventory.GetComponent<Character>());

        //If we cycle forward, we find the next character in line
        if (cycleForward_)
        {
            currentCharIndex += 1;

            if (currentCharIndex == CharacterManager.globalReference.playerParty.Count)
            {
                currentCharIndex = 0;
            }

            //Looping through until we find a non-null character slot in the party
            for(int c = currentCharIndex; c < CharacterManager.globalReference.playerParty.Count; ++c)
            {
                //If we find an index with a character, we set it as the one we display
                if(CharacterManager.globalReference.playerParty[c] != null)
                {
                    this.selectedCharacterInventory = CharacterManager.globalReference.playerParty[c].GetComponent<Inventory>();
                    this.UpdateImages();
                    break;
                }

                //If the loop is about to go out of bounds of the player party list, it's reset back to the beginning
                if(c + 1 == CharacterManager.globalReference.playerParty.Count)
                {
                    c = -1;
                }
            }
        }
        //If we cycle backward, we find the character before this one
        else
        {
            currentCharIndex -= 1;

            if (currentCharIndex == -1)
            {
                currentCharIndex = CharacterManager.globalReference.playerParty.Count - 1;
            }
            //Looping through backward until we find a non-null character slot in the party
            for (int c = currentCharIndex; c >= 0; --c)
            {
                //If we find an index with a character, we set it as the one we display
                if (CharacterManager.globalReference.playerParty[c] != null)
                {
                    this.selectedCharacterInventory = CharacterManager.globalReference.playerParty[c].GetComponent<Inventory>();
                    this.UpdateImages();
                    break;
                }

                //If the loop is about to go out of bounds of the player party list, it's reset back to the end
                if (c - 1 == -1)
                {
                    c = CharacterManager.globalReference.playerParty.Count;
                }
            }
        }
    }


    //Function called externally from the UI buttons. Rotates the sprite base 90 degrees
    public void RotateSpriteBase(bool rotateRight_)
    {
        //If we rotate the sprite base right
        if(rotateRight_)
        {
            //Changing our direction
            switch(this.spriteDirection)
            {
                case DirectionFacing.Down:
                    this.spriteDirection = DirectionFacing.Right;
                    break;
                case DirectionFacing.Right:
                    this.spriteDirection = DirectionFacing.Up;
                    break;
                case DirectionFacing.Up:
                    this.spriteDirection = DirectionFacing.Left;
                    break;
                case DirectionFacing.Left:
                    this.spriteDirection = DirectionFacing.Down;
                    break;
            }
        }
        //If we rotate the sprite base left
        else
        {
            //Changing our direction
            switch (this.spriteDirection)
            {
                case DirectionFacing.Down:
                    this.spriteDirection = DirectionFacing.Left;
                    break;
                case DirectionFacing.Right:
                    this.spriteDirection = DirectionFacing.Down;
                    break;
                case DirectionFacing.Up:
                    this.spriteDirection = DirectionFacing.Right;
                    break;
                case DirectionFacing.Left:
                    this.spriteDirection = DirectionFacing.Up;
                    break;
            }
        }

        //Updating our sprite view
        this.UpdateImages();
    }


    //Function called externally from the UI buttons. Transfers money from this trade character to the party character
    public void TradeMoney(int amountToTrade_)
    {
        //Making sure this inventory isn't the party character inventory, this inventory has money, and there's an open character inventory
        if(this.inventoryUIType == InventoryType.Party || this.selectedCharacterInventory.wallet == 0 || CharacterInventoryUI.partyInventory == null)
        {
            return;
        }

        int amountToTrade = 0;
        //If the amount of money to trade is more than this character has, we give everything they have
        if(this.selectedCharacterInventory.wallet < amountToTrade_)
        {
            amountToTrade = this.selectedCharacterInventory.wallet;
        }
        //If this character has enough money, we send the full amount
        else
        {
            amountToTrade = amountToTrade_;
        }

        //Subtracting the correct amount from this character's inventory and adding it to the party character's inventory
        this.selectedCharacterInventory.wallet -= amountToTrade;
        CharacterInventoryUI.partyInventory.selectedCharacterInventory.wallet += amountToTrade;

        //Updating the money texts for both of the inventor panels
        if (this.selectedCharacterWallet != null)
        {
            this.selectedCharacterWallet.text = "" + this.selectedCharacterInventory.wallet + " $";
        }
        if (CharacterInventoryUI.partyInventory.selectedCharacterWallet != null)
        {
            CharacterInventoryUI.partyInventory.selectedCharacterWallet.text = "" + CharacterInventoryUI.partyInventory.selectedCharacterInventory.wallet + " $";
        }
    }


    //Function called externally from the UI buttons. Transfers all money from this trade character to the party character
    public void TradeAllMoney()
    {
        //Making sure this inventory isn't the party character inventory, this inventory has money, and there's an open character inventory
        if (this.inventoryUIType == InventoryType.Party || this.selectedCharacterInventory.wallet == 0 || CharacterInventoryUI.partyInventory == null)
        {
            return;
        }

        //Subtracting the correct amount from this character's inventory and adding it to the party character's inventory
        CharacterInventoryUI.partyInventory.selectedCharacterInventory.wallet += this.selectedCharacterInventory.wallet;
        this.selectedCharacterInventory.wallet = 0;

        //Updating the money texts for both of the inventor panels
        if(this.selectedCharacterWallet != null)
        {
            this.selectedCharacterWallet.text = "" + this.selectedCharacterInventory.wallet + " $";
        }
        if(CharacterInventoryUI.partyInventory.selectedCharacterWallet != null)
        {
            CharacterInventoryUI.partyInventory.selectedCharacterWallet.text = "" + CharacterInventoryUI.partyInventory.selectedCharacterInventory.wallet + " $";
        }
    }
}
