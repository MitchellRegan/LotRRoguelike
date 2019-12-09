using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateMapLocations : MonoBehaviour
{
    //The radius around the region center that the city tiles can be within
    public int cityRadiusFromCenter = 8;



    //Function called from TileMapManager to fill the map with locations and points of interest
    public void PopulateTileMap()
    {
        //Getting the tiles for each region's city
        this.FindCityTiles();

        //Getting a reference to the CreateMapRegions.cs component to find each region's info
        CreateMapRegions mapRegions = this.GetComponent<CreateMapRegions>();

        //Initializing the list of dungeon tiles
        TileMapManager.globalReference.dungeonTiles = new List<TileInfo>();

        //Looping through each city tile
        foreach (TileInfo cityTile in TileMapManager.globalReference.cityTiles)
        {
            //Reference for the region info that the city tile is in
            RegionInfo cityRegion = null;

            //Checking if the city is in a very easy region
            foreach (RegionInfo veRegion in mapRegions.veryEasy.regions)
            {
                //If this region has the same name as the current city tile
                if (veRegion.regionName == cityTile.regionName)
                {
                    //We save the region reference and break the loop
                    cityRegion = veRegion;
                    break;
                }
            }

            //Checking if the city is in an easy region
            if (cityRegion == null)
            {
                foreach (RegionInfo eRegion in mapRegions.easy.regions)
                {
                    //If this region has the same name as the current city tile
                    if (eRegion.regionName == cityTile.regionName)
                    {
                        //We save the region reference and break the loop
                        cityRegion = eRegion;
                        break;
                    }
                }
            }

            //Checking if the city is in a medium region
            if (cityRegion == null)
            {
                foreach (RegionInfo nRegion in mapRegions.medium.regions)
                {
                    //If this region has the same name as the current city tile
                    if (nRegion.regionName == cityTile.regionName)
                    {
                        //We save the region reference and break the loop
                        cityRegion = nRegion;
                        break;
                    }
                }
            }

            //Checking if the city is in an hard region
            if (cityRegion == null)
            {
                foreach (RegionInfo hRegion in mapRegions.hard.regions)
                {
                    //If this region has the same name as the current city tile
                    if (hRegion.regionName == cityTile.regionName)
                    {
                        //We save the region reference and break the loop
                        cityRegion = hRegion;
                        break;
                    }
                }
            }

            //Checking if the city is in a very hard region
            if (cityRegion == null)
            {
                foreach (RegionInfo vhRegion in mapRegions.veryHard.regions)
                {
                    //If this region has the same name as the current city tile
                    if (vhRegion.regionName == cityTile.regionName)
                    {
                        //We save the region reference and break the loop
                        cityRegion = vhRegion;
                        break;
                    }
                }
            }

            //Checking if the city is in the final region
            if (cityRegion == null)
            {
                foreach (RegionInfo fRegion in mapRegions.final.regions)
                {
                    //If this region has the same name as the current city tile
                    if (fRegion.regionName == cityTile.regionName)
                    {
                        //We save the region reference and break the loop
                        cityRegion = fRegion;
                        break;
                    }
                }
            }


            //If we've found the city tile's region
            if (cityRegion != null)
            {
                //If the region has a city prefab
                if (cityRegion.regionCity != null)
                {
                    //Adding the city object to this city tile's list of objects
                    cityTile.decorationModel = cityRegion.regionCity.gameObject;
                }

                //If the region has a dungeon prefab
                if (cityRegion.regionDungeon != null)
                {
                    //Getting all tiles in this city tile's region
                    List<TileInfo> allRegionTiles = PathfindingAlgorithms.FindLandTilesInRange(cityTile,
                                                    TileMapManager.globalReference.tileGrid.Count, true);

                    //Getting all tiles within a range of the city tile based on how big the map is. These tiles are blacklisted from having dungeons
                    List<TileInfo> noDungeonAllowedTiles = PathfindingAlgorithms.FindLandTilesInRange(cityTile,
                                Mathf.RoundToInt((TileMapManager.globalReference.tileGrid.Count * 1f) * 0.05f), true);

                    //Getting all edge tiles for this region so the dungeon isn't on the outskirts
                    List<TileInfo> regionEdgeTiles = PathfindingAlgorithms.FindRegionEdgeTiles(cityTile);

                    //Looping through all of the tiles where dungeons aren't allowed
                    foreach (TileInfo blacklistedTile in noDungeonAllowedTiles)
                    {
                        //Removing this blacklisted tile from the list of region tiles
                        allRegionTiles.Remove(blacklistedTile);
                    }

                    //Looping through all of the tiles along the region's edge
                    foreach (TileInfo edgeTile in regionEdgeTiles)
                    {
                        //Removing this edge tile from the list of region tiles
                        allRegionTiles.Remove(edgeTile);
                    }

                    //Finding a random tile in the remaining region to set as the dungeon tile
                    if (allRegionTiles.Count > 0)
                    {
                        //Looping through all of the available region tiles until we find a suitable one for the dungeon
                        while (allRegionTiles.Count > 0)
                        {
                            //Getting a random index
                            int randTile = Random.Range(0, allRegionTiles.Count);

                            //If the tile at the index doesn't match the same region as our city tile
                            if (allRegionTiles[randTile].regionName != cityRegion.regionName)
                            {
                                //We remove this tile from the list of available region tiles
                                allRegionTiles.RemoveAt(randTile);
                            }
                            //If the tile does have the same region as our city tile
                            else
                            {
                                //We add this tile to the list of dungeon tiles
                                TileMapManager.globalReference.dungeonTiles.Add(allRegionTiles[randTile]);

                                //Adding the dungeon object to this tile's list of objects
                                allRegionTiles[randTile].decorationModel = cityRegion.regionDungeon.gameObject;

                                //Breaking out of this while loop
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    
    //Function called from PopulateTileMap to find the locations where our cities will be
    private void FindCityTiles()
    {
        //The list of tiles where cities will be spawned
        List<TileInfo> cityTiles = new List<TileInfo>();
        //The list of tiles that will be used as starting points for pathfinding
        List<TileInfo> pathfindingStartingPoints = new List<TileInfo>();
        //Adding the first tile to the starting points list
        pathfindingStartingPoints.Add(TileMapManager.globalReference.tileGrid[0][0]);

        //Looping through all of the tiles to get starting points
        for (int c = 0; c < TileMapManager.globalReference.tileGrid.Count; ++c)
        {
            for (int r = 0; r < TileMapManager.globalReference.tileGrid[0].Count; ++r)
            {
                //Bool that determines if the current tile has a new region name
                bool isTileNewRegion = true;

                //Looping through all of the tiles in the pathfinding starting points
                foreach (TileInfo startPoint in pathfindingStartingPoints)
                {
                    //If the current tile has the same region name as any of the tiles in the pathfinding starting points, it's not a new tile to be added to the list
                    if (TileMapManager.globalReference.tileGrid[c][r].regionName == startPoint.regionName)
                    {
                        isTileNewRegion = false;
                        break;
                    }

                    //Bool for if all of the tile's surrounding tiles have the same region name
                    bool allConnectedSameRegion = true;
                    //Looping through each of this tile's connections. We'll only add it to the list of starting points if we find a group of tiles that are the same
                    foreach (TileInfo connection in TileMapManager.globalReference.tileGrid[c][r].connectedTiles)
                    {
                        //If the current connection is null or a tile with a different region name
                        if (connection == null || connection.regionName != TileMapManager.globalReference.tileGrid[c][r].regionName)
                        {
                            allConnectedSameRegion = false;
                        }
                    }

                    //If all of this tile's connected points aren't in the same region, we don't add it to the list
                    if (!allConnectedSameRegion)
                    {
                        isTileNewRegion = false;
                        break;
                    }
                }

                //If the tile is a new region, we add it to the region starting points
                if (isTileNewRegion)
                {
                    pathfindingStartingPoints.Add(TileMapManager.globalReference.tileGrid[c][r]);
                }
            }
        }


        //Once we find all of the starting points, we use pathfinding to get the center tile in each region
        foreach (TileInfo startingPoint in pathfindingStartingPoints)
        {
            //Finding the edge tiles for the region that the tile is in
            List<TileInfo> edgeTiles = PathfindingAlgorithms.FindRegionEdgeTiles(startingPoint);

            //Finding the center tile using the edge tiles
            TileInfo regionCenterTile = PathfindingAlgorithms.FindRegionCenterTile(edgeTiles);

            //Finding all of the tiles within a certain radius of the center
            List<TileInfo> centerRadiusTiles = PathfindingAlgorithms.FindLandTilesInRange(regionCenterTile, this.cityRadiusFromCenter, true);

            //Finding a random tile in the radius around the center to place the city on
            int randomTile = Random.Range(0, centerRadiusTiles.Count - 1);
            TileInfo cityTile = centerRadiusTiles[randomTile];

            //Adding the region center tile to the list of city tiles
            cityTiles.Add(cityTile);
        }


        //Returning the list of city tiles
        TileMapManager.globalReference.cityTiles = cityTiles;
    }
}
