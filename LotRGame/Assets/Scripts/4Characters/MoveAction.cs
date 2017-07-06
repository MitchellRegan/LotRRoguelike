using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action
{
    //Determines if this type of movement allows the player to move through/over obstacles
    public bool ignoreObstacles = false;

    //Determines if this type of movement allows the player to move through/over enemies
    public bool ignoreEnemies = false;



    //Function inherited from Action.cs
    public override void PerformAction(CombatTile targetTile_)
    {
        //Finding the amount of time it takes for the character to move from one tile to another
        float timeBetweenMoves = this.timeToCompleteAction / CombatActionPanelUI.globalReference.movementPath.Count;
        Debug.Log(timeBetweenMoves);
        
        //Looping through each tile the character needs to move onto
        foreach(CombatTile pathTile in CombatActionPanelUI.globalReference.movementPath)
        {
            Debug.Log("Moo");
            //Waiting for each tile move
            CombatActionPanelUI.globalReference.MoveTimeDelay(timeBetweenMoves);

            //Getting a reference to the acting character's combat stats component
            CombatStats actingCharCombat = CombatManager.globalReference.actingCharacters[0].charCombatStats;

            //Removing the acting character from the tile they're on
            CombatManager.globalReference.combatTileGrid[actingCharCombat.gridPositionCol][actingCharCombat.gridPositionRow].SetObjectOnTile(null, CombatTile.ObjectType.Nothing);

            //Once the time has passed for this tile, the selected character's position is updated
            CombatManager.globalReference.actingCharacters[0].charCombatStats.gridPositionCol = pathTile.col;
            CombatManager.globalReference.actingCharacters[0].charCombatStats.gridPositionRow = pathTile.row;
            CombatManager.globalReference.combatTileGrid[actingCharCombat.gridPositionCol][actingCharCombat.gridPositionRow].SetObjectOnTile(actingCharCombat.gameObject, CombatTile.ObjectType.Player);
        }

        //Updating the combat tiles to show the character's new position
        CombatManager.globalReference.UpdateCombatTilePositions();
    }
}
