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

                    //Check if both items are armor/weapons of the same type. Also check if they're equipped
                    //Need to make sure both have enough inventory space

                    //If both items are pieces of armor
                    if(thisButtonItem.GetComponent<Armor>() && hitButtonItem.GetComponent<Armor>())
                    {
                        //If both items are for the same armor slot
                        if(thisButtonItem.GetComponent<Armor>().slot == hitButtonItem.GetComponent<Armor>().slot)
                        {
                            //If both items are equipped, they're swapped
                            if(thisButtonUI.selectedCharacterInventory.IsArmorEquipped(thisButtonItem.GetComponent<Armor>()) &&
                                hitButtonUI.selectedCharacterInventory.IsArmorEquipped(hitButtonItem.GetComponent<Armor>()))
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
                    }

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
