using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used in SaveLoadManager.cs to store info from CreateTileGrid.cs to be serialized
[System.Serializable]
public class TileGridSaveInfo
{
    //The strings that holds the entire tile grid
    public List<List<string>> serializedTileGrid;
    //The strings for the tiles the cities are on
    public List<string> serializedCityTiles;
    //The strings for the tiles the dungeons are on
    public List<string> serializedDungeonTiles;

    //Public constructor for this class
    public TileGridSaveInfo(List<List<string>> tileGridString_, List<string> cityTilesString_, List<string> dungeonTilesString_)
    {
        this.serializedTileGrid = tileGridString_;
        this.serializedCityTiles = cityTilesString_;
        this.serializedDungeonTiles = dungeonTilesString_;
    }
}