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
    private CombatStats actingCharacter;




    //Function called when this game object is created
    private void Awake()
    {
        //Initializes the movement path list
        this.movementPath = new List<CombatTile>();
        //adding in the selected character's tile as the starting point
        this.movementPath.Add(CombatManager.globalReference.FindCharactersTile(CombatManager.globalReference.actingCharacters[0]));
        //Getting the reference to the acting character from the Combat Manager
        this.actingCharacter = CombatManager.globalReference.actingCharacters[0].charCombatStats;
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
                //Removing the acting character from the tile they're on
                CombatManager.globalReference.combatTileGrid[this.actingCharacter.gridPositionCol][this.actingCharacter.gridPositionRow].SetObjectOnTile(null, CombatTile.ObjectType.Nothing);
                
                //Once the time has passed for this tile, the selected character's position is updated
                this.actingCharacter.gridPositionCol = this.movementPath[newTileMoved].col;
                this.actingCharacter.gridPositionRow = this.movementPath[newTileMoved].row;
                CombatManager.globalReference.combatTileGrid[this.actingCharacter.gridPositionCol][this.actingCharacter.gridPositionRow].SetObjectOnTile(this.actingCharacter.gameObject, CombatTile.ObjectType.Player);

                //If we've moved through all of the tiles on the movement path, this object is destroyed
                if (newTileMoved + 1 == this.movementPath.Count)
                {
                    CombatManager.globalReference.UpdateCombatTilePositions();
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
                        CombatTile.mouseOverTile.GetComponent<Image>().color = Color.blue;
                    }
                }
                //If the tile that the mouse is over IS already in the movement path and isn't the most recent tile
                else
                {
                    //Removing all tiles in the movement path that come after this one
                    int indexOfPrevTile = this.movementPath.IndexOf(CombatTile.mouseOverTile) + 1;
                    for (int t = indexOfPrevTile; t < this.movementPath.Count;)
                    {
                        this.movementPath[t].GetComponent<Image>().color = Color.white;

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
                    this.movementPath[t].GetComponent<Image>().color = Color.white;

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
                        this.movementPath[p].GetComponent<Image>().color = Color.white;
                    }

                    //Use the breadth first search algorithm to find the path to this tile from the player
                    List<CombatTile> newPath = this.BreadthFirstSearch(this.movementPath[0], CombatTile.mouseOverTile);
                    if (newPath.Count > 0)
                    {
                        this.movementPath = newPath;
                    }

                    //Looping through each tile that's now in the movement path and coloring it in
                    for(int t = 1; t < this.movementPath.Count; ++t)
                    {
                        this.movementPath[t].GetComponent<Image>().color = Color.blue;
                    }
                }
            }
        }
    }


    //Pathfinding algorithm that uses Breadth First Search to check all directions equally. Returns the tile path taken to get to the target tile.
    private List<CombatTile> BreadthFirstSearch(CombatTile startingPoint_, CombatTile targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of tiles that will be returned
        List<CombatTile> tilePath = new List<CombatTile>();

        //The list of path points that make up the frontier
        List<CombatTile> frontier = new List<CombatTile>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(startingPoint_);

        //The list of path points that have already been visited
        List<CombatTile> visitedPoints = new List<CombatTile>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.ourPathPoint.previousPoint = null;
        startingPoint_.ourPathPoint.hasBeenChecked = true;

        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            CombatTile currentPoint = frontier[0];
            
            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //If the target tile has nothing on it
                if (currentPoint.typeOnTile == CombatTile.ObjectType.Nothing)
                {
                    //Adding the current point's tile to the list of returned objects
                    tilePath.Add(currentPoint);
                }

                //Creating a variable to hold the reference to the previous point
                CombatTile prev = currentPoint.ourPathPoint.previousPoint.GetComponent<CombatTile>();

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev);

                    //If the point isn't the starting point
                    if (prev != startingPoint_)
                    {
                        //Setting the previous point to the next point in the path
                        prev = prev.ourPathPoint.previousPoint.GetComponent<CombatTile>();
                    }
                    //If the point is the starting point
                    else
                    {
                        //We break out of the loop
                        break;
                    }
                }

                //Reversing the list of path points since it's currently backward
                tilePath.Reverse();

                //If we exit early, the loop is broken
                if (earlyExit_)
                {
                    break;
                }
            }
            //If the current point isn't the point we're looking for
            else
            {
                //Looping through each path point that's connected to the current point
                foreach (PathPoint connection in currentPoint.ourPathPoint.connectedPoints)
                {
                    if (connection != null)
                    {
                        //If the connected point hasn't been visited yet
                        if (!connection.hasBeenChecked)
                        {
                            //Telling the connected point came from the current point we're checking
                            connection.previousPoint = currentPoint.ourPathPoint;

                            //If the connected tile isn't empty, we have to check it first
                            if (connection.GetComponent<CombatTile>().typeOnTile != CombatTile.ObjectType.Nothing)
                            {
                                //Making sure that this type of movement can safely travel across the type of object on the tile
                                if (connection.GetComponent<CombatTile>().typeOnTile == CombatTile.ObjectType.Object && this.ignoreObstacles ||
                                    connection.GetComponent<CombatTile>().typeOnTile == CombatTile.ObjectType.Enemy && this.ignoreEnemies ||
                                    connection.GetComponent<CombatTile>().typeOnTile == CombatTile.ObjectType.Player && this.ignoreEnemies)
                                {
                                    //Adding the connected point to the frontier and list of visited tiles
                                    frontier.Add(connection.GetComponent<CombatTile>());
                                }
                            }
                            else
                            {
                                //Adding the connected point to the frontier and list of visited tiles
                                frontier.Add(connection.GetComponent<CombatTile>());
                            }
                            visitedPoints.Add(connection.GetComponent<CombatTile>());
                            //Marking the tile as already checked so that it isn't added again
                            connection.hasBeenChecked = true;
                        }
                    }
                }

                //Adding the current point to the list of visited points and removing it from the frontier
                frontier.Remove(currentPoint);
            }
        }
        
        //Looping through all path points in the list of visited points to clear their data
        foreach (CombatTile point in visitedPoints)
        {
            point.ourPathPoint.ClearPathfinding();
        }
        
        //Returning the completed list of tiles
        return tilePath;
    }
}
