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

        this.GenerateSpokeRegion(this.tileGrid.Count / 2, this.tileGrid[0].Count / 2, new Vector2(3, 16), new Vector2(2, 14), new Vector2(15, 45));

        //Connects the path points between tiles
        this.ConnectTiles();
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


    //Fills in the outer edges of the map with ocean tiles
    private void FillEdgesWithOcean()
    {
        Color oceanColor = this.testColors.Evaluate(0);

        //Filling in the top row
        for(int t = 0; t < this.cols; ++t)
        {
            int tilesToFill = Mathf.RoundToInt( Random.Range(this.waterFillTopBottom.x, this.waterFillTopBottom.y) );
            for (int i = 0; i < tilesToFill; ++i)
            {
                this.tileGrid[t][0 + i].GetComponent<SpriteRenderer>().color = oceanColor;
            }
        }

        //Filling in the left column
        for(int l = 0; l < this.rows; ++l)
        {
            //Making sure the tile isn't null
            if (this.tileGrid[0][l] != null)
            {
                int tilesToFill = Mathf.RoundToInt(Random.Range(this.waterFillLeftRight.x, this.waterFillLeftRight.y));
                for (int i = 0; i < tilesToFill; ++i)
                {
                    this.tileGrid[0 + i][l].GetComponent<SpriteRenderer>().color = oceanColor;
                }
            }
        }

        //Filling in the right column
        for(int r = 0; r < this.rows; ++r)
        {
            //Making sure the tile isn't null
            if(this.tileGrid[this.cols - 1][r] != null)
            {
                int tilesToFill = Mathf.RoundToInt(Random.Range(this.waterFillLeftRight.x, this.waterFillLeftRight.y));
                for (int i = 0; i < tilesToFill; ++i)
                {
                    this.tileGrid[this.cols - 1 - i][r].GetComponent<SpriteRenderer>().color = oceanColor;
                }
            }
        }

        //Filling in the bottom row
        for(int b = 0; b < this.cols; ++b)
        {
            if(this.tileGrid[b][this.rows - 1] != null)
            {
                int tilesToFill = Mathf.RoundToInt(Random.Range(this.waterFillTopBottom.x, this.waterFillTopBottom.y));
                for (int i = 0; i < tilesToFill; ++i)
                {
                    this.tileGrid[b][this.rows - 1 - i].GetComponent<SpriteRenderer>().color = oceanColor;
                }
            }
        }
    }


    //Creates a region of a specific zone type using "spokes" that extend outward from the given tile
    private void GenerateSpokeRegion(int startTileRow_, int startTileCol_, Vector2 spokeMinMax_, Vector2 spokeLengthMinMax_, Vector2 spokeAngleMinMax_)
    {
        //Created a list of int arrays to hold all row/column locations for the tiles in this region
        List<List<int>> tilesInRegion = new List<List<int>>();
        tilesInRegion.Add(new List<int> { startTileCol_, startTileRow_}); 

        //Finding the number of spokes 
        int numberOfSpokes = Mathf.RoundToInt(Random.Range(spokeMinMax_.x, spokeMinMax_.y));


        //The total angle covered by all spokes. Used to offset each spoke from the previous one
        float totalAngle = 0;

        //Looping through to create each spoke
        for(int s = 0; s < numberOfSpokes; ++s)
        {
            //Finding the angle of the current spoke offset from the previous spoke
            float spokeAngle = Random.Range(spokeAngleMinMax_.x, spokeAngleMinMax_.y);
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
                xDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle) * spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle) * spokeLength);
            }
            else if(totalAngle <= 180)
            {
                //Tile is up and left
                xDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle) * -spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle) * spokeLength);
            }
            else if(totalAngle <= 270)
            {
                //Tile is down and left
                xDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle) * -spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle) * -spokeLength);
            }
            else
            {
                //Tile is down and right
                xDiff = Mathf.RoundToInt(Mathf.Sin(trigAngle) * spokeLength);
                yDiff = Mathf.RoundToInt(Mathf.Cos(trigAngle) * -spokeLength);
            }


            //Making sure the tile is within the bouderies of the map
            if(startTileCol_ + xDiff >= this.cols)
            {
                xDiff = this.cols - startTileCol_;
            }
            else if(startTileCol_ + xDiff < 0)
            {
                xDiff = -startTileCol_;
            }

            if(startTileRow_ + yDiff >= this.rows)
            {
                yDiff = this.rows - startTileRow_;
            }
            else if(startTileRow_ + yDiff < 0)
            {
                yDiff = -startTileRow_;
            }

            //Adding this tile to the list of tiles in the region
            tilesInRegion.Add(new List<int> { startTileCol_ + xDiff, startTileRow_ + yDiff });
        }


        //Looping through and colorizing the region
        foreach(List<int> tile in tilesInRegion)
        {
            int xCoord = tile[0];
            int yCoord = tile[1];
            this.tileGrid[xCoord][yCoord].GetComponent<SpriteRenderer>().color = Color.red;

            //Now that we have all of the points, the lines between each point need to be colorized.
            //Use get the slope of the line between each point and the next and fill in everything across that line?
            
        }

        //Connecting the lines between points
        for(int p = 1; p < tilesInRegion.Count; ++p)
        {
            //Getting the coordinates of the tile at the start of the line
            int startX = tilesInRegion[p][0];
            int startY = tilesInRegion[p][1];
            //Getting the coordinates of the tile at the end of the line
            int endX;
            int endY;

            //If this is the last tile in the list, it needs to connect to the first
            if(p + 1 >= tilesInRegion.Count)
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
            this.FillInLineOfTiles(startY, startX, endY, endX, Color.blue);

            this.tileGrid[startX][startY].GetComponent<SpriteRenderer>().color = Color.green;
            this.tileGrid[endX][endY].GetComponent<SpriteRenderer>().color = Color.green;
        }

        //Coloring the starting tile
        int x = tilesInRegion[0][0];
        int y = tilesInRegion[0][1];
        this.tileGrid[x][y].GetComponent<SpriteRenderer>().color = Color.yellow;
    }


    //Function called from GenerateSpokeRegion. Fills in all tiles along a line with the given tile type
    private void FillInLineOfTiles(int startTileRow_, int startTileCol_, int endTileRow_, int endTileCol_, Color tileColor_)
    {
        //Finding out if the starting tile is in a column that's offset
        bool isOffset = false;
        if(startTileCol_ % 2 == 0)
        {
            isOffset = true;
        }

        //Finding the slope of the line between the tiles
        int xDiff = endTileCol_ - startTileCol_;
        int yDiff = endTileRow_ - startTileRow_;
        float slope = 1;

        //Making sure the x difference is greater than 0 so we don't have a "divided by 0" error
        if (xDiff != 0)
        {
            slope = (yDiff * 1f) / (xDiff * 1f);
        }


        //If the slope is 0 (horizontal)
        if (yDiff == 0)
        {
            //Determining if the row is generated right (positive) or left (negative)
            int direction = 1;
            if (xDiff < 0)
            {
                direction = -1;
            }

            //Creating a loop to set a row of tiles
            for (int x = 0; x < Mathf.Abs(xDiff); ++x)
            {
                //Making sure the tile we're editing is within the tile grid
                if(startTileRow_ + (xDiff * direction) > -1 && startTileRow_ + (xDiff * direction) <= this.tileGrid[0].Count)
                {
                    this.tileGrid[startTileCol_][startTileRow_ + (x * direction)].GetComponent<SpriteRenderer>().color = tileColor_;
                }
                //If the tile is outside the grid, the loop is broken
                else
                {
                    break;
                }
            }
        }
        //If the slope is undefined (vertical, x is 0)
        else if (xDiff == 0)
        {
            //Determining if the column is generated up (positive) or down (negative)
            int direction = -1;
            if (yDiff < 0)
            {
                direction = 1;
            }

            //Creating a loop to set a column of tiles
            for (int y = 0; y < Mathf.Abs(yDiff); ++y)
            {
                //Making sure the tile we're editing is within the tile grid
                if(startTileCol_ + (yDiff * direction) > -1 && startTileCol_ + (yDiff * direction) <= this.tileGrid.Count)
                {
                    this.tileGrid[startTileCol_ + (y * direction)][startTileRow_].GetComponent<SpriteRenderer>().color = tileColor_;
                }
                //If the tile is outside the grid, the loop is broken
                else
                {
                    break;
                }
            }
        }
        //If the slope is at a 45 degree angle
        else if(slope == 0.5f)
        {
            //Int to track the increase in 
            int y = 0;
            if(isOffset)
            {

            }

            for(int x = 1; x < xDiff; ++x)
            {

                //Only increases the y
                if(isOffset)
                {
                    ++y;
                }

                //Switches the offset for the next column
                isOffset = !isOffset;
            }
        }
        //If the slope is at an angle
        else
        {
            //Determining if the direction the rows and columns are generated
            int xDirection = 1;
            int yDirection = -1;
            if(xDiff < 0)
            {
                xDirection = -1;
            }
            if(yDiff < 0)
            {
                yDirection = 1;
            }


            //Looping through each tile in the height of the line
            for (int x = 0, y = 0; y < Mathf.Abs(yDiff); ++y)
            {
                //When the height is greater than the slope, the width increases
                if (y >= slope)
                {
                    ++x;
                }

                this.tileGrid[startTileCol_ + (y * yDirection)][startTileRow_ + (x * xDirection)].GetComponent<SpriteRenderer>().color = tileColor_;
            }
        }
    }
}
