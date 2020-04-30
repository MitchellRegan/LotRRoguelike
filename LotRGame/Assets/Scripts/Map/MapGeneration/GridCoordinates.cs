using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Struct used in TileInfo.cs and CreateTileGrid.cs to save which col/row a tile is in
[System.Serializable]
public class GridCoordinates
{
    public int col;
    public int row;

    //Constructor for this struct
    public GridCoordinates(int col_, int row_)
    {
        this.col = col_;
        this.row = row_;
    }
}