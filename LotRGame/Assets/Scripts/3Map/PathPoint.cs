using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    //A list of all of the path points that are connected to this point
    public List<PathPoint> connectedPoints = new List<PathPoint>(6);

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
        //Making a list of verts for the line renderer
        List<Vector3> vertList = new List<Vector3>();

        //Looping through each connected point
        foreach(PathPoint connection in this.connectedPoints)
        {
            if (connection != null)
            {
                //Adding verts for this point and the current connection
                vertList.Add(this.transform.position);
                vertList.Add(connection.transform.position);
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