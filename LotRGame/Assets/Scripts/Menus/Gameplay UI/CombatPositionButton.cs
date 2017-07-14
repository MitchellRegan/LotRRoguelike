using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CombatPositionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Image that's used when this button doesn't have a character on it
    public Sprite emptySlotImage;

    //The column and row that this button represents
    public int col = 0;
    public int row = 0;

    //The index for this character in the character manager
    [HideInInspector]
    public Character characterInThisPosition = null;

    //Reference to this button's Image component
    private Image buttonImage;

    //Bool that tracks when the player is dragging this character icon
    private bool isBeingDragged = false;
    //The UI position of this button when not being dragged
    private Vector3 defaultPosition;



    //Function called when this object is created
    private void Awake()
    {
        //Setting the reference for this button's image component
        this.buttonImage = this.GetComponent<Image>();

        //Refreshes this image to determine if there's a character in this position
        this.RefreshButton();
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        //Sets this button's default position
        this.defaultPosition = this.transform.position;

        //Refreshes this image to determine if there's a character in this position
        this.RefreshButton();
    }


    //Finds out if a character in the selected party group in this tile's position and updates our image
    public void RefreshButton()
    {
        //Looping through each character in the selected party group to check their starting positions
        foreach (Character currentChar in CharacterManager.globalReference.selectedGroup.charactersInParty)
        {
            //Making sure the current party position isn't empty
            if (currentChar != null)
            {
                //If the character we're currently checking has the same row and column position that this button represents
                if (currentChar.charCombatStats.startingPositionCol == this.col && currentChar.charCombatStats.startingPositionRow == this.row)
                {
                    //We save this character reference as the character on this button
                    this.SetCharacterInThisPosition(currentChar);
                }
            }
        }

        //If there's a character in this button's position
        if (this.characterInThisPosition != null)
        {
            //Enabling this button's image
            //NOTE: Once there are character sprites, this is where we change this button's image to the character's face
            this.buttonImage.color = Color.red;
        }
        //If there is no character at this button's index
        else
        {
            //Disabling this button's image
            this.buttonImage.color = Color.white;
        }
    }


    //Function called when the player's mouse clicks down on this button
    public void OnPointerDown(PointerEventData eventData_)
    {
        //If this button is empty, nothing happens
        if (this.characterInThisPosition == null)
        {
            return;
        }

        //If the player left clicks to drag
        if (eventData_.button == PointerEventData.InputButton.Left)
        {
            this.defaultPosition = this.transform.position;
            //Starts dragging this item and sets it as the front UI element
            this.isBeingDragged = true;
            this.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }


    //Function called when the player's mouse releases
    public void OnPointerUp(PointerEventData eventData_)
    {
        //If this button is empty, nothing happens
        if (this.characterInThisPosition == null)
        {
            return;
        }
        
        //Ends dragging this button and resets back to the default position
        this.isBeingDragged = false;
        this.transform.position = this.defaultPosition;

        //Turning off this button's raycast blocking so that we can see what's behind it
        this.GetComponent<Image>().raycastTarget = false;

        //Casting a ray from the pointer to hit all targets under it
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData_, results);

        //If the raycast hits something, we check the first result
        if (results.Count > 0)
        {
            //If the first result has a CombatPositionButton.cs component
            if (results[0].gameObject.GetComponent<CombatPositionButton>())
            {
                //Getting the reference to the other combat position button
                CombatPositionButton otherButton = results[0].gameObject.GetComponent<CombatPositionButton>();


                //Saving the character reference on the other button
                Character otherButtonCharacter = otherButton.characterInThisPosition;

                //Swapping the characters on these buttons
                otherButton.SetCharacterInThisPosition(this.characterInThisPosition);

                this.SetCharacterInThisPosition(otherButtonCharacter);
            }
        }

        //Turning this button's raycast blocking on so that we can click it again
        this.GetComponent<Image>().raycastTarget = true;
    }


    //Function called from other CombatPositionButton.cs components to set what characters are in this position
    public void SetCharacterInThisPosition(Character newCharacter_ = null)
    {
        //Setting the character reference in this position
        this.characterInThisPosition = newCharacter_;

        //If we the given a character isn't null, we update the character's row/col position
        if(newCharacter_ != null)
        {
            newCharacter_.charCombatStats.startingPositionCol = this.col;
            newCharacter_.charCombatStats.startingPositionRow = this.row;

            this.buttonImage.color = Color.red;
        }
        else
        {
            this.buttonImage.color = Color.white;
        }
    }


    //Function called every frame
    private void Update()
    {
        //Does nothing if not being dragged
        if (!this.isBeingDragged)
        {
            return;
        }

        //Moves this button to the mouse's position on the screen
        this.transform.position = Input.mousePosition;
    }
}
