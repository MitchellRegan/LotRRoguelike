using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MoveAction : Action
{
    //Determines if this type of movement allows the player to move through/over obstacles
    public bool ignoreObstacles = false;

    //Determines if this type of movement allows the player to move through/over enemies
    public bool ignoreEnemies = false;

    //The list of all combat tiles that show the travel path of the selected movement action
    private List<CombatTile3D> movementPath;
    //The current count of tiles this character has moved
    private int currentNumTilesMoved = 0;

    //Bool that determines if we should be animating the player moving yet or not
    private bool moveCharacter = false;
    //Counter to hold how much time has passed after this action is called
    private float currentTimePassed = 0;
    //Reference to the character that we'll be moving
    private Character actingCharacter;




    //Function called when this game object is created
    private void Awake()
    {
        //Initializes the movement path list
        this.movementPath = new List<CombatTile3D>();
        //adding in the selected character's tile as the starting point
        this.movementPath.Add(CombatManager.globalReference.tileHandler.FindCharactersTile(CombatManager.globalReference.initiativeHandler.actingCharacters[0]));
        //Getting the reference to the acting character from the Combat Manager
        this.actingCharacter = CombatManager.globalReference.initiativeHandler.actingCharacters[0];
    }


    //Function inherited from Action.cs
    public override void PerformAction(CombatTile3D targetTile_)
    {
        //Calling the base function to start the cooldown time
        base.PerformAction(targetTile_);
        
        //If the acting character is an enemy, we need to set the movement path since we're not mousing over tiles
        if (CombatManager.globalReference.characterHandler.enemyCharacters.Contains(this.actingCharacter))
        {
            this.movementPath = PathfindingAlgorithms.BreadthFirstSearchCombat(this.movementPath[0], targetTile_, true, true);
        }
        //Otherwise the movement path has already been set by our Update function ("else if" when this.moveCharacter is false)

        //Makes it so that the Update function will now move the character through the movement path
        this.moveCharacter = true;
        this.currentNumTilesMoved = 0;
    }


    //Function called every frame
    private void Update()
    {
        //If we should be animating this character moving from tile to tile
        if(this.moveCharacter)
        {
            //Increasing the total time that's passed
            this.currentTimePassed += Time.deltaTime;

            //If enough time has passed that we've moved one more tile further. We progress the acting character one more tile along the movement path
            if (this.currentTimePassed >= this.timeToCompleteAction)
            {
                //Increasing the index for the number of tiles moved
                this.currentNumTilesMoved += 1;

                //Resetting the current movement time
                this.currentTimePassed = 0;

                //Moving the character sprite to the new tile position
                GameObject charModel = CombatManager.globalReference.characterHandler.GetCharacterModel(this.actingCharacter);
                charModel.transform.position = this.movementPath[this.currentNumTilesMoved].transform.position;

                //If the new tile is to the left of the old tile, we face the character left
                if(this.movementPath[this.currentNumTilesMoved].transform.position.x < this.movementPath[this.currentNumTilesMoved - 1].transform.position.x)
                {
                    charModel.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                //If the new tile is to the right of the old tile, we face the character right
                else if(this.movementPath[this.currentNumTilesMoved].transform.position.x > this.movementPath[this.currentNumTilesMoved - 1].transform.position.x)
                {
                    charModel.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                //If the new tile is above the old tile, we face the character up
                else if(this.movementPath[this.currentNumTilesMoved].transform.position.y > this.movementPath[this.currentNumTilesMoved - 1].transform.position.y)
                {
                    charModel.transform.eulerAngles = new Vector3(0, 90, 0);
                }
                //If the new tile is below the old tile, we face the character down
                else if (this.movementPath[this.currentNumTilesMoved].transform.position.y < this.movementPath[this.currentNumTilesMoved - 1].transform.position.y)
                {
                    charModel.transform.eulerAngles = new Vector3(0, 270, 0);
                }

                //Removing the acting character from the tile they're on
                CombatManager.globalReference.tileHandler.combatTileGrid[this.actingCharacter.charCombatStats.gridPositionCol][this.actingCharacter.charCombatStats.gridPositionRow].SetObjectOnTile(null, TileObjectType.Nothing);
                
                //Once the time has passed for this tile, the selected character's position is updated
                this.actingCharacter.charCombatStats.gridPositionCol = this.movementPath[this.currentNumTilesMoved].col;
                this.actingCharacter.charCombatStats.gridPositionRow = this.movementPath[this.currentNumTilesMoved].row;
                CombatManager.globalReference.tileHandler.combatTileGrid[this.actingCharacter.charCombatStats.gridPositionCol][this.actingCharacter.charCombatStats.gridPositionRow].SetObjectOnTile(this.actingCharacter.gameObject, TileObjectType.Player);

                //Looping through and triggering all effects on the moving character that happen during movement
                foreach(Effect e in this.actingCharacter.charCombatStats.combatEffects)
                {
                    e.EffectOnMove();

                    //Checking to see if the acting character has died due to some effect
                    if(this.actingCharacter.GetComponent<PhysicalState>().currentHealth <= 0)
                    {
                        //Clearing the movement path tiles
                        for(int t = this.currentNumTilesMoved; t < this.movementPath.Count; ++t)
                        {
                            this.movementPath[t].SetTileColor(this.movementPath[t].unusedColor);
                        }
                        //This game object is destroyed
                        Destroy(this.gameObject);
                        break;
                    }
                }

                //If we've moved through all of the tiles on the movement path, this object is destroyed
                if (this.currentNumTilesMoved + 1 == this.movementPath.Count)
                {
                    //Setting the character's combat sprite to a stationary position directly on the last tile in our movement path
                    CombatManager.globalReference.characterHandler.GetCharacterModel(this.actingCharacter).transform.position = this.movementPath[this.movementPath.Count - 1].transform.position;

                    Destroy(this.gameObject);
                }
            }
        }
        //If there are tiles in the movement path and the mouse is hovering over a combat tile
        else if (this.movementPath.Count > 0 && CombatTile3D.mouseOverTile != null)
        {
            CombatTile3D lastPathTile = this.movementPath[this.movementPath.Count - 1];
            List<CombatTile3D> connectedTiles = new List<CombatTile3D>() { lastPathTile.left, lastPathTile.right, lastPathTile.up, lastPathTile.down };

            //If the tile that the mouse is over is connected to the last tile in the current movement path
            if (connectedTiles.Contains(CombatTile3D.mouseOverTile) && this.movementPath.Count <= this.range)
            {
                //If the tile that the mouse is over isn't already in the movement path and this type of movement allows the user to ignore obstacles
                if (!this.movementPath.Contains(CombatTile3D.mouseOverTile))
                {
                    //If the tile has no object on it OR if there is an object and the movement action ignores objects
                    if (CombatTile3D.mouseOverTile.typeOnTile == TileObjectType.Nothing || 
                            (CombatTile3D.mouseOverTile.typeOnTile == TileObjectType.Object && this.ignoreObstacles) ||
                            ((CombatTile3D.mouseOverTile.typeOnTile == TileObjectType.Enemy || CombatTile3D.mouseOverTile.typeOnTile == TileObjectType.Player) && this.ignoreEnemies))
                    {
                        this.movementPath.Add(CombatTile3D.mouseOverTile);
                        CombatTile3D.mouseOverTile.HighlightTile(true, true);
                    }
                }
                //If the tile that the mouse is over IS already in the movement path and isn't the most recent tile
                else
                {
                    //Removing all tiles in the movement path that come after this one
                    int indexOfPrevTile = this.movementPath.IndexOf(CombatTile3D.mouseOverTile) + 1;
                    for (int t = indexOfPrevTile; t < this.movementPath.Count;)
                    {
                        this.movementPath[t].HighlightTile(false);
                        this.movementPath[t].SetTileColor(Color.white);

                        this.movementPath.RemoveAt(t);
                    }
                }
            }
            //If the tile that the mouse is over is NOT connected to the last tile in the current movement path but is still in the path
            else if (this.movementPath.Contains(CombatTile3D.mouseOverTile))
            {
                //Removing all tiles in the movement path that come after this one
                int indexOfPrevTile = this.movementPath.IndexOf(CombatTile3D.mouseOverTile) + 1;
                for (int t = indexOfPrevTile; t < this.movementPath.Count;)
                {
                    this.movementPath[t].HighlightTile(false);
                    this.movementPath[t].SetTileColor(Color.white);

                    this.movementPath.RemoveAt(t);
                }
            }
            //If the tile that the mouse is over is neither on the movement path or on a tile connected to it
            else
            {
                //Making sure the tile that the mouse is over is within this action's range
                if (CombatTile3D.mouseOverTile.inActionRange)
                {
                    //Looping through all of the tiles currently in the movement path and clearing them
                    for (int p = 1; p < this.movementPath.Count; ++p)
                    {
                        this.movementPath[p].HighlightTile(false);
                        this.movementPath[p].SetTileColor(Color.white);
                    }

                    //Use the breadth first search algorithm to find the path to this tile from the player
                    List<CombatTile3D> newPath = PathfindingAlgorithms.BreadthFirstSearchCombat(this.movementPath[0], CombatTile3D.mouseOverTile, this.ignoreObstacles, this.ignoreEnemies);
                    if (newPath.Count > 0)
                    {
                        this.movementPath = newPath;
                    }

                    //Looping through each tile that's now in the movement path and coloring it in
                    for(int t = 1; t < this.movementPath.Count; ++t)
                    {
                        this.movementPath[t].HighlightTile(true, true);
                    }
                }
            }
        }
    }


    //Function called from CombatTile3D.cs to see if a specific tile is in the current movement path
    public bool IsTileInMovementPath(CombatTile3D tileToCheck_)
    {
        //Looping through each tile in the current movement path
        foreach(CombatTile3D ct in this.movementPath)
        {
            //Returns true if the current tile is the one we're looking for
            if(ct == tileToCheck_)
            {
                return true;
            }
        }

        //If we make it through the loop, the tile we're looking for isn't in the movement path
        return false;
    }


    //Function called from CombatManager.EndActingCharactersTurn so we can clear tile highlights if no move action is selected
    public void ClearMovePathHighlights()
    {
        foreach(CombatTile3D tile in this.movementPath)
        {
            tile.HighlightTile(false);
            tile.SetTileColor(Color.white);
        }
    }
}
