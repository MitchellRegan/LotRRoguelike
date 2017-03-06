using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAlgorithms : MonoBehaviour
{
    //Function called from GenerateSpokeRegion. Fills in all tiles along a line with the given tile type
    public List<PathPoint> FindLineOfTiles(PathPoint startPoint_, PathPoint endPoint_)
    {
        //List of game objects that form the line that will be filled in
        List<PathPoint> pathLine = new List<PathPoint>();

        PathPoint currentPoint = startPoint_;

        //Looping through path points until we find the correct one
        while (currentPoint != endPoint_)
        {
            //Creating a var to hold the reference to the point connected to the current point that's closest to the end
            PathPoint closestPoint = null;
            float closestPointDist = 0;

            //Looping through each connection to find the one that's closest to the end
            foreach (PathPoint connection in currentPoint.connectedPoints)
            {
                if (connection != null)
                {
                    if (closestPoint == null)
                    {
                        closestPoint = connection;
                        closestPointDist = Vector3.Distance(closestPoint.transform.position, endPoint_.transform.position);
                    }
                    else
                    {
                        //Finding the distance between this connected point and the end point
                        float connectionDist = Vector3.Distance(connection.transform.position, endPoint_.transform.position);

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
    public List<GameObject> BreadthFirstSearch(PathPoint startingPoint_, PathPoint targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<GameObject> tilePath = new List<GameObject>();

        //The list of path points that make up the frontier
        List<PathPoint> frontier = new List<PathPoint>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(startingPoint_);

        //The list of path points that have already been visited
        List<PathPoint> visitedPoints = new List<PathPoint>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousPoint = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            PathPoint currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint.gameObject);

                //Creating a variable to hold the reference to the previous point
                PathPoint prev = currentPoint.previousPoint;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev.gameObject);

                    //If the point isn't the starting point
                    if (prev != startingPoint_)
                    {
                        //Setting the previous point to the next point in the path
                        prev = prev.previousPoint;
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
                foreach (PathPoint connection in currentPoint.connectedPoints)
                {
                    if (connection != null)
                    {
                        //If the connected point hasn't been visited yet
                        if (!connection.hasBeenChecked)
                        {
                            //Telling the connected point came from the current point we're checking
                            connection.previousPoint = currentPoint;

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
        foreach (PathPoint point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Pathfinding algorithm that's identical to Breadth First Search, but takes into account movement costs. Returns the tile path taken to get to the target tile.
    public List<GameObject> DijkstraSearch(PathPoint startingPoint_, PathPoint targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<GameObject> tilePath = new List<GameObject>();

        //The list of path points that make up the frontier
        List<PathPoint> frontier = new List<PathPoint>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(startingPoint_);

        //The list of path points that have already been visited
        List<PathPoint> visitedPoints = new List<PathPoint>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousPoint = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            PathPoint currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint.gameObject);

                //Creating a variable to hold the reference to the previous point
                PathPoint prev = currentPoint.previousPoint;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev.gameObject);

                    //If the point isn't the starting point
                    if (prev != startingPoint_)
                    {
                        //Setting the previous point to the next point in the path
                        prev = prev.previousPoint;
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
                    foreach (PathPoint connection in currentPoint.connectedPoints)
                    {
                        if (connection != null)
                        {
                            //If the connected point hasn't been visited yet
                            if (!connection.hasBeenChecked)
                            {
                                //Telling the connected point came from the current point we're checking
                                connection.previousPoint = currentPoint;

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
        foreach (PathPoint point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Pathfinding algorithm that prioritizes the direct route to the target. Returns the tile path taken to get to the target tile.
    public List<GameObject> GreedyBestFirstSearch(PathPoint startingPoint_, PathPoint targetPoint_, bool earlyExit_ = true)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<GameObject> tilePath = new List<GameObject>();

        //The list of path points that make up the frontier (definition) and their distance from the target (key)
        SortedList<float, PathPoint> frontier = new SortedList<float, PathPoint>();
        //Adding the starting tile to the fronteir and making sure its previous point is cleared
        frontier.Add(Vector3.Distance(startingPoint_.transform.position, targetPoint_.transform.position), startingPoint_);

        //The list of path points that have already been visited
        List<PathPoint> visitedPoints = new List<PathPoint>();
        visitedPoints.Add(startingPoint_);

        startingPoint_.previousPoint = null;
        startingPoint_.hasBeenChecked = true;


        //Loop through each path point until the frontier is empty
        while (frontier.Count != 0)
        {
            //Getting the reference to the next path point to check
            PathPoint currentPoint = frontier[0];


            //If the current point is the path point we're looking for
            if (currentPoint == targetPoint_)
            {
                //Adding the current point's game object to the list of returned objects
                tilePath.Add(currentPoint.gameObject);

                //Creating a variable to hold the reference to the previous point
                PathPoint prev = currentPoint.previousPoint;

                //Looping through the trail of points back to the starting point
                while (true)
                {
                    //Adding the point's game object to the list of returned objects
                    tilePath.Add(prev.gameObject);

                    //If the point isn't the starting point
                    if (prev != startingPoint_)
                    {
                        //Setting the previous point to the next point in the path
                        prev = prev.previousPoint;
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
                foreach (PathPoint connection in currentPoint.connectedPoints)
                {
                    if (connection != null)
                    {
                        //If the connected point hasn't been visited yet
                        if (!connection.hasBeenChecked)
                        {
                            //Telling the connected point came from the current point we're checking
                            connection.previousPoint = currentPoint;

                            //Finding the distance from this connection to the target point
                            float connectionDist = Vector3.Distance(connection.transform.position, targetPoint_.transform.position);

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
        foreach (PathPoint point in visitedPoints)
        {
            point.ClearPathfinding();
        }


        //Returning the completed list of tiles
        return tilePath;
    }


    //Function that uses the A* pathfinding algorithm to find the most optimized route to the target. Returns the tile path taken to get to the target tile.
    public List<GameObject> AStarSearch(GameObject startingTile_, GameObject targetTile_)
    {
        //Creating the 2D list of game objects (tiles) that will be returned
        List<GameObject> tilePath = new List<GameObject>();




        //Returning the completed list of tiles
        return tilePath;
    }
}
