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
    public ActionType actionTypeShown = ActionType.Major;

    //The list of action buttons that 
    public List<ActionButton> actionButtons;

    //The action that's currently selected
    [HideInInspector]
    public Action selectedAction;
    //The panel that displays the selected action's details
    public SelectedActionPanel selectedPanelDetails;

    [Space(8)]

    //References to the buttons that display the different kinds of actions available to the player
    public Button standardActButton;
    public Button secondaryActButton;
    public Button quickActButton;
    public Button fullRoundActButton;
    //Reference to the object that displays all of the actions of the selected type
    public GameObject actionDisplay;



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

        //Clears the action detail panel
        this.UpdateActionDetailsPanel();

        //Disables this game object
        this.gameObject.SetActive(false);
    }


    //Function called when this game object is enabled
    private void OnEnable()
    {
        //Resetting the Action Details Panel and destroying the previously selected action's object if it exists
        if(this.selectedAction != null)
        {
            Destroy(this.selectedAction.gameObject);
        }
        this.selectedAction = null;
        this.UpdateActionDetailsPanel();

        //Enabling all of the action buttons
        if (CombatManager.globalReference.actingCharacters[0].charActionList.majorActions.Count > 0)
        {
            this.standardActButton.interactable = true;
        }
        else
        {
            this.standardActButton.interactable = false;
        }
        if (CombatManager.globalReference.actingCharacters[0].charActionList.minorActions.Count > 0)
        {
            this.secondaryActButton.interactable = true;
        }
        else
        {
            this.secondaryActButton.interactable = false;
        }
        if (CombatManager.globalReference.actingCharacters[0].charActionList.fastActions.Count > 0)
        {
            this.quickActButton.interactable = true;
        }
        else
        {
            this.quickActButton.interactable = false;
        }
        if (CombatManager.globalReference.actingCharacters[0].charActionList.massiveActions.Count > 0)
        {
            this.fullRoundActButton.interactable = true;
        }
        else
        {
            this.fullRoundActButton.interactable = false;
        }
        //Hiding the action display until one of the action buttons is pressed
        this.actionDisplay.SetActive(false);
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
                this.UpdateActionButtons(actingCharActions.majorActions);
                this.backgroundImage.color = this.standardColor;
                this.actionTypeShown = ActionType.Major;
                break;
            case 1:
                this.UpdateActionButtons(actingCharActions.minorActions);
                this.backgroundImage.color = this.secondaryColor;
                this.actionTypeShown = ActionType.Minor;
                break;
            case 2:
                this.UpdateActionButtons(actingCharActions.fastActions);
                this.backgroundImage.color = this.quickColor;
                this.actionTypeShown = ActionType.Fast;
                break;
            case 3:
                this.UpdateActionButtons(actingCharActions.massiveActions);
                this.backgroundImage.color = this.fullRoundColor;
                this.actionTypeShown = ActionType.Massive;
                break;
            default:
                this.UpdateActionButtons(actingCharActions.majorActions);
                this.backgroundImage.color = this.standardColor;
                this.actionTypeShown = ActionType.Major;
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
                //Checking to see if the current action is a weapon action
                if (this.actionButtons[b].GetType() == typeof(WeaponAction))
                {
                    WeaponAction wpAct = actionsToShow_[b] as WeaponAction;
                    //If this weapon action can't be used, we need to reduce the current action count by 1
                    if(!wpAct.CanCharacterUseAction(CombatManager.globalReference.actingCharacters[0]))
                    {
                        b -= 1;
                    }
                }
                //If the action isn't a weapon action, we can display it normally
                else
                {
                    this.actionButtons[b].nameText.text = actionsToShow_[b].actionName;
                    this.actionButtons[b].descriptionText.text = actionsToShow_[b].actionDescription;
                    this.actionButtons[b].buttonComponent.interactable = true;
                }
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

        //If the currently selected action is a move action, we need to clear tile highlights along its movement path
        if(this.selectedAction != null && this.selectedAction.GetComponent<MoveAction>())
        {
            this.selectedAction.GetComponent<MoveAction>().ClearMovePathHighlights();
        }

        //Destroying the game object that holds the currently selected action
        if(this.selectedAction != null)
        {
            Destroy(this.selectedAction.gameObject);
        }

        //Creating an instance of the newly selected action prefab object
        GameObject actionObj = null;

        //Finding the range of the action based on the button hit
        int actionRange = 0;
        switch(this.actionTypeShown)
        {
            case ActionType.Major:
                actionRange = actingCharacter.charActionList.majorActions[actionIndex_].range;
                actionObj = GameObject.Instantiate(actingCharacter.charActionList.majorActions[actionIndex_].gameObject);
                //this.selectedAction = actingCharacter.charActionList.standardActions[actionIndex_];
                break;
            case ActionType.Minor:
                actionRange = actingCharacter.charActionList.minorActions[actionIndex_].range;
                actionObj = GameObject.Instantiate(actingCharacter.charActionList.minorActions[actionIndex_].gameObject);
                break;
            case ActionType.Fast:
                actionRange = actingCharacter.charActionList.fastActions[actionIndex_].range;
                actionObj = GameObject.Instantiate(actingCharacter.charActionList.fastActions[actionIndex_].gameObject);
                break;
            case ActionType.Massive:
                actionRange = actingCharacter.charActionList.massiveActions[actionIndex_].range;
                actionObj = GameObject.Instantiate(actingCharacter.charActionList.massiveActions[actionIndex_].gameObject);
                break;
        }

        //Getting the action component reference from the created action object
        this.selectedAction = actionObj.GetComponent<Action>();

        //Finding out which tiles need to be hilighted if this action isn't a move action
        List<CombatTile> tilesToHighlight;
        List<CombatTile> tilesToCheckForCharacters = new List<CombatTile>();
        if (!this.selectedAction.GetComponent<MoveAction>())
        {
            tilesToHighlight = PathfindingAlgorithms.FindTilesInActionRange(actingCharsTile, actionRange);
            tilesToCheckForCharacters = tilesToHighlight;
        }
        //If this action is a move action, we have to find the selected tiles based on environment obstacles
        else
        {
            MoveAction ourMoveAct = this.selectedAction.GetComponent<MoveAction>();

            //Looping through all of the acting character's perks to see if there are any movement boost perks
            int rangeModifier = 0;
            foreach(Perk charPerk in actingCharacter.charPerks.allPerks)
            {
                //If the current perk is a movement boost perk that applies to this movement's action type, we apply the number of added spaces
                if(charPerk.GetType() == typeof(MovementBoostPerk) && ourMoveAct.type == charPerk.GetComponent<MovementBoostPerk>().actionTypeToBoost)
                {
                    rangeModifier += charPerk.GetComponent<MovementBoostPerk>().addedMovementSpaces;
                }
            }

            //Highlighting all tiles in range
            tilesToHighlight = PathfindingAlgorithms.FindTilesInActionRange(actingCharsTile, actionRange + rangeModifier, ourMoveAct.ignoreObstacles);
            tilesToCheckForCharacters = PathfindingAlgorithms.FindTilesInActionRange(actingCharsTile, actionRange + rangeModifier + 1);
        }

        //Looping through all tiles in range and hilighting them
        foreach(CombatTile tile in tilesToHighlight)
        {
            tile.inActionRange = true;
            tile.HighlightTile(false);
        }

        //Looping through all of the tiles around the action highlights to check for characters
        foreach(CombatTile checkedTile in tilesToCheckForCharacters)
        {
            //If there's a character sprite on this tile, we hide it a bit
            if (checkedTile.objectOnThisTile != null)
            {
                if (checkedTile.objectOnThisTile.GetComponent<Character>())
                {
                    //Getting the sprite base for the character
                    CharacterSpriteBase cSprite = CombatManager.globalReference.GetCharacterSprite(checkedTile.objectOnThisTile.GetComponent<Character>());
                    //If the character on the tile isn't the one that's acting
                    if (cSprite.ourCharacter != CombatManager.globalReference.actingCharacters[0])
                    {
                        cSprite.MakeSpritesTransparent();
                    }
                }
            }
        }

        //Displays the action's details
        this.UpdateActionDetailsPanel();
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

            this.selectedPanelDetails.touchTypeText.text = "";
            this.selectedPanelDetails.critText.text = "";
            this.selectedPanelDetails.multiplierText.text = "";
            this.selectedPanelDetails.accuracyText.text = "";

            this.selectedPanelDetails.damageText.text = "";

            this.selectedPanelDetails.effectNameText.text = "";
        }
        //If there is a selected action, the text panels display the details of what it does
        else
        {
            this.selectedPanelDetails.nameText.text = this.selectedAction.actionName;
            this.selectedPanelDetails.descriptionText.text = this.selectedAction.actionDescription;
            this.selectedPanelDetails.rangeText.text = "Range: " + this.selectedAction.range;

            //If this action is also an attack
            if(this.selectedAction.gameObject.GetComponent<AttackAction>())
            {
                AttackAction atkDetails = this.selectedAction.gameObject.GetComponent<AttackAction>();

                //Displays the attack damage grid
                this.selectedPanelDetails.attackDetails.SetActive(true);

                //Setting the crit details
                this.selectedPanelDetails.critText.text = "Crit: " + Mathf.RoundToInt(atkDetails.critChance * 100) + "%";
                this.selectedPanelDetails.multiplierText.text = "Crit Multiplier: x" + atkDetails.critMultiplier;

                //Setting the type of touch type it is
                switch(atkDetails.touchType)
                {
                    case AttackTouchType.Regular:
                        this.selectedPanelDetails.touchTypeText.text = "";
                        break;
                    case AttackTouchType.IgnoreEvasion:
                        this.selectedPanelDetails.touchTypeText.text = "Ignores Evasion";
                        break;
                    case AttackTouchType.IgnoreArmor:
                        this.selectedPanelDetails.touchTypeText.text = "Ignores Armor";
                        break;
                    case AttackTouchType.IgnoreEvasionAndArmor:
                        this.selectedPanelDetails.touchTypeText.text = "Ignores Evasion & Armor";
                        break;
                }

                //Setting the accuracy bonus details
                if(atkDetails.accuracyBonus != 0)
                {
                    //If the attacker's accuracy is increased
                    if (atkDetails.accuracyBonus > 0)
                    {
                        this.selectedPanelDetails.accuracyText.text = atkDetails.accuracyBonus + "% Bonus to Accuracy";
                    }
                    //If the accuracy is lowered
                    else
                    {
                        this.selectedPanelDetails.accuracyText.text = atkDetails.accuracyBonus + "% Penalty to Accuracy";
                    }
                }
                //If there is no bonus, no text is shown
                else
                {
                    this.selectedPanelDetails.accuracyText.text = "";
                }

                //Looping through each of the attacks to display their effects
                this.selectedPanelDetails.damageText.text = "Damage: ";
                for(int a = 0; a < atkDetails.damageDealt.Count; ++a)
                {
                    //If the text box isn't blank (meaning it's already displaying damage) we add a space
                    if (a > 0)
                    {
                        this.selectedPanelDetails.damageText.text += ", + ";
                    }

                    //Shows base damage first
                    if (atkDetails.damageDealt[a].baseDamage > 0)
                    {
                        this.selectedPanelDetails.damageText.text += "" + atkDetails.damageDealt[a].baseDamage;
                    }

                    //Then shows the dice damage
                    if (atkDetails.damageDealt[a].diceRolled > 0 && atkDetails.damageDealt[a].diceSides > 0)
                    {
                        this.selectedPanelDetails.damageText.text += "" + atkDetails.damageDealt[a].diceRolled + "d" + atkDetails.damageDealt[a].diceSides;
                    }

                    //Finding out which type of damage it is
                    switch(atkDetails.damageDealt[a].type)
                    {
                        case DamageType.Slashing:
                            this.selectedPanelDetails.damageText.text += " Slashing";
                            break;
                        case DamageType.Stabbing:
                            this.selectedPanelDetails.damageText.text += " Stabbing";
                            break;
                        case DamageType.Crushing:
                            this.selectedPanelDetails.damageText.text += " Crushing";
                            break;
                        case DamageType.Arcane:
                            this.selectedPanelDetails.damageText.text += " Arcane";
                            break;
                        case DamageType.Fire:
                            this.selectedPanelDetails.damageText.text += " Fire";
                            break;
                        case DamageType.Water:
                            this.selectedPanelDetails.damageText.text += " Water";
                            break;
                        case DamageType.Electric:
                            this.selectedPanelDetails.damageText.text += " Electric";
                            break;
                        case DamageType.Wind:
                            this.selectedPanelDetails.damageText.text += " Wind";
                            break;
                        case DamageType.Holy:
                            this.selectedPanelDetails.damageText.text += " Holy";
                            break;
                        case DamageType.Dark:
                            this.selectedPanelDetails.damageText.text += " Dark";
                            break;
                        case DamageType.Pure:
                            this.selectedPanelDetails.damageText.text += " Pure";
                            break;
                        case DamageType.Nature:
                            this.selectedPanelDetails.damageText.text += " Nature";
                            break;
                        case DamageType.Bleed:
                            this.selectedPanelDetails.damageText.text += " Bleed";
                            break;
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
                        this.selectedPanelDetails.effectNameText.text += atkDetails.effectsOnHit[e].effectToApply.effectName + "(" + 
                                                                         atkDetails.effectsOnHit[e].effectChance * 100 + "%)";
                    }
                }
                //If there are no effects for this attack
                else
                {
                    this.selectedPanelDetails.effectNameText.text = "";
                }
            }
            //If this isn't an attack action, the attack details are hidden
            else
            {
                //Hides the attack damage grid
                this.selectedPanelDetails.attackDetails.SetActive(false);
            }
        }
    }


    //Function called externally when an action is used. Disables that kind of action so the player can't use it again this turn
    public void DisableUsedActions()
    {
        //The actions are disabled based on what type was used
        switch(this.selectedAction.type)
        {
            case ActionType.Major:
                this.standardActButton.interactable = false;
                this.fullRoundActButton.interactable = false;
                break;
            case ActionType.Minor:
                this.secondaryActButton.interactable = false;
                this.fullRoundActButton.interactable = false;
                break;
            case ActionType.Fast:
                this.quickActButton.interactable = false;
                break;
            case ActionType.Massive:
                this.fullRoundActButton.interactable = false;
                this.standardActButton.interactable = false;
                this.secondaryActButton.interactable = false;
                break;

        }

        //Regardless of what kind of action was used, the action display is disabled
        this.actionDisplay.SetActive(false);

        //Destroying the object for the selected action as long as it's not a movement action (they destroy themselves)
        if(this.selectedAction != null && !this.selectedAction.GetComponent<MoveAction>())
        {
            Destroy(this.selectedAction.gameObject);
        }
        this.selectedAction = null;

        //Clearing the action details panel
        this.UpdateActionDetailsPanel();
    }
}
