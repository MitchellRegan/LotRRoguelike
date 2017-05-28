using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Bool that tracks when the player is dragging this inventory item
    private bool isBeingDragged = false;
    //The UI position of this button when not being dragged
    private Vector3 defaultPosition;

    //Enum that determines what inventory slot this button represents
    public enum InventoryButtonType { Bag, Armor, Weapon};
    public InventoryButtonType buttonType = InventoryButtonType.Bag;



    //Function called in the first frame
    private void Start()
    {
        this.defaultPosition = this.transform.position;
    }


    //Function called when the player's mouse clicks down on this inventory item
    public void OnPointerDown(PointerEventData eventData_)
    {
        //If the player left clicks to drag
        if(eventData_.button == PointerEventData.InputButton.Left)
        {
            //Starts dragging this item and sets it as the front UI element
            this.isBeingDragged = true;
            this.GetComponent<RectTransform>().SetAsLastSibling();
        }
        //If the player right clicks to use/equip
        else if(eventData_.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right click");
        }
    }


    //Function called when the player's mouse releases
    public void OnPointerUp(PointerEventData eventData_)
    {
        //Ends dragging this item and resets back to the default position
        this.isBeingDragged = false;
        this.transform.position = this.defaultPosition;

        //Turning off this button's raycast blocking so that we can see what's behind it
        this.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

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

                    //If neither item is equipped and are just in the regular inventory
                        //If the hit button is empty
                            //Do changeInventoryAtIndex
                        //If the items have the same nameID
                            //If the hit button's item stack is full
                                //Do changeInventoryAtIndex
                            //If the hit button's item stack isn't full
                                //Move as many stacks from this stack to the hit button's stack as possible
                        //If the items have different nameIDs
                            //Do changeInventoryAtIndex

                    //If both items are in armor slots
                        //If the hit button is empty
                            //Swap the references in the inventories
                        //If they're the same type of armor
                            //Swap the references in the inventories
                        //If they're different types of armor
                            //Do nothing

                    //If both items are in weapon slots
                        //If the hit button is empty
                            //Swap the references in the inventories
                        //If they're the same type of weapon
                            //Swap the references in the inventories
                        //If one weapon is 2 handed and the other isn't
                            //If the inventory with the 1 handed weapon has a weapon in both hands
                                //Make sure there's a free space in the 1 handed weapon's inventory
                                    //If there's a free space
                                        //Unequip the other hand's weapon and swap the other weapon references
                                    //If the inventory is full
                                        //Do Nothing
                            //Otherwise
                                //Swap the references in the inventories

                    //If one is armor and the other is weapon
                        //Nothing happens
                    
                    //If the hit button is an armor slot and this button is an inventory slot
                        //If this button's item isn't armor
                            //Do nothing
                        //If this button's item is armor
                            //If the hit button has armor equipped
                                //If both items are the same type of armor
                                    //Swap the references in the inventories
                                //If both items are different types of armor
                                    //Do nothing
                            //If the hit button doesn't have armor equipped
                                //Find out what type of armor goes into the inventory slot
                                //If this button's armor is the same type
                                    //Swap the references in the inventories

                    //If the hit button is an inventory slot and this button is an armor slot
                        //If the hit button is empty
                            //Swap the references in the inventories
                        //If the hit button's item is armor
                            //If the hit button's item has the same nameID as this button's armor
                                //If the hit button's item has room for one more stack
                                    //Stack this button's armor onto the hit button's
                                    //Null this button's slot
                                //If the hit button can't stack
                                    //Do nothing
                            //If the hit button's armor is the same type as this button's armor
                                //Swap the references in the inventories
                            //If the hit button's armor isn't the same type
                                //Do nothing
                        //If the hit button's item isn't armor
                            //Do nothing

                    //If the hit button is a weapon slot and this button is an inventory slot
                        //If this button's item isn't a weapon
                            //Do nothing
                        //If this button's item is a weapon
                            //If this button's weapon is 1 handed
                                //If the hit button's has a weapon equipped
                                    //Swap the references in the inventories
                                //If the hit button is empty
                                    //If the hit button's inventory's right hand doesn't have a 2 handed weapon
                                        //Swap the references in the inventories
                                    //If the hit button's inventory has a 2 handed weapon in the right hand slot
                                        //Swap the references in the inventories (Remember to use the Right Hand Slot)
                            //If this button's weapon is 2 handed
                                //If the hit button's inventory doesn't have weapons in either hand
                                    //Set this button's item to nothing
                                    //Sets the hit button's inventory's right hand to this button's weapon
                                //If the hit button's inventory has a 2 handed weapon in the right hand
                                    //Swap the references in the inventories (Remember to use the Right Hand Slot)
                                //If the hit button's inventory has only 1 weapon equipped
                                    //Sets this button's item to the other button's inventory's hand's weapon
                                    //Sets the hit button's inventory's right hand to this button's weapon
                                    //Sets the hit button's inventory's left hand to nothing
                                //If the hit button's inventory has weapons equipped in both hands
                                    //If the hit button's inventory has a free slot for the left hand weapon
                                        //If the hit button is the right hand
                                            //Swaps the references in the inventories
                                            //Adds the hit button's inventory's left hand weapon to its own inventory
                                        //If the hit button is the left hand
                                            //Swaps the references in the inventories
                                            //Adds the hit button's inventory's right hand weapon to its own inventory
                                    //If the hit button's inventory is full 
                                        //Do nothing

                    //If the hit button is an inventory slot and this button is a weapon slot
                        //If the hit button's item is empty
                            //Swap the references in the inventories
                        //If the hit button's item isn't a weapon
                            //Do nothing
                        //If the hit button's item is a weapon
                            //If the hit button's weapon is 1 handed
                                //Swap the references in the inventories
                            //If the hit button's weapon is 2 handed
                                //If this button's inventory has weapons in the left and right hands
                                    //If this button's inventory has a free slot for the left hand weapon
                                        //Swaps the references in the inventories
                                        //Adds this button's inventory's left hand weapon to its own inventory
                                //If this button's inventory has weapons in only one hand
                                    //Sets this button's inventory's right hand weapon to the hit button's weapon
                                    //Sets the hit button's slot to this button's inventory's equipped weapon
                                    

                    /*//If both items are pieces of armor
                    if(thisButtonItem.GetComponent<Armor>() && hitButtonItem.GetComponent<Armor>())
                    {
                        //If both items are for the same armor slot
                        if(thisButtonItem.GetComponent<Armor>().slot == hitButtonItem.GetComponent<Armor>().slot)
                        {
                            //If both items are equipped, they're swapped
                            if(this.buttonType == InventoryButtonType.Armor && results[0].gameObject.GetComponent<InventoryButton>().buttonType == InventoryButtonType.Armor)
                            {
                                thisButtonUI.selectedCharacterInventory.ChangeItem(thisButtonItem, hitButtonItem);
                                hitButtonUI.selectedCharacterInventory.ChangeItem(hitButtonItem, thisButtonItem);
                            }
                        }
                        //If they aren't for the same slot, they're just added to the regular inventory
                        else
                        {
                            //Checking to make sure both 
                        }
                    }
                    //If both items are weapons
                    else if(thisButtonItem.GetComponent<Weapon>() && hitButtonItem.GetComponent<Weapon>())
                    {

                    }
                    //Otherwise neither item can be equipped, so both are just added to each other's inventory
                    else
                    {
                        //Finding the index of this button's item so it can be switched
                        int thisButtonIndex = thisButtonUI.slotImages.IndexOf(this.GetComponent<UnityEngine.UI.Image>());
                        //Replacing this button's inventory with the hit button's item
                        thisButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(thisButtonIndex, hitButtonItem);

                        //Finding the index of the hit button's item so it can be switched
                        int hitButtonIndex = hitButtonUI.slotImages.IndexOf(results[0].gameObject.GetComponent<UnityEngine.UI.Image>());
                        //Replacing the hit button's inventory with this button's item
                        hitButtonUI.selectedCharacterInventory.ChangeInventoryItemAtIndex(hitButtonIndex, thisButtonItem);
                    }*/

                    //Updating both of the UIs for the inventories
                    thisButtonUI.UpdateImages();
                    hitButtonUI.UpdateImages();
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
        this.transform.position = Input.mousePosition;
    }
}
