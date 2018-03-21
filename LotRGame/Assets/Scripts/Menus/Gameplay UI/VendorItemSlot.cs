using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VendorItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Image that displays this item's icon
    public Image itemIconImage;

    //Image that's used when this button doesn't have an item equipped to it
    public Sprite emptySlotImage;

    //Bool that tracks when the player is dragging this inventory item
    private bool isBeingDragged = false;
    //Bool that determines if this slot is empty. If so, it can't be dragged
    public bool slotIsEmpty = true;

    //Text box that displays the stack size of the item shown
    public Text stackSizeText;

    //Text box that displays the value of the item shown
    public Text itemValueText;
    [HideInInspector]
    public int value = 0;

    //Enum that determines if this button is for buying a vendor item or selling a character item
    public enum VendorButtonType { Buy, Sell };
    public VendorButtonType type = VendorButtonType.Buy;



    //Function called when the player's mouse clicks down on this inventory item
    public void OnPointerDown(PointerEventData eventData_)
    {
        //If the mouse button that was pressed is right click and this button isn't empty
        if(eventData_.button == PointerEventData.InputButton.Right && !this.slotIsEmpty)
        {
            //If this item slot button is an item that the player is buying from the vendor
            if(this.type == VendorButtonType.Buy)
            {

            }
            //If this item slot button is an item that the player is selling to the vendor
            else if(this.type == VendorButtonType.Sell)
            {

            }
        }
    }


    //Function called when this player's mouse releases this inventory item
    public void OnPointerUp(PointerEventData eventData_)
    {

    }


    //Function called every frame
    private void Update()
    {
        //Does noting if not being dragged
        if(!this.isBeingDragged)
        {
            return;
        }
    }


    //Function called externally from VendorPanelUI.cs to set this button slot's display info
    public void SetButtonItem(Item buttonItem_)
    {
        //If no item was given
        if (buttonItem_ == null)
        {
            //Setting this button to be empty
            this.slotIsEmpty = true;
            //Our button displays no image
            this.GetComponent<Image>().sprite = this.emptySlotImage;
            //Our stack size text is empty
            this.stackSizeText.text = "";
            //Our item value text is empty
            this.itemValueText.text = "";
            this.value = 0;
        }
        //If we were given an item to display
        else
        {
            //Setting this button to be not empty
            this.slotIsEmpty = false;
            //Setting our button to display the item icon
            this.itemIconImage.sprite = buttonItem_.icon;
            //Setting the stack size text to show how many are in this stack
            this.stackSizeText.text = "" + buttonItem_.currentStackSize;

            //If the item is a quest item, the cost is nothing because we can't sell quest items
            if (buttonItem_.GetComponent<QuestItem>())
            {
                this.itemValueText.text = "N/A";
                this.slotIsEmpty = true;
            }
            //If the item isn't a quest item, we can sell it
            else
            {
                //Marking this button as not empty so we can use it
                this.slotIsEmpty = false;

                //Float to hold the multiplier for this item's value to this vendor
                float valueMultiplier = 0;

                //Bools to determine what components this item has
                bool isFood = buttonItem_.GetComponent<Food>();
                bool isWeapon = buttonItem_.GetComponent<Weapon>();
                bool isArmor = buttonItem_.GetComponent<Armor>();

                //If this button is a player inventory item that we can sell
                if (this.type == VendorButtonType.Sell)
                {
                    //Bools to determine what item types this vendor accepts
                    bool acceptNormalItem = VendorPanelUI.globalReference.vendorToDisplay.willBuyNormalItem;
                    bool acceptFood = VendorPanelUI.globalReference.vendorToDisplay.willBuyFood;
                    bool acceptWeapon = VendorPanelUI.globalReference.vendorToDisplay.willBuyWeapons;
                    bool acceptArmor = VendorPanelUI.globalReference.vendorToDisplay.willBuyArmor;

                    //If the item has none of the components and is just a base item
                    if (!isFood && !isWeapon && !isArmor)
                    {
                        //If this vendor accepts normal items, we use the normal item markup
                        if (acceptNormalItem)
                        {
                            valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyNormalItemValueMultiplier;
                        }
                        //If this vendor doesn't accept normal items, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                    //If the item is just food
                    else if (isFood && !isWeapon && !isArmor)
                    {
                        //If this vendor accepts food items, we use the food item markup
                        if (acceptFood)
                        {
                            valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyFoodValueMultiplier;
                        }
                        //If this vendor doesn't accept food items, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                    //If the item is just a weapon
                    else if (!isFood && isWeapon && !isArmor)
                    {
                        //If this vendor accepts weapons, we use the weapon item markup
                        if (acceptWeapon)
                        {
                            valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyWeaponValueMultiplier;
                        }
                        //If this vendor doesn't accept weapons, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                    //If the item is just armor
                    else if (!isFood && !isWeapon && isArmor)
                    {
                        //If this vendor accepts armor items, we use the armor item markup
                        if (acceptArmor)
                        {
                            valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyArmorValueMultiplier;
                        }
                        //If this vendor doesn't accept armor items, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                    //If the item is both armor and weapon
                    else if (!isFood && isWeapon && isArmor)
                    {
                        //If this vendor accepts either weapons or armor, we use the highest multiplier
                        if (acceptWeapon || acceptArmor)
                        {
                            //If the weapon multiplier is higher than the armor multiplier, we use it
                            if (VendorPanelUI.globalReference.vendorToDisplay.buyWeaponValueMultiplier > VendorPanelUI.globalReference.vendorToDisplay.buyArmorValueMultiplier)
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyWeaponValueMultiplier;
                            }
                            //Otherwise we use the armor multiplier
                            else
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyArmorValueMultiplier;
                            }
                        }
                        //If this vendor doesn't accept weapons or armor, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                    //If the item is both food and weapon
                    else if (isFood && isWeapon && !isArmor)
                    {
                        //If this vendor accepts either weapons or food, we use the highest multiplier
                        if (acceptWeapon || acceptFood)
                        {
                            //If the weapon multiplier is higher than the food multiplier, we use it
                            if (VendorPanelUI.globalReference.vendorToDisplay.buyWeaponValueMultiplier > VendorPanelUI.globalReference.vendorToDisplay.buyFoodValueMultiplier)
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyWeaponValueMultiplier;
                            }
                            //Otherwise we use the food multiplier
                            else
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyFoodValueMultiplier;
                            }
                        }
                        //If this vendor doesn't accept weapons or food, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                    //If the item is both food and armor
                    else if (isFood && !isWeapon && isArmor)
                    {
                        //If this vendor accepts either food or armor, we use the highest multiplier
                        if (acceptFood || acceptArmor)
                        {
                            //If the weapon multiplier is higher than the armor multiplier, we use it
                            if (VendorPanelUI.globalReference.vendorToDisplay.buyFoodValueMultiplier > VendorPanelUI.globalReference.vendorToDisplay.buyArmorValueMultiplier)
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyFoodValueMultiplier;
                            }
                            //Otherwise we use the armor multiplier
                            else
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyArmorValueMultiplier;
                            }
                        }
                        //If this vendor doesn't accept food or armor, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                    //If the item has all of the components
                    else if (isFood && isWeapon && isArmor)
                    {
                        //If this vendor accepts either food, weapons, or armor, we use the highest multiplier
                        if (acceptFood || acceptWeapon || acceptArmor)
                        {
                            //If the weapon multiplier is higher than the armor multiplier, we use it
                            if (VendorPanelUI.globalReference.vendorToDisplay.buyWeaponValueMultiplier > VendorPanelUI.globalReference.vendorToDisplay.buyArmorValueMultiplier)
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyWeaponValueMultiplier;
                            }
                            //Otherwise we use the armor multiplier
                            else
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyArmorValueMultiplier;
                            }

                            //If the current multiplier is lower than the food multiplier, we use it instead
                            if(valueMultiplier < VendorPanelUI.globalReference.vendorToDisplay.buyFoodValueMultiplier)
                            {
                                valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.buyFoodValueMultiplier;
                            }
                        }
                        //If this vendor doesn't accept food, weapons, or armor, this button becomes marked as empty
                        else
                        {
                            this.slotIsEmpty = true;
                        }
                    }
                }
                //If this button is a vendor item that can be bought
                else if(this.type == VendorButtonType.Buy)
                {
                    //By default the multiplier is the normal item markup
                    valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.normalItemMarkupMultiplier;

                    //If this item is food, we go with the higher markup
                    if(isFood && valueMultiplier < VendorPanelUI.globalReference.vendorToDisplay.foodMarkupMultiplier)
                    {
                        valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.foodMarkupMultiplier;
                    }

                    //If this item is a weapon, we go with the higher markup
                    if(isWeapon && valueMultiplier < VendorPanelUI.globalReference.vendorToDisplay.weaponMarkupMultiplier)
                    {
                        valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.weaponMarkupMultiplier;
                    }

                    //If this item is armor, we go with the higher markup
                    if (isArmor && valueMultiplier < VendorPanelUI.globalReference.vendorToDisplay.armorMarkupMultiplier)
                    {
                        valueMultiplier = VendorPanelUI.globalReference.vendorToDisplay.armorMarkupMultiplier;
                    }
                }

                //Setting this item's value and value text
                this.value = Mathf.RoundToInt(buttonItem_.value * valueMultiplier);
                this.itemValueText.text = "" + this.value;
            }
        }
    }
}
