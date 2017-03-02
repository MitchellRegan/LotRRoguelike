﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingAlgorithms))]
public class CreateTileGrid : MonoBehaviour
{
    //Game object used to test this script
    public GameObject landTile;

    //The number of columns of hexes this grid has
    public int cols = 6;
    //The number of rows of hexes there are in each column
    public int rows = 6;

    //The height of each tile
    public float tileHeight = 20;
    //The width of each tile. This value is generated on Awake using the tile height given
    private float tileWidth = 1;

    //If true, adds an extra tile at the bottom of offset columns
    public bool addExtraTileOnOffset = true;

    //The object that all of the generated tiles are parented to
    public GameObject gridParent;

    //2 dimensional array of tiles that's generated
    public List<List<GameObject>> tileGrid;
    


    //Called on initialization
    private void Awake()
    {
        //Setting the number of columns and rows based on the game difficulty
        switch(GameData.globalReference.currentDifficulty)
        {
            case GameData.gameDifficulty.Easy:
                this.rows = Mathf.RoundToInt(GameData.globalReference.easyMapSize.y);
                this.cols = Mathf.RoundToInt(GameData.globalReference.easyMapSize.x);
                break;

            case GameData.gameDifficulty.Normal:
                this.rows = Mathf.RoundToInt(GameData.globalReference.normalMapSize.y);
                this.cols = Mathf.RoundToInt(GameData.globalReference.normalMapSize.x);
                break;

            case GameData.gameDifficulty.Hard:
                this.rows = Mathf.RoundToInt(GameData.globalReference.hardMapSize.y);
                this.cols = Mathf.RoundToInt(GameData.globalReference.hardMapSize.x);
                break;
        }


        //Generates the grid
        this.GenerateGrid(this.rows, this.cols, this.addExtraTileOnOffset);

        //Connects the path points between tiles
        this.ConnectTiles();

        //Getting the test tile info
        TileInfo testInfo = new TileInfo("Testing", LandType.Grasslands, new Vector2(0.5f, 0.8f), new Vector2(3, 4));
        this.GenerateSpokeRegion(this.tileGrid.Count / 2, this.tileGrid[0].Count / 2, new Vector2(16, 30), new Vector2(25, 40), testInfo);
        this.GenerateSpokeRegion( (this.tileGrid.Count / 2) + 20, this.tileGrid[0].Count / 2, new Vector2(16, 30), new Vector2(25, 40), testInfo);
        this.GenerateSpokeRegion((this.tileGrid.Count / 2), (this.tileGrid[0].Count / 2) + 50, new Vector2(16, 30), new Vector2(25, 40), testInfo);

        //Filling in all of the empty tiles with ocean tiles
        this.FillEmptyWithOcean();
    }


    //Loops through and generates all tiles in the grid
    private void GenerateGrid(int rows_, int cols_, bool addExtraTilesOnOffset_)
    {
        //Saves the given rows, columns, and if we should add extra tiles
        this.rows = rows_;
        this.cols = cols_;
        this.addExtraTileOnOffset = addExtraTilesOnOffset_;
        
        //Initializing the 2D list of tiles
        this.tileGrid = new List<List<GameObject>>(this.cols);
        
        //Vector to hold the starting position for grid generation in the top-left quadrant
        Vector3 startPos = new Vector3();
        //Finding the starting x coordinate using width and number of columns
        startPos.x = -(this.cols * this.tileWidth) / 2;
        //Finding the starting y coordinate using height and number of rows
        startPos.y = (this.rows * this.tileHeight) / 2;

        //Vector to hold the offset of the current tile when finding its correct location
        Vector3 offsetPos = new Vector3();

        //Bool that changes through every column loop. If true, offsets the current column up
        bool offsetCol = false;


        //Looping through each column
        for (int c = 0; c < this.cols; ++c)
        {
            //Creating a new list to hold all tiles in this column
            if (!this.addExtraTileOnOffset)
            {

                this.tileGrid.Add(new List<GameObject>(this.rows));
            }
            //If we add an extra tile on offset, there's one more tile in this row
            else
            {

                this.tileGrid.Add(new List<GameObject>(this.rows + 1));
            }

            //Looping through each row in the current column
            for(int r = 0; r < this.rows; ++r)
            {
                offsetPos.x = c * this.tileWidth; //Positive so that the grid is generated from left to right
                offsetPos.y = r * this.tileHeight * -1; //Negative so that the grid is generated downward
                
                //If the current column is offset, we add half the height of a tile
                if (offsetCol)
                {
                    offsetPos.y += this.tileHeight / 2;
                }
                
                //Creating a new tile and positioning it at the offset of the start position
                this.tileGrid[c].Add(Instantiate(this.landTile) as GameObject);
                this.tileGrid[c][r].transform.position = startPos + offsetPos;


                //If the parent isn't null, parents the current tile to it
                if (this.gridParent != null)
                {
                    this.tileGrid[c][r].transform.SetParent(this.gridParent.transform);
                }


                //Offset tile rows have an added tile at the end if the addExtraTileOnOffset is true
                if (this.addExtraTileOnOffset && offsetCol && (r + 1) == this.rows)
                {
                    offsetPos.y = ((r + 1) * this.tileHeight * -1) + (this.tileHeight / 2);

                    //Creating a new tile and positioning it at the offset of the start position
                    this.tileGrid[c][r + 1] = Instantiate(this.landTile) as GameObject;
                    this.tileGrid[c][r + 1].transform.position = startPos + offsetPos;

                    //If the parent isn't null, parents the current tile to it
                    if (this.gridParent != null)
                    {
                        this.tileGrid[c][r].transform.SetParent(this.gridParent.transform);
                    }
                }
            }
            
            //Changes the column offset for the next loop
            offsetCol = !offsetCol;
        }
    }


