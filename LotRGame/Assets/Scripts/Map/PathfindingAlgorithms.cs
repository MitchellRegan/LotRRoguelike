using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAlgorithms : MonoBehaviour
{
    //Function called from GenerateSpokeRegion. Fills in all tiles along a line with the given tile type
    public static List<TileInfo> FindLineOfTiles(TileInfo startPoint_, TileInfo endPoint_)
    {
        //List of game objects that form the line that will be filled in
        List<TileInfo> pathLine = new List<TileInfo>();

        TileInfo currentPoint = startPoint_;

        //Looping through path points until we find the correct one
        while (currentPoint != endPoint_)
        {
            //Creating a var to hold the reference to the point connected to the current point that's closest to the end
            TileInfo closestPoint = null;
            float closestPointDist = 0;

            //Looping through each connection to find the one that's closest to the end
            foreach (TileInfo connection in currentPoint.connectedTiles)
            {
                if (connection != null)
                {
                    if (closestPoint == null)
                    {
                        closestPoint = connection;
                        closestPointDist = Vector3.Distance(closestPoint.tilePosition, endPoint_.tilePosition);
                    }
                    else
                    {
                        //Finding the distance between this connected point and the end point
                        float connectionDist = Vector3.Distance(connection.tilePosition, endPoint_.tilePosition);

                        //If this connected point is closer to the end than the current closest, this point becomes the new closest
                        if (connectionDist < closestPointDist)
                        {
                            closestPoint = connection;
                            closestPointDist = connectionDist;
                        }
                    }
                }
            }

            //Adding the closest point to the path list
            pathLine.Add(closestPoint);
            //Changing the current point to the closest one
            currentPoint = closestPoint;
        }

        //Returning the line
        return pathLine;
    }


    //Pathfinding algorithm that uses Breadth First Search to check all directions equally. Returns the tile path taken to get to the target tile.
    public static List<TileInfo> BreadthFirstSearch(TileInfo startingPoint_, TileInfo targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<TileInfo> tilePath = new List<TileInfo>();

        //The list of path points that make up the frontier
        List<TileInfo> frontier = new List<TileInfo>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(startingPoint_);

        //The list of path points that have already been visited
        List<TileInfo> visitedPoints = new List<TileInfo>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousTile = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            TileInfo currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint);

                //Creating a variable to hold the reference to the previous point
                TileInfo prev = currentPoint.previousTile;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev);

                    //If the point isn't the starting point
                    if (prev != startingPoint_)
                    {
                        //Setting the previous point to the next point in the path
                        prev = prev.previousTile;
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
                foreach (TileInfo connection in currentPoint.connectedTiles)
                {
                    if (connection != null)
                    {
                        //If the connected point hasn't been visited yet
                        if (!connection.hasBeenChecked)
                        {
                            //Telling the connected point came from the current point we're checking
                            connection.previousTile = currentPoint;

                            //Adding the connected point to the frontier and list of visited tiles
                            frontier.Add(connection);
                            visitedPoints.Add(connection);
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
        foreach (TileInfo point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Pathfinding algorithm that's identical to Breadth First Search, but takes into account movement costs. Returns the tile path taken to get to the target tile.
    public static List<TileInfo> DijkstraSearch(TileInfo startingPoint_, TileInfo targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<TileInfo> tilePath = new List<TileInfo>();

        //The list of path points that make up the frontier
        List<TileInfo> frontier = new List<TileInfo>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(startingPoint_);

        //The list of path points that have already been visited
        List<TileInfo> visitedPoints = new List<TileInfo>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousTile = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            TileInfo currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint);

                //Creating a variable to hold the reference to the previous point
                TileInfo prev = currentPoint;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev);

                    //If the point isn't the starting point
                    if (prev != startingPoint_)
                    {
                        //Setting the previous point to the next point in the path
                        prev = prev.previousTile;
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
                //Adding 1 to the movement on this point
                currentPoint.currentMovement += 1;

                //If the maximum movement has been reached for this point
                if (currentPoint.currentMovement >= currentPoint.movementCost)
                {
                    //Looping through each path point that's connected to the current point
                    foreach (TileInfo connection in currentPoint.connectedTiles)
                    {
                        if (connection != null)
                        {
                            //If the connected point hasn't been visited yet
                            if (!connection.hasBeenChecked)
                            {
                                //Telling the connected point came from the current point we're checking
                                connection.previousTile = currentPoint;

                                //Adding the connected point to the frontier and list of visited tiles
                                frontier.Add(connection);
                                visitedPoints.Add(connection);
                                //Marking the tile as already checked so that it isn't added again
                                connection.hasBeenChecked = true;
                            }
                        }
                    }

                    //Adding the current point to the list of visited points and removing it from the frontier
                    frontier.Remove(currentPoint);
                }
                //If the point still requires more movement
                else
                {
                    //This point is removed from the front of the frontier and placed at the end
                    frontier.Remove(currentPoint);
                    frontier.Add(currentPoint);
                }
            }
        }


        //Looping through all path points in the list of visited points to clear their data
        foreach (TileInfo point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Pathfinding algorithm that's identical to Breadth First Search, but takes into account movement costs. Returns the tile path taken to get to the target tile.
    public static List<TileInfo> DijkstraSearchLandTile(TileInfo startingPoint_, TileInfo targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<TileInfo> tilePath = new List<TileInfo>();

        //The list of path points that make up the frontier
        List<TileInfo> frontier = new List<TileInfo>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(startingPoint_);

        //The list of path points that have already been visited
        List<TileInfo> visitedPoints = new List<TileInfo>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousTile = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            TileInfo currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint);

                //Creating a variable to hold the reference to the previous point
                TileInfo prev = currentPoint.previousTile;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev);

                    //If the point isn't the starting point
                    if (prev != startingPoint_)
                    {
                        //Setting the previous point to the next point in the path
                        prev = prev.previousTile;
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
                //Adding 1 to the movement on this point
                currentPoint.currentMovement += 1;

                //If the maximum movement has been reached for this point
                if (currentPoint.currentMovement >= currentPoint.movementCost)
                {
                    //Looping through each path point that's connected to the current point
                    foreach (TileInfo connection in currentPoint.connectedTiles)
                    {
                        if (connection != null)
                        {
                            //If the connected point hasn't been visited yet
                            if (!connection.hasBeenChecked)
                            {
                                //Telling the connected point came from the current point we're checking
                                connection.previousTile = currentPoint;

                                //Adding the connected point to the frontier and list of visited tiles
                                frontier.Add(connection);
                                visitedPoints.Add(connection);
                                //Marking the tile as already checked so that it isn't added again
                                connection.hasBeenChecked = true;
                            }
                        }
                    }

                    //Adding the current point to the list of visited points and removing it from the frontier
                    frontier.Remove(currentPoint);
                }
                //If the point still requires more movement
                else
                {
                    //This point is removed from the front of the frontier and placed at the end
                    frontier.Remove(currentPoint);
                    frontier.Add(currentPoint);
                }
            }
        }


        //Looping through all path points in the list of visited points to clear their data
        foreach (TileInfo point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Pathfinding algorithm that prioritizes the direct route to the target. Returns the tile path taken to get to the target tile.
    public static List<TileInfo> GreedyBestFirstSearch(TileInfo startingPoint_, TileInfo targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<TileInfo> tilePath = new List<TileInfo>();

        //The list of path points that make up the frontier (definition) and their distance from the target (key)
        SortedList<float, TileInfo> frontier = new SortedList<float, TileInfo>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(Vector3.Distance(startingPoint_.tilePosition, targetPoint_.tilePosition), startingPoint_);

        //The list of path points that have already been visited
        List<TileInfo> visitedPoints = new List<TileInfo>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousTile = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            TileInfo currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint);

                //Creating a variable to hold the reference to the previous point
                TileInfo prev = currentPoint.previousTile;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev);

                    //If the point is the starting point
                    if (prev == startingPoint_)
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
                foreach (TileInfo connection in currentPoint.connectedTiles)
                {
                    if (connection != null)
                    {
                        //If the connected point hasn't been visited yet
                        if (!connection.hasBeenChecked)
                        {
                            //Telling the connected point came from the current point we're checking
                            connection.previousTile = currentPoint;

                            //Finding the distance from this connection to the target point
                            float connectionDist = Vector3.Distance(connection.tilePosition, targetPoint_.tilePosition);

                            //Adding the connected point to the frontier and list of visited tiles
                            frontier.Add(connectionDist, connection);
                            visitedPoints.Add(connection);
                            //Marking the tile as already checked so that it isn't added again
                            connection.hasBeenChecked = true;
                        }
                    }
                }

                //Adding the current point to the list of visited points and removing it from the frontier
                frontier.RemoveAt(frontier.IndexOfValue(currentPoint));
            }
        }


        //Looping through all path points in the list of visited points to clear their data
        foreach (TileInfo point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Pathfinding algorithm that prioritizes the direct route to the target. Returns the tile path taken to get to the target tile.
    public static List<TileInfo> GreedyBestFirstSearchLandTile(TileInfo startingPoint_, TileInfo targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<TileInfo> tilePath = new List<TileInfo>();

        //The list of path points that make up the frontier (definition) and their distance from the target (key)
        SortedList<float, TileInfo> frontier = new SortedList<float, TileInfo>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(Vector3.Distance(startingPoint_.tilePosition, targetPoint_.tilePosition), startingPoint_);

        //The list of path points that have already been visited
        List<TileInfo> visitedPoints = new List<TileInfo>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousTile = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            TileInfo currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint);

                //Creating a variable to hold the reference to the previous point
                TileInfo prev = currentPoint.previousTile;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev);

                    //If the point is the starting point
                    if (prev != startingPoint_)
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
                foreach (TileInfo connection in currentPoint.connectedTiles)
                {
                    if (connection != null)
                    {
                        //If the connected point hasn't been visited yet
                        if (!connection.hasBeenChecked)
                        {
                            //Telling the connected point came from the current point we're checking
                            connection.previousTile = currentPoint;

                            //Finding the distance from this connection to the target point
                            float connectionDist = Vector3.Distance(connection.tilePosition, targetPoint_.tilePosition);

                            //Adding the connected point to the frontier and list of visited tiles
                            frontier.Add(connectionDist, connection);
                            visitedPoints.Add(connection);
                            //Marking the tile as already checked so that it isn't added again
                            connection.hasBeenChecked = true;
                        }
                    }
                }

                //Adding the current point to the list of visited points and removing it from the frontier
                frontier.RemoveAt(frontier.IndexOfValue(currentPoint));
            }
        }


        //Looping through all path points in the list of visited points to clear their data
        foreach (TileInfo point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Function that uses the A* pathfinding algorithm to find the most optimized route to the target. Returns the tile path taken to get to the target tile.
    public static List<GameObject> AStarSearch(GameObject startingTile_, GameObject targetTile_)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<GameObject> tilePath = new List<GameObject>();




        //Returning the completed list of tiles
        return tilePath;
    }


    //Function called from CombatActionPanelUI.cs. Returns all combat tiles within the given range of the starting tile
    public static List<CombatTile> FindTilesInActionRange(CombatTile startingTile_, int actionRange_)
    {
        //The list of combat tiles that are returned
        List<CombatTile> allTilesInRange = new List<CombatTile>();

        //The list of each group of tiles in every range incriment. The index is the range
        List<List<CombatTile>> tilesInEachIncriment = new List<List<CombatTile>>();
        //Creating the first range incriment which always includes the starting tile
        List<CombatTile> range0 = new List<CombatTile>() { startingTile_ };
        range0[0].ourPathPoint.hasBeenChecked = true;
        tilesInEachIncriment.Add(range0);

        for(int r = 1; r <= actionRange_; ++r)
        {
            //Creating a new list of tiles for this range incriment
            List<CombatTile> newRange = new List<CombatTile>();

            //Looping through each tile in the previous range
            foreach(CombatTile tile in tilesInEachIncriment[r-1])
            {
                //Looping through each tile connected to the one we're checking
                foreach(PathPoint connection in tile.ourPathPoint.connectedPoints)
                {
                    //If the connected tile hasn't already been checked
                    if(!connection.hasBeenChecked)
                    {
                        //Adding the connected tile to this new range and marking it as checked
                        newRange.Add(connection.GetComponent<CombatTile>());
                        connection.hasBeenChecked = true;
                    }
                }
            }

            //Adding this range incriment to the list
            tilesInEachIncriment.Add(newRange);
        }

        //Grouping all of the tiles into the list that is returned
        foreach(List<CombatTile> rangeList in tilesInEachIncriment)
        {
            foreach(CombatTile tile in rangeList)
            {
                allTilesInRange.Add(tile);
                //Resetting the tile to say it hasn't been checked
                tile.ourPathPoint.hasBeenChecked = false;
            }
        }

        return allTilesInRange;
    }


    //Pathfinding algorithm that uses Breadth First Search to check all directions equally. Returns the tile path taken to get to the target tile.
    public static List<CombatTile> BreadthFirstSearchCombat(CombatTile startingPoint_, CombatTile targetPoint_, bool avoidObjects_ = true, bool avoidCharacters_ = true)
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

                //Exiting early since there's no reason to continue
                break;
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
                                if (connection.GetComponent<CombatTile>().typeOnTile == CombatTile.ObjectType.Object && avoidObjects_ ||
                                    connection.GetComponent<CombatTile>().typeOnTile == CombatTile.ObjectType.Enemy && avoidCharacters_ ||
                                    connection.GetComponent<CombatTile>().typeOnTile == CombatTile.ObjectType.Player && avoidCharacters_)
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
