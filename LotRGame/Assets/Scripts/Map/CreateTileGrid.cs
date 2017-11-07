using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingAlgorithms))]
public class CreateTileGrid : MonoBehaviour
{
    //Static reference to this tile grid
    public static CreateTileGrid globalReference;

    //The number of columns of hexes this grid has
    public int cols = 6;
    //The number of rows of hexes there are in each column
    public int rows = 6;

    //The height of each tile
    public float tileHeight = 20;
    //The width of each tile. This value is generated on Awake using the tile height given
    private float tileWidth = 1;

    //The hexagon model prefab that's used for every tile
    public MeshRenderer hexMesh;

    //2 dimensional array of tiles that's generated (cols, rows)
    public List<List<TileInfo>> tileGrid;

    //The number of tiles that are visible at a given time
    [Range(2, 25)]
    public int visibilityRange = 7;

    //The list of all tiles that are currently visible
    public Dictionary<TileInfo, GameObject> visibleTileObjects;

    //Prefab for the RegionInfo of the ocean
    public RegionInfo oceanRegion;

    [Space(8)]

    //List of prefabs for different very easy regions
    public List<RegionInfo> veryEasyRegions;
    //The maximum number of splits for the very easy band
    public Vector2 minMaxVeryEasySplits = new Vector2(1, 2);

    //List of prefabs for different easy regions
    public List<RegionInfo> easyRegions;
    //The maximum number of splits for the easy band
    public Vector2 minMaxEasySplits = new Vector2(1, 3);

    //List of prefabs for different medium regions
    public List<RegionInfo> mediumRegions;
    //The maximum number of splits for the medium band
    public Vector2 minMaxMediumSplits = new Vector2(3, 5);

    //List of prefabs for different hard regions
    public List<RegionInfo> hardRegions;
    //The maximum number of splits for the hard band
    public Vector2 minMaxHardSplits = new Vector2(2, 5);

    //List of prefabs for different very hard regions
    public List<RegionInfo> veryHardRegions;
    //The maximum number of splits for the very hard band
    public Vector2 minMaxVeryHardSplits = new Vector2(1,2);

    //List of prefabs for different final regions
    public List<RegionInfo> finalRegions;
    //The maximum number of splits for the final band
    private Vector2 minMaxFinalSplits = new Vector2(1,1);

    [Space(8)]

    //The maximum spread for the region bands
    public float maxBandAngleSpread = 90;

    //The maximum percent of variance for the band regions
    [Range(0, 0.5f)]
    public float bandRegionPercentVariance = 0.2f;

    [Space(8)]

    //Prefab for the group that the player characters are added to
    public GameObject partyGroup1Prefab;

    //Empty character prefab to use while testing
    public GameObject testCharacter;
    public GameObject testCharacter2;

    //Enemy encounter for testing
    public GameObject testEnemyEncounter;

    [Space(8)]

    //The list of tiles where region cities are
    [HideInInspector]
    public List<TileInfo> cityTiles;

    //The radius around the region center that the city tiles can be within
    public int cityRadiusFromCenter = 8;

    //The min and max percent that a region can step based on the number of tiles along its edge
    public Vector2 minMaxStepPercent = new Vector2(0.1f, 0.3f);



    //Called on initialization
    private void Awake()
    {
        //Setting the static reference
        if(globalReference == null)
        {
            globalReference = this;
        }
        else
        {
            Destroy(this);
        }

        //Finding the width of the tiles based on the height given and the ratio between a hex's height and width
        this.tileWidth = this.tileHeight * 1.1547f;
        this.tileWidth -= (this.tileWidth - this.tileHeight) * 1.9f;

        //Initializing our list of visible tile objects
        this.visibleTileObjects = new Dictionary<TileInfo, GameObject>();

        //If we're starting a new game, we need to generate the map first
        if(GameData.globalReference.loadType == GameData.levelLoadType.GenerateNewLevel)
        {
            this.StartMapCreation();
        }
    }


    //Public function called from awake. Creates the correct map based on the game difficulty
    public void StartMapCreation()
    {
        //Setting the number of columns and rows based on the game difficulty map sizes
        switch (GameData.globalReference.currentDifficulty)
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

        //Generates the grid of tiles
        this.GenerateGrid(this.rows, this.cols);

        //Connects the path points between tiles
        this.ConnectTiles();

        //Creates the correct map for the game difficulty
        this.ImprovedMapGeneration();

        //Getting all of the tiles where cities will be
        this.cityTiles = this.FindCityTiles();

        //Setting all of the map locations for each region, like cities and dungeons
        this.CreateMapLocations();

        //Randomizes the regions so that they grow in different directions
        //this.ExpandRegionBoarders();

        //Creating the map texture
        this.CreateMapTexture();
    }


