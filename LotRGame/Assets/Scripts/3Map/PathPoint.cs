using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    //A list of all of the path points that are connected to this point
    public List<PathPoint> connectedPoints = new List<PathPoint>(3);
    //A line renderer that draws lines between all connected points
    LineRenderer ourLineRenderer;

    //Reference in pathfinding algorithms to the path point that lead to this one. Used in CreateTileGrid algorithms
    [HideInInspector]
    public PathPoint previousPoint;

    //Bool used in the pathfinding algorithms in CreateTileGrid.cs. If true, it will be ignored during the search.
    [HideInInspector]
    public bool hasBeenChecked = false;

    //The type of land tile this is
    [HideInInspector]
    public LandType type = LandType.Empty;

    //Bool used in the pathfinding algorithms in CreateTileGrid.cs. Represents the number of turns it takes to traverse this point
    [HideInInspector]
    public int movementCost = 1;
    //The current number of cycles in the pathfinding algorithms that have been spent on this point.
    [HideInInspector]
    public int currentMovement = 0;


    //Function called on the first frame
    private void Start()
    {
        //Setting the instance of our line renderer to this object's component
        this.ourLineRenderer = this.GetComponent<LineRenderer>();

        //Making a list of verts for the line renderer
        List<Vector3> vertList = new List<Vector3>();

        //Looping through each connected point
        foreach(PathPoint connection in this.connectedPoints)
        {
            //Adding verts for this point and the current connection
            vertList.Add(this.transform.position);
            vertList.Add(connection.transform.position);
        }

        //Looping through and adding all of the vertex positions to our line renderer's positions
        this.ourLineRenderer.numPositions = vertList.Count;
        for(int v = 0; v < vertList.Count; ++v)
        {
            this.ourLineRenderer.SetPosition(v, vertList[v]);
        }
    }


    //Function called from LandTile.cs to show or hide all points connected with this one.
    public void HilightConnectedPoints(bool hilightOn_, bool hilightConnections_)
    {
        //Turns this point on or off
        this.gameObject.SetActive(hilightOn_);

        //If we need to hilight the connections to this point
        if(hilightConnections_)
        {
            //Loops through each point connected with this one and turns them on or off depending on the passed bool
            foreach (PathPoint connection in this.connectedPoints)
            {
                connection.gameObject.SetActive(hilightOn_);
            }
        }
    }


    //Function called in the pathfinding algorithms in CreateTileGrid.cs. Clears this path point's previous point and the fact that it's been checked
    public void ClearPathfinding()
    {
        this.previousPoint = null;
        this.hasBeenChecked = false;
        this.currentMovement = 0;
    }

}


//Enum used in PathPoint.cs to denote the type of environment it's on
public enum LandType
{
    Empty,
    Ocean,
    River,
    Swamp,
    Grasslands,
    Forrest,
    Desert,
    Mountain,
    Volcano
}