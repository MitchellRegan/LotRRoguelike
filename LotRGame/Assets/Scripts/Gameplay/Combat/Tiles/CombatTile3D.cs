using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTile3D : MonoBehaviour
{
    //Tile's row and column
    public int row = 0;
    public int col = 0;

    //References to the tiles connected to this one
    [HideInInspector]
    public CombatTile3D up;
    [HideInInspector]
    public CombatTile3D down;
    [HideInInspector]
    public CombatTile3D left;
    [HideInInspector]
    public CombatTile3D right;

    //The movement cost of this tile (can be changed by obstacles)
    private int movementCost = 1;

    //Enum for what kind of object is on this tile
    [HideInInspector]
    public TileObjectType typeOnTile = TileObjectType.Nothing;

    //Color picker for when this tile is not in use
    public Color inactiveColor = Color.white;
    //Color picker for when this tile is hilighted
    public Color hilightColor = Color.blue;
    //Color picker for when a player character occupies this space
    public Color playerOccupiedColor = Color.green;
    //Color picker for when an enemy occupies this space
    public Color enemyOccupiedColor = Color.red;



    //Function called on the first frame this object exists
    private void Awake()
    {
        CombatManager.globalReference.GetComponent<CombatTileHandler>().AddCombatTileToGrid(this, this.row, this.col);
    }


    //Function called when this object is enabled
    private void OnEnable()
    {
        //Setting our movement cost to 1 by default. Other effects can change it later
        this.ChangeMovementCost(1);
    }


    //Function called from OnEnable and externally by objects on the combat tile grid to change this tile's movement cost
    public void ChangeMovementCost(int newCost_)
    {
        if(newCost_ < 1)
        {
            this.movementCost = 1;
        }
        else
        {
            this.movementCost = newCost_;
        }
    }


    //Function called from CombatTileHandler to reset this tile so there's nothing on it and it's set to the inactive color
    public void ResetTile()
    {
        /*this.objectOnThisTile = null;
        this.SetTileColor(this.inactiveColor);
        this.HighlightTile(false);*/
    }


    public void SetTileColor(Color newColor_)
    {
        this.GetComponent<Material>().color = newColor_; 
    }
}
