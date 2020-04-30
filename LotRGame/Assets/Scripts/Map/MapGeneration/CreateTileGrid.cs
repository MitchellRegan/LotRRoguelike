using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingAlgorithms))]
public class CreateTileGrid : MonoBehaviour
{
    //The height of each tile
    public float tileHeight = 20;
    //The width of each tile. This value is generated on Awake using the tile height given
    private float tileWidth = 1;

    //The hexagon model prefab that's used for every tile
    public MeshRenderer hexMesh;
    
    //Prefab for the RegionInfo of the ocean
    public RegionInfo oceanRegion;
    

    
    //Called on initialization
    private void Awake()
    {
        //Finding the width of the tiles based on the height given and the ratio between a hex's height and width
        this.tileWidth = this.tileHeight * 1.1547f;
        this.tileWidth -= (this.tileWidth - this.tileHeight) * 1.9f;
    }


    //Public function called from TileMapManager. Creates the correct map based on the game difficulty
    public void GenerateHexGrid()
    {
        int cols = 0;
        int rows = 0;

        //Refreshing the seed so that the map will ALWAYS be the same with the same seed, since the Randomize feature in the sprite customizer can offset it
        GameData.globalReference.GetComponent<RandomSeedGenerator>().SetSeed(GameData.globalReference.GetComponent<RandomSeedGenerator>().seed);

        //Setting the number of columns and rows based on the game difficulty map sizes
        switch (GameData.globalReference.currentDifficulty)
        {
            case GameDifficulty.Easy:
                rows = Mathf.RoundToInt(GameData.globalReference.easyMapSize.y);
                cols = Mathf.RoundToInt(GameData.globalReference.easyMapSize.x);
                break;

            case GameDifficulty.Normal:
                rows = Mathf.RoundToInt(GameData.globalReference.normalMapSize.y);
                cols = Mathf.RoundToInt(GameData.globalReference.normalMapSize.x);
                break;

            case GameDifficulty.Hard:
                rows = Mathf.RoundToInt(GameData.globalReference.hardMapSize.y);
                cols = Mathf.RoundToInt(GameData.globalReference.hardMapSize.x);
                break;
        }
        
        //Generates the grid of tiles
        this.GenerateGrid(rows, cols);
        
        //Connects the path points between tiles
        this.ConnectTiles(cols, rows);
    }

    
    //Loops through and generates all tiles in the grid
    private void GenerateGrid(int rows_, int cols_)
    {
        //Saves the given rows, columns, and if we should add extra tiles
        TileMapManager.globalReference.rows = rows_;
        TileMapManager.globalReference.cols = cols_;
        
        //Initializing the 2D list of tiles
        TileMapManager.globalReference.tileGrid = new List<List<TileInfo>>(cols_);
        //Vector to hold the starting position for grid generation in the top-left quadrant
        Vector3 startPos = new Vector3();
        //Finding the starting x coordinate using width and number of columns
        startPos.x = 0;
        //Finding the starting y coordinate using height and number of rows
        startPos.z = 0;

        //Vector to hold the offset of the current tile when finding its correct location
        Vector3 offsetPos = new Vector3();

        //Bool that changes through every column loop. If true, offsets the current column up
        bool offsetCol = false;
        
        //Looping through each column
        for (int c = 0; c < cols_; ++c)
        {
            //Creating a new list to hold all tiles in this column
            TileMapManager.globalReference.tileGrid.Add(new List<TileInfo>(rows_));

            //Looping through each row in the current column
            for (int r = 0; r < rows_; ++r)
            {
                offsetPos.x = c * this.tileWidth; //The grid is generated from left to right
                offsetPos.z = r * this.tileHeight; //The grid is generated from bottom to top
                
                //If the current column is offset, we add half the height of a tile
                if (offsetCol)
                {
                    offsetPos.z += this.tileHeight / 2;
                }
                //Creating a new tile and positioning it at the offset of the start position
                TileMapManager.globalReference.tileGrid[c].Add(new TileInfo(this.oceanRegion));
                TileMapManager.globalReference.tileGrid[c][r].tilePosition = startPos + offsetPos;
            }
            //Changes the column offset for the next loop
            offsetCol = !offsetCol;
        }
    }