    //Connects all of the tiles to the tiles around them
    private void ConnectTiles()
    {
        //Bool that changes through every column loop. If true, the current column is offset down
        bool offsetCol = true;

        for (int c = 0; c < this.cols; ++c)
        {
            for(int r = 0; r < this.rows; ++r)
            {
                //Reference to the current tile
                LandTile currentTile;

                //If the current tile isn't null, we connect the path points
                if(this.tileGrid[c][r] != null)
                {
                    currentTile = this.tileGrid[c][r].GetComponent<LandTile>();
                    
                    //Connecting to the tile below the current
                    if(r + 1 < this.rows)
                    {
                        //Making sure the tile below exists
                        if(this.tileGrid[c][r+1] != null)
                        {
                            LandTile bottomTile = this.tileGrid[c][r + 1].GetComponent<LandTile>();
                            //Connecting the current tile's south (bottom) point to the north (top) point of the tile below, and vice versa
                            currentTile.southPoint.connectedPoints.Add(bottomTile.northPoint);
                            bottomTile.northPoint.connectedPoints.Add(currentTile.southPoint);
                        }
                    }


                    //If the current column is offset down
                    if(offsetCol)
                    {
                        //Making sure the next column exists
                        if (c + 1 < this.cols)
                        {
                            //The northeast tile is the same row, next column
                            LandTile northEastTile = this.tileGrid[c + 1][r].GetComponent<LandTile>();
                            //Connecting the current tile's northeast point to the southwest point in the next tile, and vice versa
                            currentTile.northEastPoint.connectedPoints.Add(northEastTile.southWestPoint);
                            northEastTile.southWestPoint.connectedPoints.Add(currentTile.northEastPoint);


                            //Making sure the row down exists
                            if(r + 1 < this.rows)
                            {
                                if(this.tileGrid[c+1][r+1] != null)
                                {
                                    //The southeast tile is 1 row down, next column
                                    LandTile southEastTile = this.tileGrid[c + 1][r + 1].GetComponent<LandTile>();
                                    //Connecting the current tile's southeast point to the northwest point to the next tile, and vice versa
                                    currentTile.southEastPoint.connectedPoints.Add(southEastTile.northWestPoint);
                                    southEastTile.northWestPoint.connectedPoints.Add(currentTile.southEastPoint);
                                }
                            }
                        }
                    }
                    //If the current column isn't offset
                    else
                    {

                        //Making sure the next column exists
                        if (c + 1 < this.cols)
                        {
                            //The southeast tile is the same row, next column
                            LandTile southEastTile = this.tileGrid[c + 1][r].GetComponent<LandTile>();
                            //Connecting the current tile's southeast point to the northwest point in the next tile, and vice versa
                            currentTile.southEastPoint.connectedPoints.Add(southEastTile.northWestPoint);
                            southEastTile.northWestPoint.connectedPoints.Add(currentTile.southEastPoint);


                            //Making sure the row up exists
                            if (r - 1 > -1)
                            {
                                //The northeast tile is 1 row up, next column
                                LandTile northEastTile = this.tileGrid[c + 1][r - 1].GetComponent<LandTile>();
                                //Connecting the current tile's northeast point to the southwest point of the next tile, and vice versa
                                currentTile.northEastPoint.connectedPoints.Add(northEastTile.southWestPoint);
                                northEastTile.southWestPoint.connectedPoints.Add(currentTile.northEastPoint);
                            }
                        }
                    }
                }
            }

            //Changes the offset after every loop
            offsetCol = !offsetCol;
        }
    }


