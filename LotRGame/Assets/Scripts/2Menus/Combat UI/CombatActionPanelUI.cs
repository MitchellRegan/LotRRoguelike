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

    //The list of all combat tiles that show the travel path of the selected movement action
    private List<CombatTile> movementPath;



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

        //Initializes the movement path list
        this.movementPath = new List<CombatTile>();
    }


    //Function called when this game object is enabled
    private void OnEnable()
    {
        //Resetting the Action Details Panel
        this.selectedAction = null;
        this.UpdateActionDetailsPanel();
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

        //Clearing the current travel path for movement and adding in the selected character's tile as the starting point
        this.movementPath.Clear();
        this.movementPath.Add(CombatManager.globalReference.FindCharactersTile(CombatManager.globalReference.actingCharacters[0]));

        //Finding out which tiles need to be hilighted
        List<CombatTile> tilesToHilight = PathfindingAlgorithms.FindTilesInActionRange(actingCharsTile, actionRange);

        //Looping through all tiles in range and hilighting them
        foreach(CombatTile tile in tilesToHilight)
        {
            tile.inActionRange = true;
            tile.HighlightTile(false);
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
                    case AttackAction.attackTouchType.Regular:
                        this.selectedPanelDetails.touchTypeText.text = "";
                        break;
                    case AttackAction.attackTouchType.IgnoreEvasion:
                        this.selectedPanelDetails.touchTypeText.text = "Ignores Evasion";
                        break;
                    case AttackAction.attackTouchType.IgnoreArmor:
                        this.selectedPanelDetails.touchTypeText.text = "Ignores Armor";
                        break;
                    case AttackAction.attackTouchType.IgnoreEvasionAndArmor:
                        this.selectedPanelDetails.touchTypeText.text = "Ignores Evasion & Armor";
                        break;
                }

                //Looping through each of the attacks to display their effects
                this.selectedPanelDetails.damageText.text = "Damage: ";
                for(int a = 0; a < atkDetails.damageDealt.Count; ++a)
                {
                    //If the text box isn't blank (meaning it's already displaying damage) we add a space
                    if (this.selectedPanelDetails.damageText.text != "")
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
                        case AttackDamage.DamageType.Physical:
                            this.selectedPanelDetails.damageText.text += " Physical";
                            break;
                        case AttackDamage.DamageType.Magic:
                            this.selectedPanelDetails.damageText.text += " Magic";
                            break;
                        case AttackDamage.DamageType.Fire:
                            this.selectedPanelDetails.damageText.text += " Fire";
                            break;
                        case AttackDamage.DamageType.Water:
                            this.selectedPanelDetails.damageText.text += " Water";
                            break;
                        case AttackDamage.DamageType.Electric:
                            this.selectedPanelDetails.damageText.text += " Electric";
                            break;
                        case AttackDamage.DamageType.Wind:
                            this.selectedPanelDetails.damageText.text += " Wind";
                            break;
                        case AttackDamage.DamageType.Rock:
                            this.selectedPanelDetails.damageText.text += " Rock";
                            break;
                        case AttackDamage.DamageType.Light:
                            this.selectedPanelDetails.damageText.text += " Light";
                            break;
                        case AttackDamage.DamageType.Dark:
                            this.selectedPanelDetails.damageText.text += " Dark";
                            break;
                        default:
                            this.selectedPanelDetails.damageText.text += " Physical";
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
                        this.selectedPanelDetails.effectNameText.text += atkDetails.effectsOnHit[e].effectOnHit.effectName + "(" + 
                                                                         atkDetails.effectsOnHit[e].effectChance + ")";
                    }
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


    //Function called every frame
    private void Update()
    {
        //If the currently selected action isn't empty and it's a movement action
        if(this.selectedAction != null && this.selectedAction.gameObject.GetComponent<MoveAction>())
        {
            //If there are tiles in the movement path and the mouse is hovering over a combat tile
            if (this.movementPath.Count > 0 && CombatTile.mouseOverTile != null)
            {
                //If the current movement path doesn't have more tiles than the character can move
                if (this.movementPath.Count <= this.selectedAction.range)
                {
                    //If the tile that the mouse is over is connected to the last tile in the current movement path
                    if (this.movementPath[this.movementPath.Count - 1].ourPathPoint.connectedPoints.Contains(CombatTile.mouseOverTile.ourPathPoint))
                    {
                        //If the tile that the mouse is over isn't already in the movement path
                        if (!this.movementPath.Contains(CombatTile.mouseOverTile))
                        {
                            Debug.Log("1");
                            this.movementPath.Add(CombatTile.mouseOverTile);
                            CombatTile.mouseOverTile.GetComponent<Image>().color = Color.blue;
                        }
                        //If the tile that the mouse is over IS already in the movement path and isn't the most recent tile
                        else if(this.movementPath[this.movementPath.Count - 1] != CombatTile.mouseOverTile && this.movementPath[0] != CombatTile.mouseOverTile)
                        {
                            Debug.Log("2");
                            //Removing all tiles in the movement path that come after this one
                            int indexOfPrevTile = this.movementPath.IndexOf(CombatTile.mouseOverTile) + 1;
                            for (int t = indexOfPrevTile; t < this.movementPath.Count; )
                            {
                                CombatTile.mouseOverTile.GetComponent<Image>().color = Color.white;

                                this.movementPath.RemoveAt(t);
                            }
                        }
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

    //The parent object of the damage display
    public GameObject attackDetails;

    //The crit range of the attack
    public Text critText;
    //The crit multiplier of the attack
    public Text multiplierText;

    //The touch type of this attack
    public Text touchTypeText;

    //The damage of the attack
    public Text damageText;

    //Effect on hit
    public Text effectNameText;
}