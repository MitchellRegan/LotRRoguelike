using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveAction : Action
{
    //Determines if this type of movement allows the player to move through/over obstacles
    public bool ignoreObstacles = false;

    //Determines if this type of movement allows the player to move through/over enemies
    public bool ignoreEnemies = false;

    //The list of all combat tiles that show the travel path of the selected movement action
    private List<CombatTile> movementPath;

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
        this.movementPath = new List<CombatTile>();
        //adding in the selected character's tile as the starting point
        this.movementPath.Add(CombatManager.globalReference.FindCharactersTile(CombatManager.globalReference.actingCharacters[0]));
        //Getting the reference to the acting character from the Combat Manager
        this.actingCharacter = CombatManager.globalReference.actingCharacters[0];
    }


    //Function inherited from Action.cs
    public override void PerformAction(CombatTile targetTile_)
    {
        //Makes it so that the Update function will now move the character through the movement path
        this.moveCharacter = true;
    }


    //Function called every frame
    private void Update()
    {
        //If we should be animating this character moving from tile to tile
        if(this.moveCharacter)
        {
            //Finding the number of tiles that have currently been traveled
            int tilesMoved = Mathf.RoundToInt(this.currentTimePassed / (this.timeToCompleteAction / this.movementPath.Count));

            //Increasing the total time that's passed
            this.currentTimePassed += Time.deltaTime;

            //Finding the number of tiles that have been moved after this time progression
            int newTileMoved = Mathf.RoundToInt(this.currentTimePassed / (this.timeToCompleteAction / this.movementPath.Count));

            //If enough time has passed that we've moved one more tile further. We progress the acting character one more tile along the movement path
            if (tilesMoved < newTileMoved)
            {
                //Moving the character sprite to the new tile position
                CombatCharacterSprite charSprite = CombatManager.globalReference.GetCharacterSprite(this.actingCharacter);
                charSprite.transform.position = this.movementPath[newTileMoved].transform.position;

                //Making sure the sprites are positioned in front of each other correctly
                CombatManager.globalReference.UpdateCharacterSpriteOrder();

                //If the new tile is to the left of the old tile, we face the character left
                if(this.movementPath[newTileMoved].transform.position.x < this.movementPath[tilesMoved].transform.position.x)
                {
                    //If the current image's X scale is facing right, we face left
                    if (charSprite.spriteImage.transform.localScale.x > 0)
                    {
                        charSprite.spriteImage.transform.localScale = new Vector3(-1 * charSprite.spriteImage.transform.localScale.x,
                                                                                  charSprite.spriteImage.transform.localScale.y,
                                                                                  charSprite.spriteImage.transform.localScale.z);
                    }
                }
                //If the new tile is to the right of the old tile, we face the character right
                else if(this.movementPath[newTileMoved].transform.position.x > this.movementPath[tilesMoved].transform.position.x)
                {
                    //If the current image's X scale is facing left, we face right
                    if (charSprite.spriteImage.transform.localScale.x < 0)
                    {
                        charSprite.spriteImage.transform.localScale = new Vector3(-1 * charSprite.spriteImage.transform.localScale.x,
                                                                                  charSprite.spriteImage.transform.localScale.y,
                                                                                  charSprite.spriteImage.transform.localScale.z);
                    }
                }

                //Removing the acting character from the tile they're on
                CombatManager.globalReference.combatTileGrid[this.actingCharacter.charCombatStats.gridPositionCol][this.actingCharacter.charCombatStats.gridPositionRow].SetObjectOnTile(null, CombatTile.ObjectType.Nothing);
                
                //Once the time has passed for this tile, the selected character's position is updated
                this.actingCharacter.charCombatStats.gridPositionCol = this.movementPath[newTileMoved].col;
                this.actingCharacter.charCombatStats.gridPositionRow = this.movementPath[newTileMoved].row;
                CombatManager.globalReference.combatTileGrid[this.actingCharacter.charCombatStats.gridPositionCol][this.actingCharacter.charCombatStats.gridPositionRow].SetObjectOnTile(this.actingCharacter.gameObject, CombatTile.ObjectType.Player);

                //Looping through and triggering all effects on the moving character that happen during movement
                foreach(Effect e in this.actingCharacter.charCombatStats.combatEffects)
                {
                    e.EffectOnMove();

                    //Checking to see if the acting character has died due to some effect
                    if(this.actingCharacter.GetComponent<PhysicalState>().currentHealth <= 0)
                    {
                        //Clearing the movement path tiles
                        for(int t = newTileMoved + 1; t < this.movementPath.Count; ++t)
                        {
                            this.movementPath[t].GetComponent<Image>().color = new Color(1,1,1, this.movementPath[t].inactiveTransparency);
                        }
                        //This game object is destroyed
                        Destroy(this.gameObject);
                        break;
                    }
                }

                //If we've moved through all of the tiles on the movement path, this object is destroyed
                if (newTileMoved + 1 == this.movementPath.Count)
                {
                    //Setting the character's combat sprite to a stationary position directly on the last tile in our movement path
                    CombatManager.globalReference.GetCharacterSprite(this.actingCharacter).transform.position = this.movementPath[this.movementPath.Count - 1].transform.position;

                    Destroy(this.gameObject);
                }
            }
        }
        //If there are tiles in the movement path and the mouse is hovering over a combat tile
        else if (this.movementPath.Count > 0 && CombatTile.mouseOverTile != null)
        {
            //If the tile that the mouse is over is connected to the last tile in the current movement path
            if (this.movementPath[this.movementPath.Count - 1].ourPathPoint.connectedPoints.Contains(CombatTile.mouseOverTile.ourPathPoint) && this.movementPath.Count <= this.range)
            {
                //If the tile that the mouse is over isn't already in the movement path and this type of movement allows the user to ignore obstacles
                if (!this.movementPath.Contains(CombatTile.mouseOverTile))
                {
                    //If the tile has no object on it OR if there is an object and the movement action ignores objects
                    if (CombatTile.mouseOverTile.typeOnTile == CombatTile.ObjectType.Nothing || 
                            (CombatTile.mouseOverTile.typeOnTile == CombatTile.ObjectType.Object && this.ignoreObstacles) ||
                            ((CombatTile.mouseOverTile.typeOnTile == CombatTile.ObjectType.Enemy || CombatTile.mouseOverTile.typeOnTile == CombatTile.ObjectType.Player) && this.ignoreEnemies))
                    {
                        this.movementPath.Add(CombatTile.mouseOverTile);
                        CombatTile.mouseOverTile.HighlightTile(true);
                        CombatTile.mouseOverTile.SetTileColor(Color.blue);
                    }
                }
                //If the tile that the mouse is over IS already in the movement path and isn't the most recent tile
                else
                {
                    //Removing all tiles in the movement path that come after this one
                    int indexOfPrevTile = this.movementPath.IndexOf(CombatTile.mouseOverTile) + 1;
                    for (int t = indexOfPrevTile; t < this.movementPath.Count;)
                    {
                        this.movementPath[t].HighlightTile(false);
                        this.movementPath[t].SetTileColor(Color.white);

                        this.movementPath.RemoveAt(t);
                    }
                }
            }
            //If the tile that the mouse is over is NOT connected to the last tile in the current movement path but is still in the path
            else if (this.movementPath.Contains(CombatTile.mouseOverTile))
            {
                //Removing all tiles in the movement path that come after this one
                int indexOfPrevTile = this.movementPath.IndexOf(CombatTile.mouseOverTile) + 1;
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
                if (CombatTile.mouseOverTile.inActionRange)
                {
                    //Looping through all of the tiles currently in the movement path and clearing them
                    for (int p = 1; p < this.movementPath.Count; ++p)
                    {
                        this.movementPath[p].HighlightTile(false);
                        this.movementPath[p].SetTileColor(Color.white);
                    }

                    //Use the breadth first search algorithm to find the path to this tile from the player
                    List<CombatTile> newPath = PathfindingAlgorithms.BreadthFirstSearchCombat(this.movementPath[0], CombatTile.mouseOverTile, this.ignoreObstacles, this.ignoreEnemies);
                    if (newPath.Count > 0)
                    {
                        this.movementPath = newPath;
                    }

                    //Looping through each tile that's now in the movement path and coloring it in
                    for(int t = 1; t < this.movementPath.Count; ++t)
                    {
                        this.movementPath[t].HighlightTile(true);
                        this.movementPath[t].GetComponent<Image>().color = Color.blue;
                    }
                }
            }
        }
    }


    //Function called from CombatTile.cs to see if a specific tile is in the current movement path
    public bool IsTileInMovementPath(CombatTile tileToCheck_)
    {
        //Looping through each tile in the current movement path
        foreach(CombatTile ct in this.movementPath)
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
        foreach(CombatTile tile in this.movementPath)
        {
            tile.HighlightTile(false);
            tile.SetTileColor(Color.white);
        }
    }
}
