﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Image that's used when this button doesn't have an item equipped to it
    public Sprite emptySlotImage;

    //Bool that tracks when the player is dragging this inventory item
    private bool isBeingDragged = false;
    //Bool that determines if this slot is empty. If so, it can't be dragged
    public bool slotIsEmpty = true;

    //Enum that determines what inventory slot this button represents
    public enum InventoryButtonType { Bag, Armor, Weapon, Trash};
    public InventoryButtonType buttonType = InventoryButtonType.Bag;

    //Text box that displays the stack size of the item shown
    public Text stackSizeText;


    
    //Function called when the player's mouse clicks down on this inventory item
    public void OnPointerDown(PointerEventData eventData_)
    {
        //If this slot is empty or the trash button, nothing happens
        if(this.slotIsEmpty || this.buttonType == InventoryButtonType.Trash)
        {
            return;
        }

        //If the player left clicks to drag
        if(eventData_.button == PointerEventData.InputButton.Left)
        {
            //Setting our image opacity to be dim
            this.GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
            //Starts dragging this item and sets it as the front UI element
            this.isBeingDragged = true;

            //Setting the InventoryOpener's drag icon image to this button's image
            InventoryOpener.globalReference.dragIconImage.gameObject.SetActive(true);
            InventoryOpener.globalReference.dragIconImage.sprite = this.GetComponent<Image>().sprite;
        }
        //If the player right clicks to use/equip
        else if(eventData_.button == PointerEventData.InputButton.Right)
        {
            this.UseItem();
        }
    }


    //Function called when the player's mouse releases this inventory item
    public void OnPointerUp(PointerEventData eventData_)
    {
        //If this slot is empty, nothing happens
        if(this.slotIsEmpty)
        {
            return;
        }

        //Ends dragging this item and resets back to the default opacity
        this.isBeingDragged = false;
        InventoryOpener.globalReference.dragIconImage.gameObject.SetActive(false);
        this.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        //Turning off this button's raycast blocking so that we can see what's behind it
        this.GetComponent<Image>().raycastTarget = false;

        //Casting a ray from the pointer to hit all targets under it
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData_, results);
        
        //If the raycast hits something, we check the first result
        if(results.Count > 0)
        {
            //If the first result is a different inventory button, we can swap the items
            if(results[0].gameObject.GetComponent<InventoryButton>())
            {
                //Making sure the button has a parent object with the CharacterInventoryUI component
                if(results[0].gameObject.GetComponentInParent<CharacterInventoryUI>())
                {
                    //Getting references to the inventory UIs of this button and the hit button
                    CharacterInventoryUI thisButtonUI = this.GetComponentInParent<CharacterInventoryUI>();
                    CharacterInventoryUI hitButtonUI = results[0].gameObject.GetComponentInParent<CharacterInventoryUI>();

                    //Getting the items that each button shows from their given inventories
                    Item thisButtonItem = thisButtonUI.GetItemFromInventoryButton(this.GetComponent<UnityEngine.UI.Image>());
                    Item hitButtonItem = hitButtonUI.GetItemFromInventoryButton(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());

                    //If the hit button is a trash button
                    if(results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Trash)
                    {
                        //If the item being trashed isn't a quest item
                        if (!thisButtonItem.GetComponent<QuestGiver>())
                        {
                            //If this button is an inventory button
                            if (this.buttonType == InventoryButtonType.Bag)
                            {
                                //Getting the index of this button for the UI
                                int thisItemIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());

                                //Clearing the item in the inventory slot
                                thisButtonUI.selectedCharacterInventory.itemSlots[thisItemIndex] = null;
                            }
                            //If this button is a weapon button
                            else if(this.buttonType == InventoryButtonType.Weapon)
                            {
                                //If the weapon is in the right hand
                                if(thisButtonUI.selectedCharacterInventory.rightHand.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.rightHand = null;
                                }
                                //If the weapon is in the left hand
                                else if(thisButtonUI.selectedCharacterInventory.leftHand.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.leftHand = null;
                                }
                            }
                            //If this button is an armor button
                            else if(this.buttonType == InventoryButtonType.Armor)
                            {
                                //If this button item is a helm
                                if(thisButtonUI.selectedCharacterInventory.helm.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.helm = null;
                                }
                                //if this button item is a chestpiece
                                else if (thisButtonUI.selectedCharacterInventory.chestPiece.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.chestPiece = null;
                                }
                                //if this button item are leggings
                                else if (thisButtonUI.selectedCharacterInventory.leggings.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.leggings = null;
                                }
                                //if this button item are shoes
                                else if (thisButtonUI.selectedCharacterInventory.shoes.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.shoes = null;
                                }
                                //if this button item are gloves
                                else if (thisButtonUI.selectedCharacterInventory.gloves.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.gloves = null;
                                }
                                //if this button item is a cloak
                                else if (thisButtonUI.selectedCharacterInventory.cloak.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.cloak = null;
                                }
                                //if this button item is a ring
                                else if (thisButtonUI.selectedCharacterInventory.ring.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.ring = null;
                                }
                                //if this button item is a necklace
                                else if (thisButtonUI.selectedCharacterInventory.necklace.gameObject == thisButtonItem.gameObject)
                                {
                                    thisButtonUI.selectedCharacterInventory.necklace = null;
                                }
                            }

                            //We destroy this button's item
                            Destroy(thisButtonItem.gameObject);
                            //Refreshing this button's inventory
                            thisButtonUI.UpdateImages();
                            thisButtonUI.selectedCharacterInventory.FindArmorStats();
                        }
                    }
                    //If neither item is equipped and are just in the regular inventory
                    else if(this.buttonType == InventoryButtonType.Bag && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Bag)
                    {
                        //Finding the index of this button's item so it can be switched
                        int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());

                        //Finding the index of the hit button's item so it can be switched
                        int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());

                        //If the hit button is empty
                        if (hitButtonItem == null)
                        {
                            //Replacing this button's inventory with the hit button's item
                            thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonItem);
                            //Replacing the hit button's inventory with this button's item
                            hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                        }
                        //If the items have the same nameID
                        else if(hitButtonItem.itemNameID == thisButtonItem.itemNameID)
                        {
                            //If the hit button's item stack is full
                            if (hitButtonItem.currentStackSize == hitButtonItem.maxStackSize)
                            {
                                //Replacing this button's inventory with the hit button's item
                                thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonItem);
                                //Replacing the hit button's inventory with this button's item
                                hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                            }
                            //If the hit button's item stack isn't full
                            else
                            {
                                //Move as many stacks from this stack to the hit button's stack as possible
                                while(thisButtonItem.transform.childCount > 0)
                                {
                                    Transform itemChild = thisButtonItem.transform.FindChild(thisButtonItem.name);

                                    //If for some reason there isn't an available child of the same type but there are still children, we break the loop
                                    if(itemChild == null)
                                    {
                                        break;
                                    }

                                    //Making sure there's still room in the hit button's item stack
                                    if(hitButtonItem.currentStackSize >= hitButtonItem.maxStackSize)
                                    {
                                        break;
                                    }

                                    //Setting the item to stack to the hit button's stack
                                    itemChild.SetParent(hitButtonItem.transform);
                                    //Increasing the number of items in the hit button's item stack
                                    hitButtonItem.currentStackSize += 1;
                                    //Decreasing the number of items in this button's item stack
                                    thisButtonItem.currentStackSize -= 1;
                                }

                                //If this button's item is the last in the stack and there's still room left on the hit button's stack
                                if (thisButtonItem.currentStackSize == 1 && hitButtonItem.currentStackSize < hitButtonItem.maxStackSize)
                                {
                                    //Setting this button's item to stack with the hit button's item
                                    thisButtonItem.transform.SetParent(hitButtonItem.transform);
                                    //Increasing the number of items in the hit button's item stack
                                    hitButtonItem.currentStackSize += 1;
                                    //Setting this button's item to be empty
                                    thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, null);
                                }
                            }
                        }
                        //If the items have different nameIDs
                        else
                        {
                            //Replacing this button's inventory with the hit button's item
                            thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonItem);
                            //Replacing the hit button's inventory with this button's item
                            hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                        }
                    }

                    //If both items are in armor slots
                    else if(this.buttonType == InventoryButtonType.Armor && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Armor)
                    {
                        //If the hit button is empty
                        if (hitButtonItem == null)
                        {
                            //Swap the references in the inventories
                            thisButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, null);
                            hitButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, thisButtonItem.GetComponent<Armor>());
                        }
                        //If they're the same type of armor
                        else if (hitButtonItem.GetComponent<Armor>().slot == thisButtonItem.GetComponent<Armor>().slot)
                        {
                            //Swap the references in the inventories
                            thisButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(hitButtonItem.GetComponent<Armor>().slot, hitButtonItem.GetComponent<Armor>());
                            hitButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, thisButtonItem.GetComponent<Armor>());
                        }
                        //If they're different types of armor, nothing happens
                    }

                    //If both items are in weapon slots
                    else if(this.buttonType == InventoryButtonType.Weapon && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Weapon)
                    {
                        //If the hit button is empty
                        if (hitButtonItem == null)
                        {
                            //If this button's weapon is 2 handed
                            if(thisButtonItem.GetComponent<Weapon>().size == Weapon.WeaponSize.TwoHands)
                            {
                                //If the hit button's inventory has no weapon equipped in either hand
                                if (hitButtonUI.selectedCharacterInventory.rightHand == null && hitButtonUI.selectedCharacterInventory.leftHand == null)
                                {
                                    //Swap the references of this button's item with the hit button's inventory's right hand
                                    hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                    thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, null);
                                }
                                //If the hit button's inventory has a 2 handed weapon equipped
                                else if (hitButtonUI.selectedCharacterInventory.rightHand.size == Weapon.WeaponSize.TwoHands)
                                {
                                    //Swap the references of this button's item with the hit button's inventory's right hand
                                    hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                    thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                }
                                //If the hit button's inventory has a weapon equipped in the other hand
                                else
                                {
                                    //If the hit button's inventory has a free space for the equipped weapon
                                    if (hitButtonUI.selectedCharacterInventory.CheckForEmptySlot() > 0)
                                    {
                                        //Put the hit button's inventory's weapon in their inventory
                                        hitButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Both);
                                        //Equip this weapon in the hit button's inventory's right hand
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, null);
                                    }
                                }
                            }
                            //If this button's weapon is 1 handed
                            else
                            {
                                //If the hit button's inventory has a 2 handed weapon equipped in the right hand
                                if (hitButtonUI.selectedCharacterInventory.rightHand.size == Weapon.WeaponSize.TwoHands)
                                {
                                    //If this button's inventory has a weapon equipped in the other hand
                                    if (thisButtonUI.selectedCharacterInventory.rightHand != null && thisButtonUI.selectedCharacterInventory.leftHand != null)
                                    {
                                        //If this button's inventory has a free space for the other weapon
                                        if (thisButtonUI.selectedCharacterInventory.CheckForEmptySlot() > 0)
                                        {
                                            //If this button's weapon is in the left hand
                                            if (thisButtonUI.selectedCharacterInventory.rightHand != thisButtonItem)
                                            {
                                                //Unequip the 1 handed weapon in this button's inventory's right hand
                                                thisButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Right);

                                                //Swaps this button's 1 handed weapon to the hit button's inventory's left hand
                                                hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, thisButtonItem.GetComponent<Weapon>());
                                                //Swaps the hit button's 2 handed weapon to this button's inventory's right hand
                                                thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                                //Sets this button's left hand to be empty
                                                thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);
                                            }
                                            //If this button's weapon is in the right hand
                                            else
                                            {
                                                //Unequip the 1 handed weapon in this button's inventory's left hand
                                                thisButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Left);

                                                //Swaps this button's 1 handed weapon to the hit button's inventory's right hand
                                                hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                                //Swaps the hit button's 2 handed weapon to this button's inventory's 
                                                thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                            }
                                        }
                                        //If there's no free space, nothing happens
                                    }
                                    //If this button's inventory doesn't have a weapon equipped in the other hand
                                    if (thisButtonUI.selectedCharacterInventory.rightHand == null || thisButtonUI.selectedCharacterInventory.leftHand == null)
                                    {
                                        //If this button's weapon is in the left hand
                                        if (thisButtonUI.selectedCharacterInventory.rightHand != thisButtonItem)
                                        {
                                            //Swaps this button's 1 handed weapon to the hit button's inventory's left hand
                                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, thisButtonItem.GetComponent<Weapon>());
                                            //Swaps the hit button's 2 handed weapon to this button's inventory's right hand
                                            thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                            //Sets this button's left hand to be empty
                                            thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);
                                        }
                                        //If this button's weapon is in the right hand
                                        else
                                        {
                                            //Swaps this button's 1 handed weapon to the hit button's inventory's right hand
                                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                            //Swaps the hit button's 2 handed weapon to this button's inventory's 
                                            thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                        }
                                    }
                                }
                                //If the hit button's inventory has a 1 handed weapon in the other hand or nothing
                                else
                                {
                                    //Finding out which hand this button's weapon is in
                                    Inventory.WeaponHand thisWeaponsHand = Inventory.WeaponHand.Right;
                                    if(thisButtonUI.selectedCharacterInventory.rightHand.gameObject != thisButtonItem.gameObject)
                                    {
                                        thisWeaponsHand = Inventory.WeaponHand.Left;
                                    }

                                    //Finding out which hand the hit button's weapon is in
                                    Inventory.WeaponHand hitWeaponsHand = Inventory.WeaponHand.Right;
                                    if(hitButtonUI.selectedCharacterInventory.rightHand.gameObject != hitButtonItem.gameObject)
                                    {
                                        hitWeaponsHand = Inventory.WeaponHand.Left;
                                    }

                                    //Swapping the references in both inventories
                                    hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(hitWeaponsHand, thisButtonItem.GetComponent<Weapon>());
                                    thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(thisWeaponsHand, hitButtonItem.GetComponent<Weapon>());
                                }
                            }
                        }
                        //If they're the same type of weapon
                        if (hitButtonItem.GetComponent<Weapon>().size == thisButtonItem.GetComponent<Weapon>().size)
                        {
                            //Finding out which hand this button's weapon is in
                            Inventory.WeaponHand thisWeaponsHand = Inventory.WeaponHand.Right;
                            if (thisButtonUI.selectedCharacterInventory.rightHand.gameObject != thisButtonItem.gameObject)
                            {
                                thisWeaponsHand = Inventory.WeaponHand.Left;
                            }

                            //Finding out which hand the hit button's weapon is in
                            Inventory.WeaponHand hitWeaponsHand = Inventory.WeaponHand.Right;
                            if (hitButtonUI.selectedCharacterInventory.rightHand.gameObject != hitButtonItem.gameObject)
                            {
                                hitWeaponsHand = Inventory.WeaponHand.Left;
                            }

                            //Swapping the references in both inventories
                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(hitWeaponsHand, thisButtonItem.GetComponent<Weapon>());
                            thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(thisWeaponsHand, hitButtonItem.GetComponent<Weapon>());
                        }
                        //If one weapon is 2 handed and the other isn't
                        else
                        {
                            //If the hit weapon is the 2 handed one
                            if (hitButtonUI.selectedCharacterInventory.rightHand.size == Weapon.WeaponSize.TwoHands)
                            {
                                //If this button's inventory has a weapon equipped in the other hand
                                if (thisButtonUI.selectedCharacterInventory.rightHand != null && thisButtonUI.selectedCharacterInventory.leftHand != null)
                                {
                                    //If this button's inventory has a free space for the other weapon
                                    if (thisButtonUI.selectedCharacterInventory.CheckForEmptySlot() > 0)
                                    {
                                        //If this button's weapon is in the left hand
                                        if (thisButtonUI.selectedCharacterInventory.rightHand != thisButtonItem)
                                        {
                                            //Unequip the 1 handed weapon in this button's inventory's right hand
                                            thisButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Right);

                                            //Swaps this button's 1 handed weapon to the hit button's inventory's left hand
                                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, thisButtonItem.GetComponent<Weapon>());
                                            //Swaps the hit button's 2 handed weapon to this button's inventory's right hand
                                            thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                            //Sets this button's left hand to be empty
                                            thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);
                                        }
                                        //If this button's weapon is in the right hand
                                        else
                                        {
                                            //Unequip the 1 handed weapon in this button's inventory's left hand
                                            thisButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Left);

                                            //Swaps this button's 1 handed weapon to the hit button's inventory's right hand
                                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                            //Swaps the hit button's 2 handed weapon to this button's inventory's 
                                            thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                        }
                                    }
                                    //If there's no free space, nothing happens
                                }
                                //If this button's inventory doesn't have a weapon equipped in the other hand
                                if (thisButtonUI.selectedCharacterInventory.rightHand == null || thisButtonUI.selectedCharacterInventory.leftHand == null)
                                {
                                    //If this button's weapon is in the left hand
                                    if (thisButtonUI.selectedCharacterInventory.rightHand != thisButtonItem)
                                    {
                                        //Swaps this button's 1 handed weapon to the hit button's inventory's left hand
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, thisButtonItem.GetComponent<Weapon>());
                                        //Swaps the hit button's 2 handed weapon to this button's inventory's right hand
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                        //Sets this button's left hand to be empty
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);
                                    }
                                    //If this button's weapon is in the right hand
                                    else
                                    {
                                        //Swaps this button's 1 handed weapon to the hit button's inventory's right hand
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                        //Swaps the hit button's 2 handed weapon to this button's inventory's 
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                    }
                                }
                            }
                            //If the hit button is the 1 handed one
                            else
                            {
                                //Finding out which hand this button's weapon is in
                                Inventory.WeaponHand thisWeaponsHand = Inventory.WeaponHand.Right;
                                if (thisButtonUI.selectedCharacterInventory.rightHand.gameObject != thisButtonItem.gameObject)
                                {
                                    thisWeaponsHand = Inventory.WeaponHand.Left;
                                }

                                //Finding out which hand the hit button's weapon is in
                                Inventory.WeaponHand hitWeaponsHand = Inventory.WeaponHand.Right;
                                if (hitButtonUI.selectedCharacterInventory.rightHand.gameObject != hitButtonItem.gameObject)
                                {
                                    hitWeaponsHand = Inventory.WeaponHand.Left;
                                }

                                //Swapping the references in both inventories
                                hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(hitWeaponsHand, thisButtonItem.GetComponent<Weapon>());
                                thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(thisWeaponsHand, hitButtonItem.GetComponent<Weapon>());
                            }
                        }
                    }

                    //If one button is a weapon slot and the other is an armor slot
                    /*else if((this.buttonType == InventoryButtonType.Armor && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Weapon) ||
                            (this.buttonType == InventoryButtonType.Weapon && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Armor))
                    {
                        //Nothing happens
                    }*/
                    
                    //If the hit button is an armor slot and this button is an inventory slot
                    else if(this.buttonType == InventoryButtonType.Bag && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Armor)
                    {
                        //If this button's item is armor
                        if(thisButtonItem.GetComponent<Armor>())
                        {
                            //If the hit button doesn't have armor equipped
                            if (hitButtonItem == null)
                            {
                                //Find out what type of armor goes into the inventory slot
                                Armor.ArmorSlot hitButtonSlot = hitButtonUI.GetArmorSlotFromImage(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());

                                //If this button's armor is the same type
                                if (thisButtonItem.GetComponent<Armor>().slot == hitButtonSlot)
                                {
                                    //Find the index of this button's item in this inventory
                                    int thisButtonsItemIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());

                                    //Sets this button's item to the hit button's armor's slot
                                    hitButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, thisButtonItem.GetComponent<Armor>());
                                    //Sets this button's inventory slot to be empty
                                    thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonsItemIndex, null);
                                }
                            }
                            //If the hit button has armor equipped
                            else if (hitButtonItem.GetComponent<Armor>())
                            {
                                //If both items are the same type of armor
                                if (hitButtonItem.GetComponent<Armor>().slot == thisButtonItem.GetComponent<Armor>().slot)
                                {
                                    //Find the index of this button's item in this inventory
                                    int thisButtonsItemIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());

                                    //Sets this button's item to the hit button's armor's slot
                                    hitButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, thisButtonItem.GetComponent<Armor>());
                                    //Sets the hit button's armor to this button's inventory slot
                                    thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonsItemIndex, hitButtonItem);
                                }
                                //If both items are different types of armor, nothing happens
                            }
                        }
                        //If this button's item isn't armor, nothing happens
                    }

                    //If the hit button is an inventory slot and this button is an armor slot
                    else if(this.buttonType == InventoryButtonType.Armor && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Bag)
                    {
                        //If the hit button is empty
                        if (hitButtonItem == null)
                        {
                            //Swap the references in the inventories
                            thisButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, null);

                            //Finding the index of the hit button's item so it can be switched
                            int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                            hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                        }
                        //If the hit button's item is armor
                        else if(hitButtonItem.GetComponent<Armor>())
                        {
                            //If the hit button's item has the same nameID as this button's armor
                            if (hitButtonItem.GetComponent<Item>().itemNameID == thisButtonItem.GetComponent<Item>().itemNameID)
                            {
                                //If the hit button's item has room for one more stack
                                if (hitButtonItem.GetComponent<Item>().currentStackSize < hitButtonItem.GetComponent<Item>().maxStackSize)
                                {
                                    //Stack this button's armor onto the hit button's
                                    hitButtonItem.GetComponent<Item>().currentStackSize += 1;
                                    thisButtonItem.transform.SetParent(hitButtonItem.transform);

                                    //Null this button's slot
                                    thisButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, null);
                                }
                                //If the hit button can't stack, nothing happens
                            }
                            //If the hit button's armor is the same type as this button's armor
                            else if (hitButtonItem.GetComponent<Armor>().slot == thisButtonItem.GetComponent<Armor>().slot)
                            {
                                //Swap the references in the inventories
                                thisButtonUI.selectedCharacterInventory.ChangeArmorItemAtSlot(thisButtonItem.GetComponent<Armor>().slot, hitButtonItem.GetComponent<Armor>());

                                //Finding the index of the hit button's item so it can be switched
                                int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                                hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                            }
                            //If the hit button's armor isn't the same type, nothing happens
                        }
                        //If the hit button's item isn't armor, nothing happens
                    }

                    //If the hit button is a weapon slot and this button is an inventory slot
                    else if(this.buttonType == InventoryButtonType.Bag && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Weapon)
                    {
                        //If this button's item is a weapon
                        if(thisButtonItem.GetComponent<Weapon>())
                        {
                            //If this button's weapon is 1 handed
                            if(thisButtonItem.GetComponent<Weapon>().size == Weapon.WeaponSize.OneHand)
                            {
                                //If the hit button has a weapon equipped
                                if (hitButtonItem != null)
                                {
                                    //If the hit button's weapon was in the right hand
                                    if (hitButtonItem != null && hitButtonUI.selectedCharacterInventory.rightHand.gameObject == hitButtonItem.gameObject)
                                    {
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                    }
                                    //If the hit button's weapon was in the left hand
                                    else
                                    {
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, thisButtonItem.GetComponent<Weapon>());
                                    }

                                    //Finding the index of this button's item so it can be switched
                                    int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                    thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonItem);
                                }
                                //If the hit button is empty
                                else
                                {
                                    //If the hit button's inventory's right hand doesn't have a 2 handed weapon
                                    if (hitButtonUI.selectedCharacterInventory.rightHand == null || hitButtonUI.selectedCharacterInventory.rightHand.size == Weapon.WeaponSize.OneHand)
                                    {
                                        //Finding the hand slot that the weapon slot is in
                                        Inventory.WeaponHand weaponHandSlot = hitButtonUI.GetWeaponHandSlotFromImage(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                                        //Setting the hit button's inventory's weapon to this button's weapon
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(weaponHandSlot, thisButtonItem.GetComponent<Weapon>());

                                        //Finding the index of this button's item so it can be switched
                                        int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                        thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, null);
                                    }
                                    //If the hit button's inventory has a 2 handed weapon in the right hand slot
                                    else if(hitButtonUI.selectedCharacterInventory.rightHand != null && hitButtonUI.selectedCharacterInventory.rightHand.size == Weapon.WeaponSize.TwoHands)
                                    {
                                        //Finding the index of this button's item so it can be switched
                                        int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                        thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonUI.selectedCharacterInventory.rightHand.GetComponent<Item>());

                                        //Setting the hit button's inventory's left hand to this button's weapon and the right hand to empty
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, thisButtonItem.GetComponent<Weapon>());
                                        hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, null);
                                    }
                                }
                            }
                            //If this button's weapon is 2 handed
                            else
                            {
                                //If the hit button's inventory doesn't have weapons in either hand
                                if (hitButtonUI.selectedCharacterInventory.rightHand == null && hitButtonUI.selectedCharacterInventory.leftHand == null)
                                {
                                    //Setting the hit button's inventory's right hand weapon to this button's weapon
                                    hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());

                                    //Finding the index of this button's item so it can be set to empty
                                    int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                    thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, null);
                                }
                                //If the hit button's inventory has a 2 handed weapon in the right hand
                                else if (hitButtonUI.selectedCharacterInventory.rightHand.size == Weapon.WeaponSize.TwoHands)
                                {
                                    //Finding the index of this button's item so it can be switched
                                    int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                    thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonUI.selectedCharacterInventory.rightHand.GetComponent<Item>());

                                    //Setting the hit button's inventory's weapon to this button's weapon
                                    hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                }
                                //If the hit button's inventory has weapons equipped in both hands
                                else if (hitButtonUI.selectedCharacterInventory.rightHand != null && hitButtonUI.selectedCharacterInventory.leftHand != null)
                                {
                                    //If the hit button's inventory has a free slot for the left hand weapon
                                    if (hitButtonUI.selectedCharacterInventory.CheckForEmptySlot() > 0)
                                    {
                                        //If the hit button is the right hand
                                        if (hitButtonUI.GetWeaponHandSlotFromImage(results[0].gameObject.GetComponent<UnityEngine.UI.Image>()) == Inventory.WeaponHand.Right)
                                        {
                                            //Setting the hit button's inventory's weapon to this button's weapon
                                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());

                                            //Finding the index of this button's item so it can be switched
                                            int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                            thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonItem);

                                            //Adds the hit button's inventory's left hand weapon to its own inventory
                                            hitButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Left);
                                        }
                                        //If the hit button is the left hand
                                        else
                                        {
                                            //Adds the hit button's inventory's right hand weapon to its own inventory
                                            hitButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Right);

                                            //Setting the hit button's inventory's right hand weapon to this button's weapon
                                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                            //Setting the hit button's inventory's left hand weapon to be empty
                                            hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);

                                            //Finding the index of this button's item so it can be switched
                                            int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                            thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonItem);
                                        }
                                    }
                                    //If the hit button's inventory is full, nothing happens
                                }
                                //If the hit button's inventory has only 1 weapon equipped
                                else if (hitButtonUI.selectedCharacterInventory.rightHand != null || hitButtonUI.selectedCharacterInventory.leftHand != null)
                                {
                                    //If the hit button's inventory has a weapon equipped in the right hand
                                    if (hitButtonUI.selectedCharacterInventory.rightHand != null)
                                    {
                                        //Finding the index of this button's item so it can be switched
                                        int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                        thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonUI.selectedCharacterInventory.rightHand.GetComponent<Item>());
                                    }
                                    //If the hit button's inventory has a weapon equipped in the left hand
                                    else
                                    {
                                        //Finding the index of this button's item so it can be switched
                                        int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                                        thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonUI.selectedCharacterInventory.leftHand.GetComponent<Item>());
                                    }

                                    //Setting the hit button's inventory's right hand to this button's weapon
                                    hitButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, thisButtonItem.GetComponent<Weapon>());
                                }
                             }
                        }
                        //If this button's item isn't a weapon, nothing happens
                    }

                    //If the hit button is an inventory slot and this button is a weapon slot
                    else if(this.buttonType == InventoryButtonType.Weapon && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Bag)
                    {
                        //If the hit button's item is empty
                        if (hitButtonItem == null)
                        {
                            //If this button's weapon is equipped in the right hand
                            if(thisButtonUI.GetWeaponHandSlotFromImage(this.GetComponent<UnityEngine.UI.Image>()) == Inventory.WeaponHand.Right)
                            {
                                //Sets this button's right hand to empty
                                thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, null);
                            }
                            //If this button's weapon is equipped in the left hand
                            else
                            {
                                //Sets this button's left hand to empty
                                thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);
                            }

                            //Finding the index of the hit button's item so it can be switched
                            int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                            hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                        }
                        //If the hit button's item is a weapon
                        else
                        {
                            //If the hit button shares the same ID as this button's weapon
                            if (hitButtonItem.itemNameID == thisButtonItem.itemNameID)
                            {
                                //If this button's weapon can be stacked on the hit button's weapon
                                if (hitButtonItem.currentStackSize < hitButtonItem.maxStackSize)
                                {
                                    //Stack this button's weapon on the hit button's weapon
                                    hitButtonItem.currentStackSize += 1;
                                    thisButtonItem.transform.SetParent(hitButtonItem.transform);

                                    //If this button's weapon is equipped in the right hand
                                    if (thisButtonUI.GetWeaponHandSlotFromImage(this.GetComponent<UnityEngine.UI.Image>()) == Inventory.WeaponHand.Right)
                                    {
                                        //Sets this button's right hand to empty
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, null);
                                    }
                                    //If this button's weapon is equipped in the left hand
                                    else
                                    {
                                        //Sets this button's left hand to empty
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);
                                    }
                                }
                            }
                            //If the hit button's weapon is 1 handed
                            else if (hitButtonItem.GetComponent<Weapon>().size == Weapon.WeaponSize.OneHand)
                            {
                                //If this button's weapon is equipped in the right hand
                                if (thisButtonUI.GetWeaponHandSlotFromImage(this.GetComponent<UnityEngine.UI.Image>()) == Inventory.WeaponHand.Right)
                                {
                                    //Sets this button's right hand to hold the hit button's weapon
                                    thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                }
                                //If this button's weapon is equipped in the left hand
                                else
                                {
                                    //Sets this button's left hand to hold the hit button's weapon
                                    thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, hitButtonItem.GetComponent<Weapon>());
                                }

                                //Finding the index of the hit button's item so it can be switched
                                int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                                hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                            }
                            //If the hit button's weapon is 2 handed
                            else if(hitButtonItem.GetComponent<Weapon>().size == Weapon.WeaponSize.TwoHands)
                            {
                                //If this button's inventory has weapons in the left and right hands
                                if (thisButtonUI.selectedCharacterInventory.rightHand != null && thisButtonUI.selectedCharacterInventory.leftHand != null)
                                {
                                    //If this button's inventory has a free slot for the left hand weapon
                                    if (thisButtonUI.selectedCharacterInventory.CheckForEmptySlot() > 0)
                                    {
                                        //Adds this button's inventory's left hand weapon to its own inventory
                                        thisButtonUI.selectedCharacterInventory.UnequipWeapon(Inventory.WeaponHand.Left);

                                        //Sets this button's right hand to hold the hit button's weapon
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());

                                        //Finding the index of the hit button's item so it can be switched
                                        int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                                        hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                                    }
                                }
                                //If this button's inventory has weapons in only one hand
                                else
                                {
                                    //Sets this button's inventory's right hand weapon to the hit button's weapon
                                    //Sets the hit button's slot to this button's inventory's equipped weapon
                                    //If this button's weapon is equipped in the right hand
                                    if (thisButtonUI.GetWeaponHandSlotFromImage(this.GetComponent<UnityEngine.UI.Image>()) == Inventory.WeaponHand.Right)
                                    {
                                        //Sets this button's right hand to hold the hit button's weapon
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                    }
                                    //If this button's weapon is equipped in the left hand
                                    else
                                    {
                                        //Sets this button's right hand to hold the hit button's weapon and empties the left
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Right, hitButtonItem.GetComponent<Weapon>());
                                        thisButtonUI.selectedCharacterInventory.ChangeWeaponItem(Inventory.WeaponHand.Left, null);
                                    }

                                    //Finding the index of the hit button's item so it can be switched
                                    int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                                    hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                                }
                            }
                        }
                        //If the hit button's item isn't a weapon, nothing happens
                    }


                    //Updating both of the UIs for the inventories
                    thisButtonUI.UpdateImages();
                    hitButtonUI.UpdateImages();
                    //Updating both of the inventory weights
                    thisButtonUI.selectedCharacterInventory.FindArmorStats();
                    hitButtonUI.selectedCharacterInventory.FindArmorStats();
                }
            }
        }

        this.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
    }


    //Function called every frame
    private void Update()
    {
        //Does nothing if not being dragged
        if(!this.isBeingDragged)
        {
            return;
        }

        //Moves this item to the mouse's position on the screen
        InventoryOpener.globalReference.dragIconImage.transform.position = Input.mousePosition;
    }


    //Function called from OnPointerDown to use the item that was right clicked
    private void UseItem()
    {
        //Getting references to the inventory UI of this button
        CharacterInventoryUI thisButtonUI = this.GetComponentInParent<CharacterInventoryUI>();

        //If this is an inventory button
        if(this.buttonType == InventoryButtonType.Bag)
        {
            Item thisButtonItem = thisButtonUI.GetItemFromInventoryButton(this.GetComponent<Image>());

            //If the clicked button is a quest item in the party inventory
            if(thisButtonItem.GetComponent<QuestGiver>() && thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Party)
            {
                //Getting the quest from the item
                Quest itemQuest = thisButtonItem.GetComponent<QuestGiver>().questToGive;

                //If the quest that is given isn't in our quest log
                if(!QuestTracker.globalReference.IsQuestInQuestLog(itemQuest))
                {
                    //If the quest that is given isn't in the list of completed quests
                    if(!QuestTracker.globalReference.completedQuestNames.Contains(itemQuest.questName))
                    {
                        //We create a new event to dispatch the quest info
                        EVTData data = new EVTData();
                        data.promptQuest = new PromptQuestEVT(itemQuest);

                        //Dispatching the quest prompt
                        EventManager.TriggerEvent(PromptQuestEVT.eventName, data);

                        //Returning so nothing else happens with the item until the quest is accepted
                        return;
                    }
                }
            }

            //If the clicked button is food
            if(thisButtonItem.GetComponent<Food>())
            {
                //If this button's inventory is on the Party character, they can eat it
                if(thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Party)
                {
                    //If this character's food isn't full (no sense eating when you're full. Always good to remember)
                    if (thisButtonUI.selectedCharacterInventory.GetComponent<PhysicalState>().currentFood < thisButtonUI.selectedCharacterInventory.GetComponent<PhysicalState>().maxFood)
                    {
                        //Tells this character to eat this food
                        thisButtonUI.selectedCharacterInventory.GetComponent<PhysicalState>().EatFood(thisButtonItem.GetComponent<Food>());

                        //If this food has a stack higher than 1, one of the children is eaten, much like Chronos did
                        if (thisButtonItem.currentStackSize > 1)
                        {
                            thisButtonItem.currentStackSize -= 1;
                            //Finding the child of this button's item and destroying the first instance
                            Transform childItem = thisButtonItem.transform.FindChild(thisButtonItem.name);
                            if (childItem != null)
                            {
                                Destroy(childItem.gameObject);
                            }
                            //If for some reason there's no child, I think something's gone wrong....
                        }
                        //Otherwise, this item is completely eaten and set to null
                        else
                        {
                            //Finding the index of this button's item in the inventory
                            int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                            //Destroying this item's object
                            Destroy(thisButtonItem.gameObject);
                            //Setting this inventory's item slot to be empty
                            thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisItemsIndex, null);
                        }
                    }
                }
                //If this button's inventory is on a trade character or inventory bag/chest, it can be added to the party character's inventory
                else
                {
                    //If this inventory is the bag/chest OR if it's the trade character and the party character isn't the same
                    if (thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Bag ||
                        thisButtonUI.selectedCharacterInventory != CharacterInventoryUI.partyInventory.selectedCharacterInventory)
                    {
                        //Making sure the party inventory UI screen is showing. Can't add it to the party inventory if we don't know who to send it to
                        if (InventoryOpener.globalReference.partyInventoryUIObject.activeSelf)
                        {
                            //Getting a quick reference to the hit button's Character Inventory UI component
                            CharacterInventoryUI hitButtonUI = InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>();

                            //If true, all items in this stack were successfully added to the party inventory and we can empty this button
                            if (hitButtonUI.selectedCharacterInventory.AddItemToInventory(thisButtonItem))
                            {
                                //Finding the index of this button's item in the inventory and setting the slot to be empty
                                int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                                thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisItemsIndex, null);
                            }
                            //Otherwise, not all of the stack could fit, so we can't set this button to empty

                            //Telling the party inventory UI screen to update so it shows the items that were added
                            InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>().UpdateImages();
                        }
                    }
                }
            }
            //If the clicked button is a weapon
            else if (thisButtonItem.GetComponent<Weapon>())
            {
                //If this button's inventory is on a character in the player party
                if (thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Party)
                {
                    //Finding the index of this button's item in the inventory
                    int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                    //Equipping this piece of armor
                    thisButtonUI.selectedCharacterInventory.EquipWeapon(thisItemsIndex);
                }
                //If this button's inventory is on a trade character or inventory bag/chest, it can be added to the party character's inventory
                else
                {
                    //If this inventory is the bag/chest OR if it's the trade character and the party character isn't the same
                    if (thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Bag ||
                        thisButtonUI.selectedCharacterInventory != CharacterInventoryUI.partyInventory.selectedCharacterInventory)
                    {
                        //Making sure the party inventory UI screen is showing. Can't add it to the party inventory if we don't know who to send it to
                        if (InventoryOpener.globalReference.partyInventoryUIObject.activeSelf)
                        {
                            //Getting a quick reference to the hit button's Character Inventory UI component
                            CharacterInventoryUI hitButtonUI = InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>();

                            //If true, all items in this stack were successfully added to the party inventory and we can empty this button
                            if (hitButtonUI.selectedCharacterInventory.AddItemToInventory(thisButtonItem))
                            {
                                //Finding the index of this button's item in the inventory and setting the slot to be empty
                                int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                                thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisItemsIndex, null);
                            }
                            //Otherwise, not all of the stack could fit, so we can't set this button to empty

                            //Telling the party inventory UI screen to update so it shows the items that were added
                            InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>().UpdateImages();
                        }
                    }
                }
            }
            //If the clicked button is armor
            else if(thisButtonItem.GetComponent<Armor>())
            {
                //If this button's inventory is on a character in the player party, it can be equipped
                if(thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Party)
                {
                    //Finding the index of this button's item in the inventory
                    int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                    //Equipping this piece of armor
                    thisButtonUI.selectedCharacterInventory.EquipArmor(thisItemsIndex);
                }
                //If this button's inventory is on a trade character or inventory bag/chest, it can be added to the party character's inventory
                else
                {
                    //If this inventory is the bag/chest OR if it's the trade character and the party character isn't the same
                    if (thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Bag ||
                        thisButtonUI.selectedCharacterInventory != CharacterInventoryUI.partyInventory.selectedCharacterInventory)
                    {
                        //Making sure the party inventory UI screen is showing. Can't add it to the party inventory if we don't know who to send it to
                        if (InventoryOpener.globalReference.partyInventoryUIObject.activeSelf)
                        {
                            //Getting a quick reference to the hit button's Character Inventory UI component
                            CharacterInventoryUI hitButtonUI = InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>();

                            //If true, all items in this stack were successfully added to the party inventory and we can empty this button
                            if (hitButtonUI.selectedCharacterInventory.AddItemToInventory(thisButtonItem))
                            {
                                //Finding the index of this button's item in the inventory and setting the slot to be empty
                                int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                                thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisItemsIndex, null);
                            }
                            //Otherwise, not all of the stack could fit, so we can't set this button to empty

                            //Telling the party inventory UI screen to update so it shows the items that were added
                            InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>().UpdateImages();
                        }
                    }
                }
            }
            //If the clicked button is a skill tome
            else if(thisButtonItem.GetComponent<SkillTome>())
            {
                //If this button's inventory is on the Party character, they can use it
                if (thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Party)
                {
                    //Tells this character to use this tome
                    thisButtonItem.GetComponent<SkillTome>().CharacterUseTome(thisButtonUI.selectedCharacterInventory.GetComponent<Character>());

                    //If this tome has a stack higher than 1, one of the children is eaten. Tasty, tasty white meat
                    if (thisButtonItem.currentStackSize > 1)
                    {
                        thisButtonItem.currentStackSize -= 1;
                        //Finding the child of this button's item and destroying the first instance
                        Transform childItem = thisButtonItem.transform.FindChild(thisButtonItem.name);
                        if (childItem != null)
                        {
                            Destroy(childItem.gameObject);
                        }
                        //If for some reason there's no child, I think something's gone wrong....
                    }
                    //Otherwise, this item is completely eaten and set to null
                    else
                    {
                        //Finding the index of this button's item in the inventory
                        int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                        //Destroying this item's object
                        Destroy(thisButtonItem.gameObject);
                        //Setting this inventory's item slot to be empty
                        thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisItemsIndex, null);
                    }
                }
                //If this button's inventory is on a trade character or inventory bag/chest, it can be added to the party character's inventory
                else
                {
                    //If this inventory is the bag/chest OR if it's the trade character and the party character isn't the same
                    if (thisButtonUI.inventoryUIType == CharacterInventoryUI.InventoryType.Bag ||
                        thisButtonUI.selectedCharacterInventory != CharacterInventoryUI.partyInventory.selectedCharacterInventory)
                    {
                        //Making sure the party inventory UI screen is showing. Can't add it to the party inventory if we don't know who to send it to
                        if (InventoryOpener.globalReference.partyInventoryUIObject.activeSelf)
                        {
                            //Getting a quick reference to the hit button's Character Inventory UI component
                            CharacterInventoryUI hitButtonUI = InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>();

                            //If true, all items in this stack were successfully added to the party inventory and we can empty this button
                            if (hitButtonUI.selectedCharacterInventory.AddItemToInventory(thisButtonItem))
                            {
                                //Finding the index of this button's item in the inventory and setting the slot to be empty
                                int thisItemsIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<Image>());
                                thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisItemsIndex, null);
                            }
                            //Otherwise, not all of the stack could fit, so we can't set this button to empty

                            //Telling the party inventory UI screen to update so it shows the items that were added
                            InventoryOpener.globalReference.partyInventoryUIObject.GetComponent<CharacterInventoryUI>().UpdateImages();
                        }
                    }
                }
            }
        }
        //If this is an armor button or weapon button
        else if(this.buttonType == InventoryButtonType.Armor || this.buttonType == InventoryButtonType.Weapon)
        {
            //If this button's inventory is a party character's, we can add it to their inventory
            if (CharacterManager.globalReference.playerParty.Contains(thisButtonUI.selectedCharacterInventory.GetComponent<Character>()))
            {
                //If there's an empty slot in our inventory
                if (thisButtonUI.selectedCharacterInventory.CheckForEmptySlot() > 0)
                {
                    //If this item is armor
                    if (this.buttonType == InventoryButtonType.Armor)
                    {
                        //Finding the slot to unequip
                        Armor.ArmorSlot slotToUnequip = thisButtonUI.GetArmorSlotFromImage(this.GetComponent<Image>());
                        thisButtonUI.selectedCharacterInventory.UnequipArmor(slotToUnequip);
                    }
                    //If this is a weapon
                    else
                    {
                        //Finding the weapon hand to unequip
                        Inventory.WeaponHand handToUnequip = thisButtonUI.GetWeaponHandSlotFromImage(this.GetComponent<Image>());
                        thisButtonUI.selectedCharacterInventory.UnequipWeapon(handToUnequip);
                    }
                }
            }
            //If this button's inventory is NOT a party character's, it's left alone. No stealing from others
        }

        //Updating the UI for the inventory
        thisButtonUI.UpdateImages();
        //Updating the inventory weight
        thisButtonUI.selectedCharacterInventory.FindArmorStats();
    }


    //Function called from CharacterInventoryUI.cs. If the image given isn't null, it's set as this button's image
    public void SetButtonIcon(Sprite buttonIcon_)
    {
        //If no icon was given, this button's image is set to the empty slot image
        if(buttonIcon_ == null)
        {
            this.GetComponent<Image>().sprite = this.emptySlotImage;
        }
        //Otherwise, this button's image is set to the image given
        else
        {
            this.GetComponent<Image>().sprite = buttonIcon_;
        }

        //If the stack size text box isn't null, we can display the count of items in this stack
        if(this.stackSizeText != null)
        {
            //Getting references to the inventory UIs of this button and the hit button
            CharacterInventoryUI thisButtonUI = this.GetComponentInParent<CharacterInventoryUI>();
            //Getting the items that each button shows from their given inventories
            Item thisButtonItem = thisButtonUI.GetItemFromInventoryButton(this.GetComponent<Image>());

            //If this button's slot is empty, we make sure the text is empty
            if(thisButtonItem == null)
            {
                this.stackSizeText.text = "";
            }
            //If this button's item has more than 1 item in its stack, we show how many there is
            else if(thisButtonItem.currentStackSize > 1)
            {
                this.stackSizeText.text = "" + thisButtonItem.currentStackSize;
            }
            //If this button's item doesn't have any others in its stack, we don't show the count
            else
            {
                this.stackSizeText.text = "";
            }
        }
    }
}