    //Loops through all tiles in the map and fills in empty ones with ocean tiles
    private void FillEmptyWithOcean()
    {
        TileInfo oceanInfo = new TileInfo("Ocean of Dispair", LandType.Ocean, new Vector2(0, 0.1f), new Vector2(3, 5));

        //Looping through each tile in the tile grid
        for(int x = 0; x < this.tileGrid.Count; ++x)
        {
            for(int y = 0; y < this.tileGrid[0].Count; ++y)
            {
                //If the current tile is empty (has no designated land type)
                if(this.tileGrid[x][y].GetComponent<LandTile>().allTilePoints[0].type == LandType.Empty)
                {
                    //Setting the tile's type, movement cost, name, and color
                    this.tileGrid[x][y].GetComponent<LandTile>().allTilePoints[0].type = oceanInfo.type;
                    this.tileGrid[x][y].GetComponent<LandTile>().allTilePoints[0].movementCost = Mathf.RoundToInt(Random.Range(oceanInfo.movementCostMinMax.x, oceanInfo.movementCostMinMax.y));
                    this.tileGrid[x][y].GetComponent<LandTile>().allTilePoints[0].name = oceanInfo.regionName;
                    this.tileGrid[x][y].GetComponent<SpriteRenderer>().color = oceanInfo.landColor;
                }
            }
        }
    }


    //Creates a region of a specific zone type using "spokes" that extend outward from the given tile
    private void GenerateSpokeRegion(int startTileRow_, int startTileCol_, Vector2 spokeMinMax_, Vector2 spokeLengthMinMax_, TileInfo regionInfo_)
    {
        //Throwing an exception if the user inputs a starting tile that isn't on the grid
        if(startTileCol_ < 0 || startTileCol_ >= this.tileGrid.Count || startTileRow_ < 0 || startTileRow_ >= this.tileGrid[0].Count)
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
            if(startTileCol_ + xDiff >= this.cols)
            {
                xDiff = this.cols - startTileCol_ - 1;
            }
            else if(startTileCol_ + xDiff < 0)
            {
                xDiff = -startTileCol_;
            }

            if(startTileRow_ + yDiff >= this.rows)
            {
                yDiff = this.rows - startTileRow_ - 1;
            }
            else if(startTileRow_ + yDiff < 0)
            {
                yDiff = -startTileRow_;
            }

            //Adding this tile to the list of tiles in the region
            tilesInRegion.Add(new List<int> { startTileCol_ + xDiff, startTileRow_ + yDiff });
        }

        //The list of edge tiles around this region
        List<PathPoint> edgePoints = new List<PathPoint>();
        
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
            PathPoint startPoint = this.tileGrid[startX][startY].GetComponent<LandTile>().allTilePoints[0];
            PathPoint endPoint = this.tileGrid[endX][endY].GetComponent<LandTile>().allTilePoints[0];

            List<PathPoint> linePoints = this.GetComponent<PathfindingAlgorithms>().FindLineOfTiles(startPoint, endPoint);

