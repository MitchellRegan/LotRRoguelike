using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTileGrid : MonoBehaviour
{
    //Game object used to test this script
    public GameObject shortTile;
    public GameObject medTile;
    public GameObject longTile;

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

    //Gradient used to color tiles based on their height
    public Gradient testColors;

    //The how zoomed in on the perlin noise we are. Lower numbers = more zoomed in, higher = more zoomed out
    [Range(0.1f, 10f)]
    public float perlinZoomX = 2;
    [Range(0.1f, 10f)]
    public float perlinZoomY = 2;

    //The range of tiles that are filled with water from the edges
    public Vector2 waterFillTopBottom = new Vector2(0, 2);
    public Vector2 waterFillLeftRight = new Vector2(0, 2);

    public GameObject pathTracer;
    


    //Called on initialization
    private void Awake()
    {
        //Finding the distance from corner to corner using trig (Cosine Theta = Adgecent Length / Hypotenuse Length. In this case, solving for the hypotenuse)
        this.tileWidth = this.tileHeight * ( Mathf.Cos(30 * Mathf.Deg2Rad));
        
        //Generates the grid
        this.GenerateGrid(this.rows, this.cols, this.addExtraTileOnOffset);
        //Assigns elevation to the grid
        //this.GeneratePerlinMapElevation();
        //Fills in the edges
        //this.FillEdgesWithOcean();

        //Connects the path points between tiles
        this.ConnectTiles();


        //Getting the test tile info
        TileInfo testInfo = new TileInfo("Testing", LandType.Grasslands, new Vector2(0.5f, 0.8f), new Vector2(3, 4));
        this.GenerateSpokeRegion(this.tileGrid.Count / 2, this.tileGrid[0].Count / 2, new Vector2(16, 30), new Vector2(25, 40), testInfo);
        this.GenerateSpokeRegion( (this.tileGrid.Count / 2) + 20, this.tileGrid[0].Count / 2, new Vector2(16, 30), new Vector2(25, 40), testInfo);
        this.GenerateSpokeRegion((this.tileGrid.Count / 2), (this.tileGrid[0].Count / 2) + 50, new Vector2(16, 30), new Vector2(25, 40), testInfo);

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
                this.tileGrid[c].Add(Instantiate(this.shortTile) as GameObject);
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
                    this.tileGrid[c][r + 1] = Instantiate(this.shortTile) as GameObject;
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


    //Assigns an elevation map to each tile using perlin noise
    private void GeneratePerlinMapElevation()
    {
        //A random offset so that games aren't the same (unless the seed is input)
        float offset = Random.value * 100000f;

        //Creating a new color for the sprites using perlin noise
        Color tileColor;
        
        //Looping through each column of tiles
        for (int c = 0; c < this.cols; ++c)
        {
            //Looping through each row in the current column
            for(int r = 0; r < this.rows; ++r)
            {
                //Checking to make sure that the current tile isn't null (adding extra tiles can do that)
                if(this.tileGrid[c][r] != null)
                {
                    //Making sure that the current tile has a sprite
                    if(this.tileGrid[c][r].GetComponent<SpriteRenderer>() != null)
                    {
                        //Setting the tile's color to a value on the gradient based on perlin noise
                        float xRand = (this.perlinZoomX * (c * 1.0f) / this.rows) + offset;// Random.Range(0f, 1.1f);
                        float yRand = (this.perlinZoomY * (r * 1.0f) / this.cols) + offset;// Random.Range(0f, 1.1f);
                        float perlin = Mathf.PerlinNoise(xRand, yRand);
                        tileColor = this.testColors.Evaluate(perlin);
                        this.tileGrid[c][r].GetComponent<SpriteRenderer>().color = tileColor;
                    }
                }
            }
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

            List<PathPoint> linePoints = this.FindLineOfTiles(startPoint, endPoint);

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


    //Function called from GenerateSpokeRegion. Fills in all tiles along a line with the given tile type
    private List<PathPoint> FindLineOfTiles(PathPoint startPoint_, PathPoint endPoint_)
    {
        //List of game objects that form the line that will be filled in
        List<PathPoint> pathLine = new List<PathPoint>();

        PathPoint currentPoint = startPoint_;

        //Looping through path points until we find the correct one
        while (currentPoint != endPoint_)
        {
            //Creating a var to hold the reference to the point connected to the current point that's closest to the end
            PathPoint closestPoint = currentPoint.connectedPoints[0];
            float closestPointDist = Vector3.Distance(closestPoint.transform.position, endPoint_.transform.position);

            //Looping through each connection to find the one that's closest to the end
            foreach (PathPoint connection in currentPoint.connectedPoints)
            {
                //Finding the distance between this connected point and the end point
                float connectionDist = Vector3.Distance(connection.transform.position, endPoint_.transform.position);

                //If this connected point is closer to the end than the current closest, this point becomes the new closest
                if(connectionDist < closestPointDist)
                {
                    closestPoint = connection;
                    closestPointDist = connectionDist;
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

                //Adding the current point to the list of visited points and removing it from the frontier
                frontier.Remove(currentPoint);
            }
        }


        //Looping through all path points in the list of visited points to clear their data
        foreach(PathPoint point in visitedPoints)
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