    //Enum for the direction from the center that the zones are created from
    private enum MapDirections { North, South, East, West };
    //Function that creates a map for normal games
    private void ImprovedMapGeneration()
    {
        //Finding out which edge of the map the player starting zone is created
        MapDirections startZoneDirection = MapDirections.North;
        int startEdge = Random.Range(1, 4);
        switch(startEdge)
        {
            case 1:
                startZoneDirection = MapDirections.North;
                break;
            case 2:
                startZoneDirection = MapDirections.South;
                break;
            case 3:
                startZoneDirection = MapDirections.West;
                break;
            case 4:
                startZoneDirection = MapDirections.East;
                break;
        }
        //Finding which fifth of the map the start zone will be along
        int startZoneSection = Random.Range(0, 4);

        //Setting the end zone along the opposite edge of the map that the start zone is
        MapDirections endZoneDirection = MapDirections.South;
        switch(startZoneDirection)
        {
            case MapDirections.North:
                endZoneDirection = MapDirections.South;
                break;
            case MapDirections.South:
                endZoneDirection = MapDirections.North;
                break;
            case MapDirections.West:
                endZoneDirection = MapDirections.East;
                break;
            case MapDirections.East:
                endZoneDirection = MapDirections.West;
                break;
        }

        //Finding which fifth of the map the end zone will be along. It's within a fifth of the start zone section
        int endZoneSection = 0;
        if(startZoneSection == 0)
        {
            endZoneSection = Random.Range(0, 1);
        }
        else if(startZoneSection == 4)
        {
            endZoneSection = Random.Range(0, 1);
        }
        else
        {
            endZoneSection = startZoneSection + Random.Range(-1, 1);
        }
        
        //Finding the Starting tile based on the starting edge and fifth of the map
        TileInfo startTile = null;
        Vector2 startRowRanges = new Vector2();
        Vector2 startColRanges = new Vector2();
        int startRow = 0;
        int startCol = 0;
        switch (startZoneDirection)
        {
            //Along the top of the map
            case MapDirections.North:
                //Finding the rows that it can be between
                startRowRanges.x = 0;
                startRowRanges.y = this.tileGrid[0].Count / 5;
                //Finding the columns that it can be between
                startColRanges.x = (this.tileGrid.Count / 5) * startZoneSection;
                startColRanges.y = (this.tileGrid.Count / 5) * (startZoneSection + 1);

                //Finding a random row and column based on the ranges
                startCol = Mathf.RoundToInt(Random.Range(startColRanges.x, startColRanges.y));
                startRow = Mathf.RoundToInt(Random.Range(startRowRanges.x, startRowRanges.y));

                //Setting the start tile using the row and column
                startTile = this.tileGrid[startCol][startRow];
                break;

            //Along the bottom of the map
            case MapDirections.South:
                //Finding the rows that it can be between
                startRowRanges.x = (this.tileGrid[0].Count / 5) * 4;
                startRowRanges.y = this.tileGrid[0].Count;
                //Finding the columns that it can be between
                startColRanges.x = (this.tileGrid.Count / 5) * startZoneSection;
                startColRanges.y = (this.tileGrid.Count / 5) * (startZoneSection + 1);

                //Finding a random row and column based on the ranges
                startCol = Mathf.RoundToInt(Random.Range(startColRanges.x, startColRanges.y));
                startRow = Mathf.RoundToInt(Random.Range(startRowRanges.x, startRowRanges.y));

                //Setting the start tile using the row and column
                startTile = this.tileGrid[startCol][startRow];
                break;

            //Along the left side of the map
            case MapDirections.West:
                //Finding the rows that it can be between
                startRowRanges.x = (this.tileGrid[0].Count / 5) * startZoneSection;
                startRowRanges.y = (this.tileGrid[0].Count / 5) * (startZoneSection + 1);
                //Finding the columns that it can be between
                startColRanges.x = 0;
                startColRanges.y = this.tileGrid.Count / 5;

                //Finding a random row and column based on the ranges
                startCol = Mathf.RoundToInt(Random.Range(startColRanges.x, startColRanges.y));
                startRow = Mathf.RoundToInt(Random.Range(startRowRanges.x, startRowRanges.y));

                //Setting the start tile using the row and column
                startTile = this.tileGrid[startCol][startRow];
                break;

            //Along the right side of the map
            case MapDirections.East:
                //Finding the rows that it can be between
                startRowRanges.x = (this.tileGrid[0].Count / 5) * startZoneSection;
                startRowRanges.y = (this.tileGrid[0].Count / 5) * (startZoneSection + 1);
                //Finding the columns that it can be between
                startColRanges.x = (this.tileGrid.Count / 5) * 4;
                startColRanges.y = this.tileGrid.Count;

                //Finding a random row and column based on the ranges
                startCol = Mathf.RoundToInt(Random.Range(startColRanges.x, startColRanges.y));
                startRow = Mathf.RoundToInt(Random.Range(startRowRanges.x, startRowRanges.y));

                //Setting the start tile using the row and column
                startTile = this.tileGrid[startCol][startRow];
                break;
        }
        
        //Finding the Starting tile based on the starting edge and fifth of the map
        TileInfo endTile = null;
        Vector2 endRowRanges = new Vector2();
        Vector2 endColRanges = new Vector2();
        int endRow = 0;
        int endCol = 0;
        switch (endZoneDirection)
        {
            //Along the top of the map
            case MapDirections.North:
                //Finding the rows that it can be between
                endRowRanges.x = 0;
                endRowRanges.y = this.tileGrid[0].Count / 5;
                //Finding the columns that it can be between
                endColRanges.x = (this.tileGrid.Count / 5) * endZoneSection;
                endColRanges.y = (this.tileGrid.Count / 5) * (endZoneSection + 1);

                //Finding a random row and column based on the ranges
                endCol = Mathf.RoundToInt(Random.Range(endColRanges.x, endColRanges.y));
                endRow = Mathf.RoundToInt(Random.Range(endRowRanges.x, endRowRanges.y));

                //Making sure the endcol and endrow are within the bounds of the tile grid
                if (endCol < 0)
                {
                    endCol = 0;
                }
                else if (endCol >= this.tileGrid.Count)
                {
                    endCol = this.tileGrid.Count - 1;
                }
                if (endRow < 0)
                {
                    endRow = 0;
                }
                else if (endRow >= this.tileGrid[0].Count)
                {
                    endRow = this.tileGrid[0].Count - 1;
                }

                //Setting the start tile using the row and column
                endTile = this.tileGrid[endCol][endRow];
                break;

            //Along the bottom of the map
            case MapDirections.South:
                //Finding the rows that it can be between
                endRowRanges.x = (this.tileGrid[0].Count / 5) * 4;
                endRowRanges.y = this.tileGrid[0].Count;
                //Finding the columns that it can be between
                endColRanges.x = (this.tileGrid.Count / 5) * endZoneSection;
                endColRanges.y = (this.tileGrid.Count / 5) * (endZoneSection + 1);

                //Finding a random row and column based on the ranges
                endCol = Mathf.RoundToInt(Random.Range(endColRanges.x, endColRanges.y));
                endRow = Mathf.RoundToInt(Random.Range(endRowRanges.x, endRowRanges.y));

                //Making sure the endcol and endrow are within the bounds of the tile grid
                if(endCol < 0)
                {
                    endCol = 0;
                }
                else if(endCol >= this.tileGrid.Count)
                {
                    endCol = this.tileGrid.Count - 1;
                }
                if(endRow < 0)
                {
                    endRow = 0;
                }
                else if(endRow >= this.tileGrid[0].Count)
                {
                    endRow = this.tileGrid[0].Count - 1;
                }

                //Setting the start tile using the row and column
                endTile = this.tileGrid[endCol][endRow];
                break;

            //Along the left side of the map
            case MapDirections.West:
                //Finding the rows that it can be between
                endRowRanges.x = (this.tileGrid[0].Count / 5) * endZoneSection;
                endRowRanges.y = (this.tileGrid[0].Count / 5) * (endZoneSection + 1);
                //Finding the columns that it can be between
                endColRanges.x = 0;
                endColRanges.y = this.tileGrid.Count / 5;

                //Finding a random row and column based on the ranges
                endCol = Mathf.RoundToInt(Random.Range(endColRanges.x, endColRanges.y));
                endRow = Mathf.RoundToInt(Random.Range(endRowRanges.x, endRowRanges.y));

                //Making sure the endcol and endrow are within the bounds of the tile grid
                if (endCol < 0)
                {
                    endCol = 0;
                }
                else if (endCol >= this.tileGrid.Count)
                {
                    endCol = this.tileGrid.Count - 1;
                }
                if (endRow < 0)
                {
                    endRow = 0;
                }
                else if (endRow >= this.tileGrid[0].Count)
                {
                    endRow = this.tileGrid[0].Count - 1;
                }

                //Setting the start tile using the row and column
                endTile = this.tileGrid[endCol][endRow];
                break;

            //Along the right side of the map
            case MapDirections.East:
                //Finding the rows that it can be between
                endRowRanges.x = (this.tileGrid[0].Count / 5) * endZoneSection;
                endRowRanges.y = (this.tileGrid[0].Count / 5) * (endZoneSection + 1);
                //Finding the columns that it can be between
                endColRanges.x = (this.tileGrid.Count / 5) * 4;
                endColRanges.y = this.tileGrid.Count;

                //Finding a random row and column based on the ranges
                endCol = Mathf.RoundToInt(Random.Range(endColRanges.x, endColRanges.y));
                endRow = Mathf.RoundToInt(Random.Range(endRowRanges.x, endRowRanges.y));

                //Making sure the endcol and endrow are within the bounds of the tile grid
                if (endCol < 0)
                {
                    endCol = 0;
                }
                else if (endCol >= this.tileGrid.Count)
                {
                    endCol = this.tileGrid.Count - 1;
                }
                if (endRow < 0)
                {
                    endRow = 0;
                }
                else if (endRow >= this.tileGrid[0].Count)
                {
                    endRow = this.tileGrid[0].Count - 1;
                }

                //Setting the start tile using the row and column
                endTile = this.tileGrid[endCol][endRow];
                break;
        }

        //Creating list for all of the tile difficulty bands
        List<List<TileInfo>> veryEasyBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> easyBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> mediumBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> hardBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> veryHardBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> finalBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        
        
        //Finding the tile in the corner opposite of the end tile
        TileInfo startCorner;
        int startRowCorner = 0;
        int startColCorner = 0;
        //If the end tile is in the first 5th of the map rows
        if(endRow <= this.tileGrid[0].Count / 5)
        {
            //The opposite row is the last one
            startRowCorner = this.tileGrid[0].Count - 1;
        }
        //If the end tile is in the last 5th of the map rows
        else if(endRow >= this.tileGrid[0].Count - (this.tileGrid[0].Count / 5))
        {
            //The opposite row is the first one
            startRowCorner = 0;
        }
        //If the end tile is in the middle of the map rows
        else
        {
            //The opposite row is the middle one
            startRowCorner = this.tileGrid[0].Count / 2;
        }

        //If the end tile is in the first 5th of the map cols
        if(endCol <= this.tileGrid.Count / 5)
        {
            //The opposite col is the last one
            startColCorner = this.tileGrid.Count - 1;
        }
        //If the end tile is in the last 5th of the map cols
        else if(endCol >= this.tileGrid.Count - (this.tileGrid.Count/ 5))
        {
            //The opposite col is the first one
            startColCorner = 0;
        }
        //If the end tile is in the middle of the map cols
        else
        {
            //The opposite col is the middle one
            startColCorner = this.tileGrid.Count / 2;
        }

        //Getting the tile that's at the opposite end of the end tile map
        startCorner = this.tileGrid[startColCorner][startRowCorner];

        //Finding the tile in the corner opposite of the start tile
        TileInfo endCorner;
        int endRowCorner = 0;
        int endColCorner = 0;
        //If the end tile is in the first 5th of the map rows
        if (endRow <= this.tileGrid[0].Count / 5)
        {
            //The row is the first one
            endRowCorner = 0;
        }
        //If the end tile is in the last 5th of the map rows
        else if (endRow >= this.tileGrid[0].Count - (this.tileGrid[0].Count / 5))
        {
            //The row is the first one
            endRowCorner = this.tileGrid[0].Count - 1;
        }
        //If the end tile is in the middle of the map rows
        else
        {
            //The row is the middle one
            endRowCorner = this.tileGrid[0].Count / 2;
        }

        //If the end tile is in the first 5th of the map cols
        if (endCol <= this.tileGrid.Count / 5)
        {
            //The col is the first one
            endColCorner = 0;
        }
        //If the end tile is in the last 5th of the map cols
        else if (startCol >= this.tileGrid.Count - (this.tileGrid.Count / 5))
        {
            //The col is the first one
            endColCorner = this.tileGrid.Count - 1;
        }
        //If the end tile is in the middle of the map cols
        else
        {
            //The col is the middle one
            endColCorner = this.tileGrid.Count / 2;
        }

        //Getting the tile that's at the opposite end of the start tile map
        endCorner = this.tileGrid[endColCorner][endRowCorner];

        //Finding the distance from the end zone corner to the start zone corner of the map
        float totalDistStartToEnd = Vector3.Distance(startCorner.tilePosition, endCorner.tilePosition);

        //Creating a radius for each band outward from the end zone
        float veryEasyRadius = totalDistStartToEnd;
        float easyRadius = (totalDistStartToEnd / 5) + (totalDistStartToEnd / 7) * 4;
        float mediumRadius = (totalDistStartToEnd / 5) + (totalDistStartToEnd / 7) * 3;//4/5
        float hardRadius = (totalDistStartToEnd / 5) + (totalDistStartToEnd / 7) * 2;//3/5
        float veryHardRadius = (totalDistStartToEnd / 5) + (totalDistStartToEnd / 7);//(2/5)
        float finalRadius = totalDistStartToEnd / 5;

        //Looping through every tile in the grid to find out what difficulty band they belong in
        for (int c = 0; c < this.tileGrid.Count; ++c)
        {
            for (int r = 0; r < this.tileGrid[0].Count; ++r)
            {
                //Finding the distance the current tile is from the end zone
                float currentTileDist = Vector3.Distance(this.tileGrid[c][r].tilePosition, endCorner.tilePosition);

                //Determining which radius the tile is within and adding it to that list of tiles
                if (currentTileDist < finalRadius)
                {
                    finalBand[0].Add(this.tileGrid[c][r]);
                }
                else if (currentTileDist < veryHardRadius)
                {
                    veryHardBand[0].Add(this.tileGrid[c][r]);
                }
                else if (currentTileDist < hardRadius)
                {
                    hardBand[0].Add(this.tileGrid[c][r]);
                }
                else if (currentTileDist < mediumRadius)
                {
                    mediumBand[0].Add(this.tileGrid[c][r]);
                }
                else if (currentTileDist < easyRadius)
                {
                    easyBand[0].Add(this.tileGrid[c][r]);
                }
                else
                {
                    veryEasyBand[0].Add(this.tileGrid[c][r]);
                }
            }
        }
        

        //Splitting the very easy difficulty band
        this.SplitDifficultyBands(veryEasyBand, this.veryEasyRegions, this.minMaxVeryEasySplits, startCorner, endCorner);
        //Splitting the easy difficulty band
        this.SplitDifficultyBands(easyBand, this.easyRegions, this.minMaxEasySplits, startCorner, endCorner);
        //Splitting the medium difficulty band
        this.SplitDifficultyBands(mediumBand, this.mediumRegions, this.minMaxMediumSplits, startCorner, endCorner);
        //Splitting the hard difficulty band
        this.SplitDifficultyBands(hardBand, this.hardRegions, this.minMaxHardSplits, startCorner, endCorner);
        //Splitting the very hard difficulty band
        this.SplitDifficultyBands(veryHardBand, this.veryHardRegions, this.minMaxVeryHardSplits, startCorner, endCorner);
        //Splitting the final difficulty band
        this.SplitDifficultyBands(finalBand, this.finalRegions, this.minMaxFinalSplits, startCorner, endCorner);

        //Once the map is created, we set the player on the starting tile
        this.SetPlayerPartyPosition(startTile);
    }