            //Adding each edge point to the list of edge points
            foreach (PathPoint point in linePoints)
            {
                edgePoints.Add(point);
            }
        }

        //Finding the starting tile
        int x = tilesInRegion[0][0];
        int y = tilesInRegion[0][1];
        PathPoint startTile = this.tileGrid[x][y].GetComponent<LandTile>().allTilePoints[0];

        //Setting the tile info to all tiles in the region
        this.FillInRegionOfTiles(startTile, edgePoints, regionInfo_);
    }


    //Function called from GenerateSpokeRegion. Uses the same kind of algorithm as Breadth First Search to fill all tiles within a given region
    public void FillInRegionOfTiles(PathPoint startPoint_, List<PathPoint> edgeTiles_, TileInfo regionInfo_, bool onlyPaintEmpty = true)
    {
        //The list of path points that make up the frontier
        List<PathPoint> frontier = new List<PathPoint>();
        //Adding the starting point to the frontier and making sure its previous point is cleared
        frontier.Add(startPoint_);
        startPoint_.previousPoint = null;
        startPoint_.hasBeenChecked = false;

        //The list of path points that have already been visited
        List<PathPoint> visitedPoints = new List<PathPoint>();
        visitedPoints.Add(startPoint_);

        //Adding all edge tiles to the list of visited points so it cuts off the search
        foreach(PathPoint edge in edgeTiles_)
        {
            visitedPoints.Add(edge);
            edge.hasBeenChecked = true;
        }


        //Loop through each path point in the frontier until it's empty
        while(frontier.Count != 0)
        {
            //Getting the reference to the next point to check
            PathPoint currentPoint = frontier[0];

            //Looping through each path point that's connected to the current point
            foreach(PathPoint connection in currentPoint.connectedPoints)
            {
                //Making sure the point hasn't been visited yet
                if(!connection.hasBeenChecked)
                {
                    //Marking the connection as visited and adding it to the frontier
                    frontier.Add(connection);
                    visitedPoints.Add(connection);
                    connection.hasBeenChecked = true;
                }
            }

            //Removing the current tile from the frontier
            frontier.Remove(currentPoint);
        }


        //Looping through all of the tiles in our selection so that we can give them the region info
        foreach(PathPoint regionTile in visitedPoints)
        {
            //The tile is only given info if it's empty, or we don't care about only painting empty tiles
            if ( (onlyPaintEmpty && regionTile.type == LandType.Empty) || !onlyPaintEmpty)
            {
                regionTile.name = regionInfo_.regionName;
                regionTile.type = regionInfo_.type;
                regionTile.transform.parent.GetComponent<SpriteRenderer>().color = regionInfo_.landColor;

                //Generating the tile's height
                float newHeight = Random.Range(regionInfo_.heightMinMax.x, regionInfo_.heightMinMax.y);
                regionTile.transform.parent.transform.position += new Vector3(0, newHeight, 0);

                //Generating the tile's movement cost
                regionTile.movementCost = Mathf.RoundToInt(Random.Range(regionInfo_.movementCostMinMax.x, regionInfo_.movementCostMinMax.y));
            }

            //Clearing the pathfinding for each tile once we're finished with them
            regionTile.ClearPathfinding();
        }
    }
}


public class TileInfo
{
    //The name of the region that this tile is in
    public string regionName = "";

    //The environment type for the tile
    public LandType type = LandType.Empty;
    //The color of the tile based on the land type
    public Color landColor = Color.blue;

    //The elevation of this tile
    public Vector2 heightMinMax = new Vector2(0.1f, 0.2f);

    //The movement cost for traveling through this tile
    public Vector2 movementCostMinMax = new Vector2(1, 4);


    //Constructor for this class
    public TileInfo(string regionName_, LandType type_, Vector2 heightMinMax_, Vector2 movementCostMinMax_)
    {
        this.regionName = regionName_;
        this.type = type_;
        this.heightMinMax = heightMinMax_;
        this.movementCostMinMax = movementCostMinMax_;

        //Switch statement that sets the land color based on the type
        switch(this.type)
        {
            case LandType.Ocean:
                this.landColor = Color.blue;
                break;

            case LandType.River:
                this.landColor = new Color(0, 0.3f, 1, 1); //Light blue
                break;

            case LandType.Swamp:
                this.landColor = Color.cyan;
                break;

            case LandType.Grasslands:
                this.landColor = Color.green;
                break;

            case LandType.Forrest:
                this.landColor = new Color(0, 0.5f, 0, 1); //Dark green
                break;

            case LandType.Desert:
                this.landColor = Color.yellow;
                break;

            case LandType.Mountain:
                this.landColor = Color.grey;
                break;

            case LandType.Volcano:
                this.landColor = Color.red;
                break;

            default:
                this.landColor = Color.white;
                break;
        }
    }
}