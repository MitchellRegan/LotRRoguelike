using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBoostPerk : Perk
{
    //The number of added movement spaces
    public int addedMovementSpaces = 0;

    //The type of action that this perk effects
    public Action.ActionType actionTypeToBoost = Action.ActionType.Minor;

    //NOTE: This is used in CombatActionPanelUI.SelectActionAtIndex
}