    //Function called from ImprovedMapGeneration to split all of the difficulty bands into different regions
    private void SplitDifficultyBands(List<List<TileInfo>> difficultyBand_, List<RegionInfo> difficultyRegions_, Vector2 numberOfSplitsMinMax_, TileInfo startTile_, TileInfo endTile_)
    {
        //Finding the number of splits we need in the band
        int splits = Mathf.RoundToInt(Random.Range(numberOfSplitsMinMax_.x, numberOfSplitsMinMax_.y));

        //Finding the angle from the end tile to the start tile
        float endStartAngle = Mathf.Atan2(startTile_.tilePosition.z - endTile_.tilePosition.z, startTile_.tilePosition.x - endTile_.tilePosition.x);
        endStartAngle *= Mathf.Rad2Deg;

        //Normalizing the endStartAngle so that it's between 0-360, not -180 and 180
        if(endStartAngle < 0)
        {
            endStartAngle += 360;
        }
        
        //Creating a list of all the angles where each split angle is
        List<float> splitAngles = new List<float>();
        for(int s = 0; s < splits; ++s)
        {
            //Finding the cut of the max angle spread that this split will be at
            float baseAngle = this.maxBandAngleSpread / (splits + 1);
            baseAngle *= s + 1;
            
            //Offsetting the split based on the angle between the start and end tiles
            baseAngle += endStartAngle - (this.maxBandAngleSpread / 2);
            
            //Adding variance to the angle based on a percent of the base angle
            float variance = Random.Range((this.maxBandAngleSpread / splits) * -(this.bandRegionPercentVariance / 2), (this.maxBandAngleSpread / splits) * (this.bandRegionPercentVariance / 2));

            baseAngle += variance;

            //Normalizing the base angle so that it's within 0-360
            if (baseAngle < 0)
            {
                baseAngle += 360;
            }

            //Setting the current split to the split angle list
            splitAngles.Add(baseAngle);
        }
        
        //Duplicating the list of regions in the difficulty band so we can modify it
        List<RegionInfo> regionDup = difficultyRegions_;

        //Creating a list of each region in this difficulty band
        List<RegionInfo> bandRegions = new List<RegionInfo>();
        for(int r = 0; r < splits + 1; ++r)
        {
            //Getting a random region from the list of difficulty regions
            int regionIndex = Random.Range(0, regionDup.Count - 1);

            //Adding the region to the band region list
            bandRegions.Add(regionDup[regionIndex]);

            //Removing the current region from the list of difficulty regions so it doesn't show up multiple times
            if (regionDup.Count > 1)
            {
                regionDup.RemoveAt(regionIndex);
            }
        }

        
        //Looping through each tile in the given difficulty band
        foreach (TileInfo tile in difficultyBand_[0])
        {
            //Finding the angle that the current tile is from the end point
            float angleDiff = Mathf.Atan2(tile.tilePosition.z - endTile_.tilePosition.z, tile.tilePosition.x - endTile_.tilePosition.x);
            angleDiff *= Mathf.Rad2Deg;

            //Normalizing the angleDiff so that it's between 0-360, not -180 and 180
            if (angleDiff < 0)
            {
                angleDiff += 360;
            }

            //Finding which split the tile is within
            for (int t = 0; t < splits; ++t)
            {
                //Checking if the current tile's angle is within the current split
                if(angleDiff < splitAngles[t])
                {
                    //We set the tile's info using the region with the same index
                    tile.SetTileBasedOnRegion(bandRegions[t]);
                    t = splits;
                }
                //If the tile isn't within the current split and this is the last split
                else if(t + 1 == splits)
                {
                    //We set the tile's info using the region with the index of the last region
                    tile.SetTileBasedOnRegion(bandRegions[t+1]);
                }
            }
        }
    }


