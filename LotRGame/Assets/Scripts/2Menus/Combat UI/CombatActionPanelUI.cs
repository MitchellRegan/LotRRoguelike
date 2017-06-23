using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatActionPanelUI : MonoBehaviour
{
    //Static reference to this UI script so the Combat Manager can tell it what to do
    public static CombatActionPanelUI globalReference;

    //The background panel that has its color changed
    public Image backgroundImage;

    //The colors that the background image is turned to based on what action type is displayed
    public Color standardColor;
    public Color secondaryColor;
    public Color quickColor;
    public Color fullRoundColor;

    //The current type of action that we're displaying
    public Action.ActionType actionTypeShown = Action.ActionType.Standard;

    //The list of action buttons that 
    public List<ActionButton> actionButtons;

    //The action that's currently selected
    [HideInInspector]
    public Action selectedAction;
    //The panel that displays the selected action's details
    public SelectedActionPanel selectedPanelDetails;



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
                this.actionTypeShown = Action.ActionType.Standard;
                break;
            case 1:
                this.UpdateActionButtons(actingCharActions.secondaryActions);
                this.backgroundImage.color = this.secondaryColor;
                this.actionTypeShown = Action.ActionType.Secondary;
                break;
            case 2:
                this.UpdateActionButtons(actingCharActions.quickActions);
                this.backgroundImage.color = this.quickColor;
                this.actionTypeShown = Action.ActionType.Quick;
                break;
            case 3:
                this.UpdateActionButtons(actingCharActions.fullRoundActions);
                this.backgroundImage.color = this.fullRoundColor;
                this.actionTypeShown = Action.ActionType.FullRound;
                break;
            default:
                this.UpdateActionButtons(actingCharActions.standardActions);
                this.backgroundImage.color = this.standardColor;
                this.actionTypeShown = Action.ActionType.Standard;
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
                this.actionButtons[b].buttonComponent.interactable = true;
            }
            //If there are no more actions to display, the rest of the buttons are disabled
            else
            {
                this.actionButtons[b].nameText.text = "";
                this.actionButtons[b].descriptionText.text = "";
                this.actionButtons[b].buttonComponent.interactable = false;
            }
        }
    }


    //Function called externally from the buttons on the action panel. Tells the combat manager to hilight an action's range
    public void SelectActionAtIndex(int actionIndex_)
    {
        //Clearing all tile highlights before we highlight different ones
        CombatManager.globalReference.ClearCombatTileHighlights();

        //Getting a reference to the character that's currently acting
        Character actingCharacter = CombatManager.globalReference.actingCharacters[0];
        //Finding out which tile the acting character is on
        CombatTile actingCharsTile = CombatManager.globalReference.FindCharactersTile(actingCharacter);
        //Finding the range of the action based on the button hit
        int actionRange = 0;
        switch(this.actionTypeShown)
        {
            case Action.ActionType.Standard:
                actionRange = actingCharacter.charActionList.standardActions[actionIndex_].range;
                this.selectedAction = actingCharacter.charActionList.standardActions[actionIndex_];
                break;
            case Action.ActionType.Secondary:
                actionRange = actingCharacter.charActionList.secondaryActions[actionIndex_].range;
                this.selectedAction = actingCharacter.charActionList.secondaryActions[actionIndex_];
                break;
            case Action.ActionType.Quick:
                actionRange = actingCharacter.charActionList.quickActions[actionIndex_].range;
                this.selectedAction = actingCharacter.charActionList.quickActions[actionIndex_];
                break;
            case Action.ActionType.FullRound:
                actionRange = actingCharacter.charActionList.fullRoundActions[actionIndex_].range;
                this.selectedAction = actingCharacter.charActionList.fullRoundActions[actionIndex_];
                break;
        }

        //Finding out which tiles need to be hilighted
        List<CombatTile> tilesToHilight = PathfindingAlgorithms.FindTilesInActionRange(actingCharsTile, actionRange);

        //Looping through all tiles in range and hilighting them
        foreach(CombatTile tile in tilesToHilight)
        {
            tile.inAttackRange = true;
            tile.HighlightTile(true);
        }
    }


    //Function called externally to display the selected action's details
    public void UpdateActionDetailsPanel()
    {
        //If there is no selected action, all of the text panels are set to empty
        if(this.selectedAction == null)
        {
            this.selectedPanelDetails.nameText.text = "";
            this.selectedPanelDetails.descriptionText.text = "";
            this.selectedPanelDetails.rangeText.text = "";

            this.selectedPanelDetails.critText.text = "";
            this.selectedPanelDetails.multiplierText.text = "";

            this.selectedPanelDetails.physDamageText.text = "";
            this.selectedPanelDetails.magicDamageText.text = "";
            this.selectedPanelDetails.fireDamageText.text = "";
            this.selectedPanelDetails.waterDamageText.text = "";
            this.selectedPanelDetails.electridDamageText.text = "";
            this.selectedPanelDetails.windDamageText.text = "";
            this.selectedPanelDetails.rockDamageText.text = "";
            this.selectedPanelDetails.lightDamageText.text = "";
            this.selectedPanelDetails.darkDamageText.text = "";

            this.selectedPanelDetails.effectNameText.text = "";
        }
        //If there is a selected action, the text panels display the details of what it does
        else
        {
            this.selectedPanelDetails.nameText.text = this.selectedAction.actionName;
            this.selectedPanelDetails.descriptionText.text = this.selectedAction.actionDescription;
            this.selectedPanelDetails.rangeText.text = "" + this.selectedAction.range;

            //If this action is also an attack
            if(this.selectedAction.gameObject.GetComponent<AttackAction>())
            {
                AttackAction atkDetails = this.selectedAction.gameObject.GetComponent<AttackAction>();

                //Setting the crit details
                this.selectedPanelDetails.critText.text = "" + atkDetails.critChance;
                this.selectedPanelDetails.multiplierText.text = "" + atkDetails.critMultiplier;

                //Looping through each of the attacks to display their effects
                for(int a = 0; a < atkDetails.damageDealt.Count; ++a)
                {
                    //Finding out which text box to display damage
                    Text damageText;
                    switch(atkDetails.damageDealt[a].type)
                    {
                        case AttackDamage.DamageType.Physical:
                            damageText = this.selectedPanelDetails.physDamageText;
                            break;
                        case AttackDamage.DamageType.Magic:
                            damageText = this.selectedPanelDetails.magicDamageText;
                            break;
                        case AttackDamage.DamageType.Fire:
                            damageText = this.selectedPanelDetails.fireDamageText;
                            break;
                        case AttackDamage.DamageType.Water:
                            damageText = this.selectedPanelDetails.waterDamageText;
                            break;
                        case AttackDamage.DamageType.Electric:
                            damageText = this.selectedPanelDetails.electridDamageText;
                            break;
                        case AttackDamage.DamageType.Wind:
                            damageText = this.selectedPanelDetails.windDamageText;
                            break;
                        case AttackDamage.DamageType.Rock:
                            damageText = this.selectedPanelDetails.rockDamageText;
                            break;
                        case AttackDamage.DamageType.Light:
                            damageText = this.selectedPanelDetails.lightDamageText;
                            break;
                        case AttackDamage.DamageType.Dark:
                            damageText = this.selectedPanelDetails.darkDamageText;
                            break;
                        default:
                            damageText = this.selectedPanelDetails.physDamageText;
                            break;
                    }

                    //If the text box isn't blank (meaning it's already displaying damage) we add a space
                    if(damageText.text != "")
                    {
                        damageText.text += ", ";
                    }

                    //Shows base damage first
                    if(atkDetails.damageDealt[a].baseDamage > 0)
                    {
                        damageText.text += "" + atkDetails.damageDealt[a].baseDamage;
                    }

                    //Then shows the dice damage
                    if(atkDetails.damageDealt[a].diceRolled > 0 && atkDetails.damageDealt[a].diceSides > 0)
                    {
                        damageText.text += "" + atkDetails.damageDealt[a].diceRolled + "d" + atkDetails.damageDealt[a].diceSides;
                    }
                }

                //If there are effects for this attack
                if (atkDetails.effectsOnHit.Count > 0)
                {
                    this.selectedPanelDetails.effectNameText.text = "Effect on hit: ";

                    //Looping through each effect to display what they do
                    for(int e = 0; e < atkDetails.effectsOnHit.Count; ++e)
                    {
                        //If this isn't the first effect listed, we need to add a space
                        if(e > 0)
                        {
                            this.selectedPanelDetails.effectNameText.text += ", ";
                        }

                        //Displaying the name of the effect and the % of it happening
                        this.selectedPanelDetails.effectNameText.text += atkDetails.effectsOnHit[e].effectOnHit.effectName + "(" + 
                                                                         atkDetails.effectsOnHit[e].effectChance + ")";
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class ActionButton
{
    //The name text for the action
    public Text nameText;
    //The description for the action
    public Text descriptionText;
    //The button component that can be enabled/disabled
    public Button buttonComponent;
}

[System.Serializable]
public class SelectedActionPanel
{
    //The name of the action
    public Text nameText;
    //The description for the action
    public Text descriptionText;
    //The range of the action
    public Text rangeText;

    [Space(8)]

    //The crit range of the attack
    public Text critText;
    //The crit multiplier of the attack
    public Text multiplierText;

    [Space(8)]

    //The damage of the attack
    public Text physDamageText;
    public Text magicDamageText;
    public Text fireDamageText;
    public Text waterDamageText;
    public Text electridDamageText;
    public Text windDamageText;
    public Text rockDamageText;
    public Text lightDamageText;
    public Text darkDamageText;

    [Space(8)]

    //Effect on hit
    public Text effectNameText;
}