    //Connects all of the tiles to the tiles around them
    private void ConnectTiles(int cols_, int rows_)
    {
        //Bool that changes through every column loop. If true, the current column is offset down
        bool offsetCol = true;

        for (int c = 0; c < cols_; ++c)
        {
            for(int r = 0; r < rows_; ++r)
            {
                //Reference to the current tile
                TileInfo currentTile;

                //If the current tile isn't null, we connect the path points
                if(TileMapManager.globalReference.tileGrid[c][r] != null)
                {
                    currentTile = TileMapManager.globalReference.tileGrid[c][r];
                    
                    //Connecting to the tile below the current
                    if(r + 1 < rows_)
                    {
                        //Making sure the tile below exists
                        if(TileMapManager.globalReference.tileGrid[c][r+1] != null)
                        {
                            TileInfo bottomTile = TileMapManager.globalReference.tileGrid[c][r + 1];
                            //Connecting the current tile's south (bottom) point to the north (top) point of the tile below, and vice versa
                            currentTile.connectedTiles[0] = bottomTile;
                            currentTile.connectedTileCoordinates[0] = new GridCoordinates(c, r + 1);
                            bottomTile.connectedTiles[3] = currentTile;
                            bottomTile.connectedTileCoordinates[3] = new GridCoordinates(c, r);
                        }
                    }


                    //If the current column is offset down
                    if(offsetCol)
                    {
                        //Making sure the next column exists
                        if (c + 1 < cols_)
                        {
                            //The northeast tile is the same row, next column
                            TileInfo northEastTile = TileMapManager.globalReference.tileGrid[c + 1][r];
                            //Connecting the current tile's northeast point to the southwest point in the next tile, and vice versa
                            currentTile.connectedTiles[1] = northEastTile;
                            currentTile.connectedTileCoordinates[1] = new GridCoordinates(c + 1, r);
                            northEastTile.connectedTiles[4] = currentTile;
                            northEastTile.connectedTileCoordinates[4] = new GridCoordinates(c, r);


                            //Making sure the row down exists
                            if(r - 1 > -1)
                            {
                                if(TileMapManager.globalReference.tileGrid[c+1][r-1] != null)
                                {
                                    //The southeast tile is 1 row down, next column
                                    TileInfo southEastTile = TileMapManager.globalReference.tileGrid[c + 1][r - 1];
                                    //Connecting the current tile's southeast point to the northwest point to the next tile, and vice versa
                                    currentTile.connectedTiles[2] = southEastTile;
                                    currentTile.connectedTileCoordinates[2] = new GridCoordinates(c + 1, r - 1);
                                    southEastTile.connectedTiles[5] = currentTile;
                                    southEastTile.connectedTileCoordinates[5] = new GridCoordinates(c, r);
                                }
                            }
                        }
                    }
                    //If the current column isn't offset
                    else
                    {
                        //Making sure the next column exists
                        if (c + 1 < cols_)
                        {
                            //The southeast tile is the same row, next column
                            TileInfo southEastTile = TileMapManager.globalReference.tileGrid[c + 1][r];
                            //Connecting the current tile's southeast point to the northwest point in the next tile, and vice versa
                            currentTile.connectedTiles[2] = southEastTile;
                            currentTile.connectedTileCoordinates[2] = new GridCoordinates(c + 1, r);
                            southEastTile.connectedTiles[5] = currentTile;
                            southEastTile.connectedTileCoordinates[5] = new GridCoordinates(c, r);


                            //Making sure the row up exists
                            if (r + 1 < rows_)
                            {
                                //The northeast tile is 1 row up, next column
                                TileInfo northEastTile = TileMapManager.globalReference.tileGrid[c + 1][r + 1];
                                //Connecting the current tile's northeast point to the southwest point of the next tile, and vice versa
                                currentTile.connectedTiles[1] = northEastTile;
                                currentTile.connectedTileCoordinates[1] = new GridCoordinates(c + 1, r + 1);
                                northEastTile.connectedTiles[4] = currentTile;
                                northEastTile.connectedTileCoordinates[4] = new GridCoordinates(c, r);
                            }
                        }
                    }
                }
            }

            //Changes the offset after every loop
            offsetCol = !offsetCol;
        }
    }
    
    
    //Creates a region of a specific zone type using "spokes" that extend outward from the given tile
    private void GenerateSpokeRegion(int startTileRow_, int startTileCol_, Vector2 spokeMinMax_, Vector2 spokeLengthMinMax_, RegionInfo regionInfo_)
    {
        //Throwing an exception if the user inputs a starting tile that isn't on the grid
        if (startTileCol_ < 0 || startTileCol_ >= TileMapManager.globalReference.tileGrid.Count || startTileRow_ < 0 || startTileRow_ >= TileMapManager.globalReference.tileGrid[0].Count)
        {
            throw new System.Exception("The starting tile row or column must be within the bounds of the tile grid");
        }

        //Created a list of int arrays to hold all row/column locations for the tiles in this region
        List<List<int>> tilesInRegion = new List<List<int>>();
        tilesInRegion.Add(new List<int> { startTileCol_, startTileRow_}); 

        //Finding the number of spokes 
        int numberOfSpokes = Mathf.RoundToInt(Random.Range(spokeMinMax_.x, spokeMinMax_.y));


        //The total angle covered by all spokes. Used to offset each spoke from the previous one
        float totalAngle = Random.Range(0, 360);
        float newMinAngle = (360f / numberOfSpokes) * 0.5f;
        float newMaxAngle = newMinAngle * 3;

        //Looping through to create each spoke
        for(int s = 0; s < numberOfSpokes; ++s)
        {
            //Finding the angle of the current spoke offset from the previous spoke
            float spokeAngle = Random.Range(newMinAngle, newMaxAngle);

            totalAngle += spokeAngle;

            //Making sure the total angle is between 0 and 360 degrees
            if(totalAngle > 360)
            {
                totalAngle -= 360;
            }

            //Finding the length of the current spoke
            float spokeLength = Random.Range(spokeLengthMinMax_.x, spokeLengthMinMax_.y);

            //Floats to hold the difference in XY coordinates from the starting tile
            int xDiff = 0;
            int yDiff = 0;

            //Finding the angle under 90 degrees so that we can use trig functions
            float trigAngle = totalAngle % 90;

            //Finding the XY difference from the starting tile
            if (totalAngle <= 90)
            {
                //Tile is up and right
                xDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle * Mathf.Deg2Rad) * spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle * Mathf.Deg2Rad) * spokeLength);
            }
            else if(totalAngle <= 180)
            {
                //Tile is up and left
                xDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle * Mathf.Deg2Rad) * -spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle * Mathf.Deg2Rad) * spokeLength);
            }
            else if(totalAngle <= 270)
            {
                //Tile is down and left
                xDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle * Mathf.Deg2Rad) * -spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle * Mathf.Deg2Rad) * -spokeLength);
            }
            else
            {
                //Tile is down and right
                xDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle * Mathf.Deg2Rad) * spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle * Mathf.Deg2Rad) * -spokeLength);
            }


            //Making sure the tile is within the bouderies of the map
            if(startTileCol_ + xDiff >= TileMapManager.globalReference.cols)
            {
                xDiff = TileMapManager.globalReference.cols - startTileCol_ - 1;
            }
            else if(startTileCol_ + xDiff < 0)
            {
                xDiff = -startTileCol_;
            }

            if(startTileRow_ + yDiff >= TileMapManager.globalReference.rows)
            {
                yDiff = TileMapManager.globalReference.rows - startTileRow_ - 1;
            }
            else if(startTileRow_ + yDiff < 0)
            {
                yDiff = -startTileRow_;
            }

            //Adding this tile to the list of tiles in the region
            tilesInRegion.Add(new List<int> { startTileCol_ + xDiff, startTileRow_ + yDiff });
        }

        //The list of edge tiles around this region
        List<TileInfo> edgePoints = new List<TileInfo>();
        
        //Connecting the lines between points
        for (int p = 1; p < tilesInRegion.Count; ++p)
        {
            //Getting the coordinates of the tile at the start of the line
            int startX = tilesInRegion[p][0];
            int startY = tilesInRegion[p][1];
            //Getting the coordinates of the tile at the end of the line
            int endX;
            int endY;

            //If this is the last tile in the list, it needs to connect to the first
            if (p + 1 >= tilesInRegion.Count)
            {
                endX = tilesInRegion[1][0];
                endY = tilesInRegion[1][1];
            }
            else
            {
                endX = tilesInRegion[p + 1][0];
                endY = tilesInRegion[p + 1][1];
            }

            //Filling in a line between the two tiles
            TileInfo startPoint = TileMapManager.globalReference.tileGrid[startX][startY];
            TileInfo endPoint = TileMapManager.globalReference.tileGrid[endX][endY];

            List<TileInfo> linePoints = PathfindingAlgorithms.FindLineOfTiles(startPoint, endPoint);

            //Adding each edge tile to the list of edge tiles
            foreach (TileInfo tile in linePoints)
            {
                edgePoints.Add(tile);
            }
        }

        //Finding the starting tile
        int x = tilesInRegion[0][0];
        int y = tilesInRegion[0][1];
        TileInfo startTile = TileMapManager.globalReference.tileGrid[x][y];

        //Setting the tile info to all tiles in the region
        this.FillInRegionOfTiles(startTile, edgePoints, regionInfo_);
    }


    //Function called from GenerateSpokeRegion. Uses the same kind of algorithm as Breadth First Search to fill all tiles within a given region
    public void FillInRegionOfTiles(TileInfo startPoint_, List<TileInfo> edgeTiles_, RegionInfo regionInfo_)
    {
        //The list of path points that make up the frontier
        List<TileInfo> frontier = new List<TileInfo>();
        //Adding the starting point to the frontier and making sure its previous point is cleared
        frontier.Add(startPoint_);
        startPoint_.previousTile = null;
        startPoint_.hasBeenChecked = false;

        //The list of path points that have already been visited
        List<TileInfo> visitedPoints = new List<TileInfo>();
        visitedPoints.Add(startPoint_);

        //Adding all edge tiles to the list of visited points so it cuts off the search
        foreach(TileInfo edge in edgeTiles_)
        {
            visitedPoints.Add(edge);
            edge.hasBeenChecked = true;
        }


        //Loop through each path point in the frontier until it's empty
        while(frontier.Count != 0)
        {
            //Getting the reference to the next point to check
            TileInfo currentPoint = frontier[0];

            //Looping through each path point that's connected to the current point
            foreach(TileInfo connection in currentPoint.connectedTiles)
            {
                if(connection != null)
                {
                    //Making sure the point hasn't been visited yet
                    if (!connection.hasBeenChecked)
                    {
                        //Marking the connection as visited and adding it to the frontier
                        frontier.Add(connection);
                        visitedPoints.Add(connection);
                        connection.hasBeenChecked = true;
                    }
                }
            }

            //Removing the current tile from the frontier
            frontier.Remove(currentPoint);
        }


        //Looping through all of the tiles in our selection so that we can give them the region info
        foreach(TileInfo regionTile in visitedPoints)
        {
            //Setting the tile's info using the region given
            regionTile.SetTileBasedOnRegion(regionInfo_);

            //Clearing the pathfinding for each tile once we're finished with them
            regionTile.ClearPathfinding();
        }
    }
}