    //Loops through and generates all tiles in the grid
    private void GenerateGrid(int rows_, int cols_)
    {
        //Saves the given rows, columns, and if we should add extra tiles
        this.rows = rows_;
        this.cols = cols_;
        
        //Initializing the 2D list of tiles
        this.tileGrid = new List<List<TileInfo>>(this.cols);
        //Vector to hold the starting position for grid generation in the top-left quadrant
        Vector3 startPos = new Vector3();
        //Finding the starting x coordinate using width and number of columns
        startPos.x = -(this.cols * this.tileWidth) / 2;
        //Finding the starting y coordinate using height and number of rows
        startPos.z = (this.rows * this.tileHeight) / 2;

        //Vector to hold the offset of the current tile when finding its correct location
        Vector3 offsetPos = new Vector3();

        //Bool that changes through every column loop. If true, offsets the current column up
        bool offsetCol = false;
        
        //Looping through each column
        for (int c = 0; c < this.cols; ++c)
        {
            //Creating a new list to hold all tiles in this column
            this.tileGrid.Add(new List<TileInfo>(this.rows));

            //Looping through each row in the current column
            for (int r = 0; r < this.rows; ++r)
            {
                offsetPos.x = c * this.tileWidth; //Positive so that the grid is generated from left to right
                offsetPos.z = r * this.tileHeight * -1; //Negative so that the grid is generated downward
                
                //If the current column is offset, we add half the height of a tile
                if (offsetCol)
                {
                    offsetPos.z += this.tileHeight / 2;
                }
                //Creating a new tile and positioning it at the offset of the start position
                this.tileGrid[c].Add(new TileInfo(this.oceanRegion));
                this.tileGrid[c][r].tilePosition = startPos + offsetPos;
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
                TileInfo currentTile;

                //If the current tile isn't null, we connect the path points
                if(this.tileGrid[c][r] != null)
                {
                    currentTile = this.tileGrid[c][r];
                    
                    //Connecting to the tile below the current
                    if(r + 1 < this.rows)
                    {
                        //Making sure the tile below exists
                        if(this.tileGrid[c][r+1] != null)
                        {
                            TileInfo bottomTile = this.tileGrid[c][r + 1];
                            //Connecting the current tile's south (bottom) point to the north (top) point of the tile below, and vice versa
                            currentTile.connectedTiles[0] = bottomTile;
                            bottomTile.connectedTiles[3] = currentTile;
                        }
                    }


                    //If the current column is offset down
                    if(offsetCol)
                    {
                        //Making sure the next column exists
                        if (c + 1 < this.cols)
                        {
                            //The northeast tile is the same row, next column
                            TileInfo northEastTile = this.tileGrid[c + 1][r];
                            //Connecting the current tile's northeast point to the southwest point in the next tile, and vice versa
                            currentTile.connectedTiles[1] = northEastTile;
                            northEastTile.connectedTiles[4] = currentTile;


                            //Making sure the row down exists
                            if(r + 1 < this.rows)
                            {
                                if(this.tileGrid[c+1][r+1] != null)
                                {
                                    //The southeast tile is 1 row down, next column
                                    TileInfo southEastTile = this.tileGrid[c + 1][r + 1];
                                    //Connecting the current tile's southeast point to the northwest point to the next tile, and vice versa
                                    currentTile.connectedTiles[2] = southEastTile;
                                    southEastTile.connectedTiles[5] = currentTile;
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
                            TileInfo southEastTile = this.tileGrid[c + 1][r];
                            //Connecting the current tile's southeast point to the northwest point in the next tile, and vice versa
                            currentTile.connectedTiles[2] = southEastTile;
                            southEastTile.connectedTiles[5] = currentTile;


                            //Making sure the row up exists
                            if (r - 1 > -1)
                            {
                                //The northeast tile is 1 row up, next column
                                TileInfo northEastTile = this.tileGrid[c + 1][r - 1];
                                //Connecting the current tile's northeast point to the southwest point of the next tile, and vice versa
                                currentTile.connectedTiles[1] = northEastTile;
                                northEastTile.connectedTiles[4] = currentTile;
                            }
                        }
                    }
                }
            }

            //Changes the offset after every loop
            offsetCol = !offsetCol;
        }
    }


    //Function called from StartMapCreation to find all of the tile locations for cities
    private List<TileInfo> FindCityTiles()
    {
        //The list of tiles where cities will be spawned
        List<TileInfo> cityTiles = new List<TileInfo>();
        //The list of tiles that will be used as starting points for pathfinding
        List<TileInfo> pathfindingStartingPoints = new List<TileInfo>();
        //Adding the first tile to the starting points list
        pathfindingStartingPoints.Add(this.tileGrid[0][0]);

        //Looping through all of the tiles to get starting points
        for(int c = 0; c < this.tileGrid.Count; ++c)
        {
            for(int r = 0; r < this.tileGrid[0].Count; ++r)
            {
                //Bool that determines if the current tile has a new region name
                bool isTileNewRegion = true;

                //Looping through all of the tiles in the pathfinding starting points
                foreach(TileInfo startPoint in pathfindingStartingPoints)
                {
                    //If the current tile has the same region name as any of the tiles in the pathfinding starting points, it's not a new tile to be added to the list
                    if(this.tileGrid[c][r].regionName == startPoint.regionName)
                    {
                        isTileNewRegion = false;
                        break;
                    }
                }

                //If the tile is a new region, we add it to the region starting points
                if(isTileNewRegion)
                {
                    pathfindingStartingPoints.Add(this.tileGrid[c][r]);
                }
            }
        }


        //Once we find all of the starting points, we use pathfinding to get the center tile in each region
        foreach(TileInfo startingPoint in pathfindingStartingPoints)
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
        return cityTiles;
    }


    //Function called from StartMapCreation to set all of the map locations in each region
    private void CreateMapLocations()
    {
        //Looping through each city tile
        foreach(TileInfo cityTile in this.cityTiles)
        {
            //Reference for the region info that the city tile is in
            RegionInfo cityRegion = null;

            //Checking if the city is in a very easy region
            foreach(RegionInfo veRegion in this.veryEasyRegions)
            {
                //If this region has the same name as the current city tile
                if(veRegion.regionName == cityTile.regionName)
                {
                    //We save the region reference and break the loop
                    cityRegion = veRegion;
                    break;
                }
            }

            //Checking if the city is in an easy region
            if(cityRegion == null)
            {
                foreach(RegionInfo eRegion in this.easyRegions)
                {
                    //If this region has the same name as the current city tile
                    if(eRegion.regionName == cityTile.regionName)
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
                foreach (RegionInfo nRegion in this.mediumRegions)
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
                foreach (RegionInfo hRegion in this.hardRegions)
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
                foreach (RegionInfo vhRegion in this.veryHardRegions)
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
                foreach (RegionInfo fRegion in this.finalRegions)
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
            if(cityRegion != null)
            {
                //If the region has a city prefab
                if(cityRegion.regionCity != null)
                {
                    //Adding the city object to this city tile's list of objects
                    cityTile.AddObjectToThisTile(cityRegion.regionCity.gameObject);
                    Debug.Log("Here's where cities are being added");
                }

                //If the region has a dungeon prefab
                if(cityRegion.regionDungeon != null)
                {

                }
            }
        }
    }


    //Function called from StartMapCreation to expand the boarders of each region
    private void ExpandRegionBoarders()
    {
        //Making a list of each index for the number of total regions
        List<int> numRegions = new List<int>();
        for(int r = 0; r < this.cityTiles.Count; ++r)
        {
            numRegions.Add(r);
        }

        //Creating a separate list to randomize the order of the region indexes
        List<int> randRegionOrder = new List<int>();
        for(int c = 0; c < this.cityTiles.Count; ++c)
        {
            //Getting a random index from the numRegions list
            int randIndex = Random.Range(0, this.cityTiles.Count - 1);
            //Adding the region index to our list of random region orders
            randRegionOrder.Add(numRegions[randIndex]);
        }

        //Looping through each region in our randomized order
        foreach(int index in randRegionOrder)
        {
            //We get the tile reference for the given index's city
            TileInfo cityTile = this.cityTiles[index];

            //We find the list of tiles along the edge of the city tile's region
            List<TileInfo> edgeTiles = PathfindingAlgorithms.FindRegionEdgeTiles(cityTile);

            //And then we have the region step outward based on the size of the region's boarders
            Vector2 minMaxSteps = new Vector2();
            minMaxSteps.x = edgeTiles.Count * this.minMaxStepPercent.x;
            minMaxSteps.y = edgeTiles.Count * this.minMaxStepPercent.y;
            PathfindingAlgorithms.StepOutRegionEdge(edgeTiles, minMaxSteps);
        }
    }


    //Function called from ImprovedMapGeneration and CreateMapNormal to put the test player party on the start tile
    private void SetPlayerPartyPosition(TileInfo startTile_)
    {
        //Instantiating the player group at the starting tile's location
        GameObject playerParty1 = GameObject.Instantiate(this.partyGroup1Prefab, startTile_.tilePosition, new Quaternion());

        playerParty1.GetComponent<Movement>().SetCurrentTile(startTile_);

        //Looping through all of the children for the GameData object to get the created characters
        foreach(Character t in GameData.globalReference.transform.GetComponentsInChildren<Character>())
        {
            playerParty1.GetComponent<PartyGroup>().AddCharacterToGroup(t.GetComponent<Character>());
        }

        //If there are no characters that were added (either because of a glitch or just testing), we create the test characters
        if (playerParty1.GetComponent<PartyGroup>().charactersInParty.Count == 0)
        {
            //Instantiating the test characters at the starting tile's location
            GameObject startChar1 = GameObject.Instantiate(this.testCharacter, startTile_.tilePosition, new Quaternion());
            GameObject startChar2 = GameObject.Instantiate(this.testCharacter2, startTile_.tilePosition, new Quaternion());

            //Adding the starting characters to the party group
            playerParty1.GetComponent<PartyGroup>().AddCharacterToGroup(startChar1.GetComponent<Character>());
            playerParty1.GetComponent<PartyGroup>().AddCharacterToGroup(startChar2.GetComponent<Character>());
        }
        
        //Setting the character manager to be selecting the player party 1
        CharacterManager.globalReference.selectedGroup = playerParty1.GetComponent<PartyGroup>();

        //Creating the test enemy and adding them to a tile next to the start tile
        int connectedTileIndex = 0;
        for(int c = 0; c < startTile_.connectedTiles.Count; ++c)
        {
            if(startTile_.connectedTiles[c] != null)
            {
                connectedTileIndex = c;
                break;
            }
        }
        
        GameObject testEnemy = GameObject.Instantiate(this.testEnemyEncounter, startTile_.connectedTiles[connectedTileIndex].tilePosition, new Quaternion());
        testEnemy.GetComponent<Movement>().SetCurrentTile(startTile_.connectedTiles[connectedTileIndex]);


        int connectedTileIndex2 = connectedTileIndex;
        for(int c2 = connectedTileIndex + 1; c2 < startTile_.connectedTiles.Count; ++ c2)
        {
            if(startTile_.connectedTiles[c2] != null)
            {
                connectedTileIndex2 = c2;
            }
        }

        GameObject testEnemy2 = GameObject.Instantiate(this.testEnemyEncounter, startTile_.connectedTiles[connectedTileIndex2].tilePosition, new Quaternion());
        testEnemy2.GetComponent<Movement>().SetCurrentTile(startTile_.connectedTiles[connectedTileIndex2]);


        int connectedTileIndex3 = connectedTileIndex2;
        for(int c3 = connectedTileIndex2 + 1; c3 < startTile_.connectedTiles.Count; ++c3)
        {
            if(startTile_.connectedTiles[c3] != null)
            {
                connectedTileIndex3 = c3;
            }
        }
        GameObject testEnemy3 = GameObject.Instantiate(this.testEnemyEncounter, startTile_.connectedTiles[connectedTileIndex3].tilePosition, new Quaternion());
        testEnemy3.GetComponent<Movement>().SetCurrentTile(startTile_.connectedTiles[connectedTileIndex3]);
    }


    //Creates a region of a specific zone type using "spokes" that extend outward from the given tile
    private void GenerateSpokeRegion(int startTileRow_, int startTileCol_, Vector2 spokeMinMax_, Vector2 spokeLengthMinMax_, RegionInfo regionInfo_)
    {
        //Throwing an exception if the user inputs a starting tile that isn't on the grid
        if (startTileCol_ < 0 || startTileCol_ >= this.tileGrid.Count || startTileRow_ < 0 || startTileRow_ >= this.tileGrid[0].Count)
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
            TileInfo startPoint = this.tileGrid[startX][startY];
            TileInfo endPoint = this.tileGrid[endX][endY];

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
        TileInfo startTile = this.tileGrid[x][y];

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


    //Function called externally and from StartMapCreation. Only spawns the nearest tiles around the player party
    public void GenerateVisibleLand(Movement visiblePlayerParty_)
    {
        //The list of each group of tiles in every range incriment. The index is the range
        List<List<TileInfo>> tilesInEachIncriment = new List<List<TileInfo>>();
        //Creating the first range incriment which always includes only the player party's tile
        List<TileInfo> range0 = new List<TileInfo>() { visiblePlayerParty_.currentTile };
        range0[0].hasBeenChecked = true;
        tilesInEachIncriment.Add(range0);
        
        //Looping through each radius of tiles in the visibility range
        for (int r = 1; r <= this.visibilityRange; ++r)
        {
            //Creating a new list of tiles for this range incriment
            List<TileInfo> newRange = new List<TileInfo>();

            //Looping through each tile in the previous range
            foreach(TileInfo tile in tilesInEachIncriment[r-1])
            {
                //Looping through each tile connected to the one we're checking
                foreach(TileInfo connection in tile.connectedTiles)
                {
                    //If the connected tile hasn't already been checked
                    if(connection != null && !connection.hasBeenChecked)
                    {
                        //Adding the connected tile to this new range and marking it as checked
                        newRange.Add(connection);
                        connection.hasBeenChecked = true;
                    }
                }
            }
            //Adding this range incriment to the list
            tilesInEachIncriment.Add(newRange);
        }

        //Clearing the current list of visible tile objects and destroying them
        foreach(TileInfo tile in this.visibleTileObjects.Keys)
        {
            Destroy(this.visibleTileObjects[tile]);
            this.visibleTileObjects[tile] = null;
        }
        this.visibleTileObjects.Clear();
        
        //Grouping all of the tiles into the list that are visible
        foreach (List<TileInfo> rangeList in tilesInEachIncriment)
        {
            foreach(TileInfo tile in rangeList)
            {
                //Resetting the tile to say it hasn't been checked
                tile.hasBeenChecked = false;

                //Creating an instance of the hex mesh for this tile
                GameObject tileMesh = Instantiate(this.hexMesh.gameObject, new Vector3(tile.tilePosition.x, tile.elevation, tile.tilePosition.z), new Quaternion());
                //Setting the mesh's material to the correct one for the tile
                Material[] tileMat = tileMesh.GetComponent<MeshRenderer>().materials;
                tileMat[0] = tile.tileMaterial;
                tileMesh.GetComponent<MeshRenderer>().materials = tileMat;
                //Setting the tile's reference in the LandTile component
                tileMesh.GetComponent<LandTile>().tileReference = tile;

                //Adding the hex mesh to our list of visible tile objects
                this.visibleTileObjects.Add(tile, tileMesh);

                //If this tile has a decoration model, an instance of it is created and parented to this tile's mesh object
                if(tile.decorationModel != null)
                {
                    GameObject decor = Instantiate(tile.decorationModel, tile.tilePosition, new Quaternion());
                    decor.transform.SetParent(tileMesh.transform);
                }
            }
        }
    }


    //Function called externally from Movement.cs and from StartMapCreation. Spawns the nearest tiles around the player party
    public void GenerateVisibleLand2(TileInfo currentTile_)
    {
        //Getting all of the tiles within view range of the current tile
        List<TileInfo> tilesInRange = PathfindingAlgorithms.FindLandTilesInRange(currentTile_, this.visibilityRange);

        //Creating a list to hold all of the tiles that are just coming into view
        List<TileInfo> newTiles = new List<TileInfo>();
        //Creating a list to hold all of the tiles that are now outside our view
        List<TileInfo> oldTiles = new List<TileInfo>();

        //Looping through all of the tiles that are currently in view
        foreach(TileInfo inRangeTile in tilesInRange)
        {
            //If the tile in range isn't in the current dictionary of visible objects
            if(!this.visibleTileObjects.ContainsKey(inRangeTile))
            {
                //The tile is new
                newTiles.Add(inRangeTile);
            }
        }

        //Looping through all of the tiles that are currently visible
        foreach(TileInfo visibleTile in this.visibleTileObjects.Keys)
        {
            //If the visible tile isn't in range of the new tile
            if(!tilesInRange.Contains(visibleTile))
            {
                oldTiles.Add(visibleTile);
            }
        }
        
        //Looping through and removing each tile in the list of old tiles
        foreach(TileInfo oldTile in oldTiles)
        {
            //Deleting the game object for this tile in the visible tile objects dictionary
            Destroy(this.visibleTileObjects[oldTile]);
            //Removing this tile from the list
            this.visibleTileObjects.Remove(oldTile);
        }

        //Looping through all of the new tiles and creating an instance of its game object
        foreach(TileInfo newTile in newTiles)
        {
            //Resetting the tile to say it hasn't been checked
            newTile.hasBeenChecked = false;

            //Creating an instance of the hex mesh for this tile
            GameObject tileMesh = Instantiate(this.hexMesh.gameObject, new Vector3(newTile.tilePosition.x, newTile.elevation, newTile.tilePosition.z), new Quaternion());
            //Setting the mesh's material to the correct one for the tile
            Material[] tileMat = tileMesh.GetComponent<MeshRenderer>().materials;
            tileMat[0] = newTile.tileMaterial;
            tileMesh.GetComponent<MeshRenderer>().materials = tileMat;
            //Setting the tile's reference in the LandTile component
            tileMesh.GetComponent<LandTile>().tileReference = newTile;

            //Adding the hex mesh to our list of visible tile objects
            this.visibleTileObjects.Add(newTile, tileMesh);

            //If this tile has a decoration model, an instance of it is created and parented to this tile's mesh object
            if (newTile.decorationModel != null)
            {
                GameObject decor = Instantiate(newTile.decorationModel, newTile.tilePosition, new Quaternion());
                decor.transform.SetParent(tileMesh.transform);
            }
        }
    }


    //Function called at the end of StartMapCreation to make the map texture
    private void CreateMapTexture()
    {
        //Getting the height and width of the texture based on the size of the map
        int mapWidth = CreateTileGrid.globalReference.cols;
        int mapHeight = CreateTileGrid.globalReference.rows;

        //Creating a new map texture using the map width and height
        Texture2D mapTexture = new Texture2D(mapWidth * 2, (mapHeight * 2) + 1, TextureFormat.ARGB32, false);

        //Looping through each column
        for (int c = 0; c < mapWidth; ++c)
        {
            //Looping through each row
            for (int r = 0; r < mapHeight; ++r)
            {
                //Creating a color for the selected pixel
                Color pixelColor;

                
                //If this tile is one of the city tiles
                if (this.cityTiles.Contains(CreateTileGrid.globalReference.tileGrid[c][r]))
                {
                    pixelColor = Color.black;
                }
                //If this tile isn't a city tile
                else
                {
                    //Setting the color based on the type of tile we're currently on
                    switch (CreateTileGrid.globalReference.tileGrid[c][r].type)
                    {
                        case LandType.Ocean:
                            pixelColor = Color.blue;
                            break;

                        case LandType.Grasslands:
                            pixelColor = Color.yellow;
                            break;

                        case LandType.Forest:
                            pixelColor = Color.green;
                            break;

                        case LandType.Desert:
                            pixelColor = new Color(1, 0.8f, 0.55f);
                            break;

                        case LandType.Swamp:
                            pixelColor = new Color(0, 1, 0.58f);
                            break;

                        case LandType.Mountain:
                            pixelColor = Color.grey;
                            break;

                        case LandType.Volcano:
                            pixelColor = Color.red;
                            break;

                        default:
                            pixelColor = Color.white;
                            break;
                    }
                }

                //If we're on an even numbered column
                if (c % 2 == 0)
                {
                    //Setting the tile color to the pixel
                    mapTexture.SetPixel(c * 2, (r * 2) + 1, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, (r * 2) + 1, pixelColor);
                    mapTexture.SetPixel(c * 2, (r * 2) + 2, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, (r * 2) + 2, pixelColor);
                }
                //If we're on an odd numbered column
                else
                {
                    //Setting the tile color to the pixel
                    mapTexture.SetPixel(c * 2, r * 2, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, r * 2, pixelColor);
                    mapTexture.SetPixel(c * 2, (r * 2) + 1, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, (r * 2) + 1, pixelColor);
                }
            }
        }

        //Applying the pixels
        mapTexture.Apply();

        //Encoding the texture to a png
        byte[] bytes = mapTexture.EncodeToPNG();

        string seedName = GameData.globalReference.GetComponent<RandomSeedGenerator>().seed;

        //Writing the file to the desktop
        System.IO.File.WriteAllBytes("C:/Users/Mitch/Desktop/" + seedName + ".png", bytes);
    }
}