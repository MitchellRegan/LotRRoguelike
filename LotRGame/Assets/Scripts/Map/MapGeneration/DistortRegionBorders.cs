using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortRegionBorders : MonoBehaviour
{
    //The number of times we loop through the randomization process
    [Range(0, 25)]
    public int randomizationLoops = 15;

    /* Definition for the following variables:
     *      A "STEP" is expanding the border of a region by one adjacent tile */

    //The min and max percent that a region can step based on the number of tiles along its edge
    public Vector2 minMaxStepPercent = new Vector2(0.1f, 0.25f);
    //The bare bones minimum steps that a region has to step
    public int absoluteMinimumSteps = 45;



    //Function called from TileMapManager.cs to distort the tile map regions
    public void DistortTileMap()
    {
        //Looping through to randomize the regions so that they grow in different directions
        for(int i = 0; i < this.randomizationLoops; i++)
        {
            this.ExpandRegionBoarders();
        }
    }


    //Function called from StartMapCreation to expand the boarders of each region
    private void ExpandRegionBoarders()
    {
        //Making a list of each index for the number of total cities
        List<int> numCities = new List<int>();
        for (int r = 0; r < TileMapManager.globalReference.cityTiles.Count; ++r)
        {
            numCities.Add(r);
        }

        //Creating a separate list to randomize the order of the city indexes
        List<int> randCityOrder = new List<int>();
        for (int c = 0; c < TileMapManager.globalReference.cityTiles.Count; ++c)
        {
            //Getting a random index from the numCities list
            int randIndex = Random.Range(0, TileMapManager.globalReference.cityTiles.Count);
            //Adding the city index to our list of random city orders
            randCityOrder.Add(numCities[randIndex]);
        }

        //Looping through each city in our randomized order
        foreach (int index in randCityOrder)
        {
            //We get the tile reference for the given index's city
            TileInfo cityTile = TileMapManager.globalReference.cityTiles[index];

            //We find the list of tiles along the edge of the city tile's region
            List<TileInfo> edgeTiles = PathfindingAlgorithms.FindRegionEdgeTiles(cityTile);

            //And then we have the region step outward based on the size of the region's boarders
            Vector2 minMaxSteps = new Vector2();
            minMaxSteps.x = edgeTiles.Count * this.minMaxStepPercent.x;
            minMaxSteps.y = edgeTiles.Count * this.minMaxStepPercent.y;
            PathfindingAlgorithms.StepOutRegionEdge(edgeTiles, minMaxSteps, this.absoluteMinimumSteps);
        }

        //Making a list of each index for the number of total dungeons
        List<int> numDungeons = new List<int>();
        for (int d = 0; d < TileMapManager.globalReference.dungeonTiles.Count; ++d)
        {
            numDungeons.Add(d);
        }

        //Creating a separate list to randomize the order of the dungeon indexes
        List<int> randDungeonOrder = new List<int>();
        for (int c = 0; c < TileMapManager.globalReference.dungeonTiles.Count; ++c)
        {
            //Getting a random index from the numDungeons list
            int randIndex = Random.Range(0, TileMapManager.globalReference.dungeonTiles.Count);
            //Adding the dungeon index to our list of random dungeon orders
            randDungeonOrder.Add(numDungeons[randIndex]);
        }

        //Looping through each dungeon in our randomized order
        foreach (int index in randDungeonOrder)
        {
            //We get the tile reference for the given index's dungeon
            TileInfo dungeonTile = TileMapManager.globalReference.dungeonTiles[index];

            //We find the list of tiles along the edge of the dungeon tile's region
            List<TileInfo> edgeTiles = PathfindingAlgorithms.FindRegionEdgeTiles(dungeonTile);

            //And then we have the region step outward based on the size of the region's boarders
            Vector2 minMaxSteps = new Vector2();
            minMaxSteps.x = edgeTiles.Count * this.minMaxStepPercent.x;
            minMaxSteps.y = edgeTiles.Count * this.minMaxStepPercent.y;
            PathfindingAlgorithms.StepOutRegionEdge(edgeTiles, minMaxSteps, this.absoluteMinimumSteps);
        }
    }
}
