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
                //Float to hold the multiplier for this item's value to this vendor
                .//Need to set this to the vendor's base item value
                float valueMultiplier = 0f;

                //If the item is food, we multiply it
                if (buttonItem_.GetType() == typeof(Food))
                {
                    .//Need to get a reference to the selected vendor to find their value multipliers
                }

                //If the item is a weapon
                if(buttonItem_.GetType() == typeof(Weapon))
                {

                }

                //If the item is armor
                if(buttonItem_.GetType() == typeof(Armor))
                {

                }
            }
        }
    }
}
