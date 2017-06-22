using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionPanelUI : MonoBehaviour
{
    //Static reference to this UI script so the Combat Manager can tell it what to do
    public static CombatActionPanelUI globalReference;

    //The background panel that has its color changed
    public UnityEngine.UI.Image backgroundImage;

    //The colors that the background image is turned to based on what action type is displayed
    public Color standardColor;
    public Color secondaryColor;
    public Color quickColor;
    public Color fullRoundColor;

    //The list of action buttons that 
    public List<ActionButton> actionButtons;



    //Function called when this object is created
    private void Awake()
    {
        //Setting this component as the global reference if there isn't already one
        if(globalReference == null)
        {
            globalReference = this;
        }
        //If there's already a global reference, this is destroyed
        else
        {
            Destroy(this);
        }
    }


    //Function called externally to display all of a certain type of action
    public void DisplayActionTypes(int typeIndex_)
    {
        //Making sure there's an acting character first
        if(CombatManager.globalReference.actingCharacters.Count <= 0)
        {
            return;
        }

        //Getting a reference to the character's action list
        ActionList actingCharActions = CombatManager.globalReference.actingCharacters[0].charActionList;

        switch (typeIndex_)
        {
            case 0:
                this.UpdateActionButtons(actingCharActions.standardActions);
                this.backgroundImage.color = this.standardColor;
                break;
            case 1:
                this.UpdateActionButtons(actingCharActions.secondaryActions);
                this.backgroundImage.color = this.secondaryColor;
                break;
            case 2:
                this.UpdateActionButtons(actingCharActions.quickActions);
                this.backgroundImage.color = this.quickColor;
                break;
            case 3:
                this.UpdateActionButtons(actingCharActions.fullRoundActions);
                this.backgroundImage.color = this.fullRoundColor;
                break;
            default:
                this.UpdateActionButtons(actingCharActions.standardActions);
                this.backgroundImage.color = this.standardColor;
                break;
        }
    }


    //Function called from DisplayActionTypes to loop through our action buttons and display the actions given
    private void UpdateActionButtons(List<Action> actionsToShow_)
    {
        //Looping through all of the action buttons in our list
        for(int b = 0; b < this.actionButtons.Count; ++b)
        {
            //If there are still actions to display, we display them
            if(b < actionsToShow_.Count)
            {
                this.actionButtons[b].nameText.text = actionsToShow_[b].actionName;
                this.actionButtons[b].descriptionText.text = actionsToShow_[b].actionDescription;
                this.actionButtons[b].buttonComponent.enabled = true;
            }
            //If there are no more actions to display, the rest of the buttons are disabled
            else
            {
                this.actionButtons[b].nameText.text = "";
                this.actionButtons[b].descriptionText.text = "";
                this.actionButtons[b].buttonComponent.enabled = false;
            }
        }
    }
}

[System.Serializable]
public class ActionButton
{
    //The name text for the action
    public UnityEngine.UI.Text nameText;
    //The description for the action
    public UnityEngine.UI.Text descriptionText;
    //The button component that can be enabled/disabled
    public UnityEngine.UI.Button buttonComponent;
}