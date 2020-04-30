using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMapRegions : MonoBehaviour
{
    //The maximum spread for the region bands
    public float maxBandAngleSpread = 60;

    //The maximum percent of variance for the band regions
    [Range(0, 0.5f)]
    public float bandRegionPercentVariance = 0.2f;

    [Space(8)]

    //References to each RegionDifficultyDefinition component for all difficulties
    public RegionDifficultyDefinition veryEasy;
    public RegionDifficultyDefinition easy;
    public RegionDifficultyDefinition medium;
    public RegionDifficultyDefinition hard;
    public RegionDifficultyDefinition veryHard;
    public RegionDifficultyDefinition final;



    //Function called from TileMapManager.cs to create the different regions of our map
    public void GenerateRegions()
    {
        this.GenerateDifficultyBands();
    }


    //Function to split the tile map into semi-circle bands of different difficulties
    private void GenerateDifficultyBands()
    {
        //Creating variables to hold the tiles that determine which corner the starting zone and final zone are in
        TileInfo startCorner = TileMapManager.globalReference.tileGrid[0][0];
        TileInfo endCorner = TileMapManager.globalReference.tileGrid[TileMapManager.globalReference.tileGrid.Count - 1][TileMapManager.globalReference.tileGrid[0].Count - 1]; ;

        //Creating a random int from 0-3 to determine which corner the end zone is in
        int cornerIndex = Random.Range(0, 4);
        switch (cornerIndex)
        {
            case 0://North East corner
                //Setting the starting tile as the NE corner
                startCorner = TileMapManager.globalReference.tileGrid[TileMapManager.globalReference.tileGrid.Count - 1][TileMapManager.globalReference.tileGrid[0].Count - 1];
                //Setting the end tile as the SW corner
                endCorner = TileMapManager.globalReference.tileGrid[0][0];
                break;

            case 1://North West corner
                //Setting the starting tile as the NW corner
                startCorner = TileMapManager.globalReference.tileGrid[0][TileMapManager.globalReference.tileGrid[0].Count - 1];
                //Setting the end tile as the SE corner
                endCorner = TileMapManager.globalReference.tileGrid[TileMapManager.globalReference.tileGrid.Count - 1][0];
                break;

            case 2://South West corner
                //Setting the starting tile as the SW corner
                startCorner = TileMapManager.globalReference.tileGrid[0][0];
                //Setting the end tile as the NE corner
                endCorner = TileMapManager.globalReference.tileGrid[TileMapManager.globalReference.tileGrid.Count - 1][TileMapManager.globalReference.tileGrid[0].Count - 1];
                break;

            case 3://South East corner
                //Setting the starting tile as the SE corner
                startCorner = TileMapManager.globalReference.tileGrid[TileMapManager.globalReference.tileGrid.Count - 1][0];
                //Setting the end tile as the NW corner
                endCorner = TileMapManager.globalReference.tileGrid[0][TileMapManager.globalReference.tileGrid[0].Count - 1];
                break;
        }

        //Creating list for all of the tile difficulty bands
        List<List<TileInfo>> veryEasyBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> easyBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> mediumBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> hardBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> veryHardBand = new List<List<TileInfo>>() { new List<TileInfo>() };
        List<List<TileInfo>> finalBand = new List<List<TileInfo>>() { new List<TileInfo>() };


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
        for (int c = 0; c < TileMapManager.globalReference.tileGrid.Count; ++c)
        {
            for (int r = 0; r < TileMapManager.globalReference.tileGrid[0].Count; ++r)
            {
                //Finding the distance the current tile is from the end zone
                float currentTileDist = Vector3.Distance(TileMapManager.globalReference.tileGrid[c][r].tilePosition, endCorner.tilePosition);

                //Determining which radius the tile is within and adding it to that list of tiles
                if (currentTileDist < finalRadius)
                {
                    finalBand[0].Add(TileMapManager.globalReference.tileGrid[c][r]);
                }
                else if (currentTileDist < veryHardRadius)
                {
                    veryHardBand[0].Add(TileMapManager.globalReference.tileGrid[c][r]);
                }
                else if (currentTileDist < hardRadius)
                {
                    hardBand[0].Add(TileMapManager.globalReference.tileGrid[c][r]);
                }
                else if (currentTileDist < mediumRadius)
                {
                    mediumBand[0].Add(TileMapManager.globalReference.tileGrid[c][r]);
                }
                else if (currentTileDist < easyRadius)
                {
                    easyBand[0].Add(TileMapManager.globalReference.tileGrid[c][r]);
                }
                else
                {
                    veryEasyBand[0].Add(TileMapManager.globalReference.tileGrid[c][r]);
                }
            }
        }


        //Splitting the very easy difficulty band
        this.SplitDifficultyBands(veryEasyBand, this.veryEasy.regions, this.veryEasy.minMaxSplits, startCorner, endCorner);
        //Splitting the easy difficulty band
        this.SplitDifficultyBands(easyBand, this.easy.regions, this.easy.minMaxSplits, startCorner, endCorner);
        //Splitting the medium difficulty band
        this.SplitDifficultyBands(mediumBand, this.medium.regions, this.medium.minMaxSplits, startCorner, endCorner);
        //Splitting the hard difficulty band
        this.SplitDifficultyBands(hardBand, this.hard.regions, this.hard.minMaxSplits, startCorner, endCorner);
        //Splitting the very hard difficulty band
        this.SplitDifficultyBands(veryHardBand, this.veryHard.regions, this.veryHard.minMaxSplits, startCorner, endCorner);
        //Splitting the final difficulty band
        this.SplitDifficultyBands(finalBand, this.final.regions, this.final.minMaxSplits, startCorner, endCorner);
    }


    //Function called from GenerateDifficultyBands to split each band into multiple regions
    private void SplitDifficultyBands(List<List<TileInfo>> difficultyBand_, List<RegionInfo> difficultyRegions_, Vector2 numberOfSplitsMinMax_, TileInfo startTile_, TileInfo endTile_)
    {
        //Finding the number of splits we need in the band
        int splits = Mathf.RoundToInt(Random.Range(numberOfSplitsMinMax_.x, numberOfSplitsMinMax_.y));

        /*Since the angle splits originate from the corner of the map where the start tile is, the largest angle it can create is 90 degrees
        * Once we find the direction that the corner is in, we can find the offset for each split by thinking of the start tile position as the origin
        * in an XY plane and the rest of the map as a quadrant*/

        //Creating a float to hold the offset angle. The default assumption is that the end tile is in the South West corner (quadrant 1)
        float angleOffset = 0;

        //If the end tile is in the South East corner (quadrant 2)
        if (startTile_.tilePosition.x < endTile_.tilePosition.x && startTile_.tilePosition.z > endTile_.tilePosition.z)
        {
            //The offset starts at 90 degrees
            angleOffset = 90;
        }
        //If the end tile is in the North East corner (quadrant 3)
        else if (startTile_.tilePosition.x < endTile_.tilePosition.x && startTile_.tilePosition.z < endTile_.tilePosition.z)
        {
            //The offset starts at 180 degrees
            angleOffset = 180;
        }
        //If the end tile is in the North West corner (quadrant 4)
        else if (startTile_.tilePosition.x > endTile_.tilePosition.x && startTile_.tilePosition.z < endTile_.tilePosition.z)
        {
            //The offset starts at 270 degrees
            angleOffset = 270;
        }

        //Now we create a list to hold each angle that a split happens at
        List<float> splitAngles = new List<float>();
        for (int s = 0; s < splits; ++s)
        {
            float newSplit = 0;
            //The base angle is from 90 degrees divided evenly based on the number of splits
            newSplit = (90 / (splits + 1)) * (s + 1);
            //Adding the angle offset so this split is in the correct quadrant
            newSplit += angleOffset;
            //Adding this split to our list
            splitAngles.Add(newSplit);
        }

        //Duplicating the list of regions in the difficulty band so we can modify it
        List<RegionInfo> regionDup = new List<RegionInfo>();
        foreach (RegionInfo duplicate in difficultyRegions_)
        {
            regionDup.Add(duplicate);
        }

        //Creating a list of each region in this difficulty band
        List<RegionInfo> bandRegions = new List<RegionInfo>();

        //If we have more than 1 region
        if (splits > 0)
        {
            //We loop through and get multiple regions for this difficulty band
            for (int r = 0; r < splits + 1; ++r)
            {
                //Getting a random region from the list of difficulty regions
                int regionIndex = Random.Range(0, regionDup.Count);

                //Adding the region to the band region list
                bandRegions.Add(regionDup[regionIndex]);

                //Removing the current region from the list of difficulty regions so it doesn't show up multiple times
                if (regionDup.Count > 1)
                {
                    regionDup.RemoveAt(regionIndex);
                }
            }
        }
        //If this band only has 1 region, we only get 1 random region for this difficulty band
        else
        {
            //Getting a random region from the list of difficulty regions
            int regionIndex = Random.Range(0, regionDup.Count);

            //Adding the region to the band region list
            bandRegions.Add(regionDup[regionIndex]);
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

            //If we have multiple splits in this difficulty band
            if (splits > 0)
            {
                //If the angle is below 10 degrees but the lowest split is above 180, this angle is probably a loop around just over 360 back to 0
                if (angleDiff < 10 && splitAngles[0] > 180)
                {
                    angleDiff += 360;
                }

                //Finding which split the tile is within
                for (int t = 0; t < splits; ++t)
                {
                    //Checking if the current tile's angle is within the current split
                    if (angleDiff < splitAngles[t])
                    {
                        //We set the tile's info using the region with the same index
                        tile.SetTileBasedOnRegion(bandRegions[t]);
                        t = splits;
                    }
                    //If the tile isn't within the current split and this is the last split
                    else if (t + 1 == splits)
                    {
                        //We set the tile's info using the region with the index of the last region
                        tile.SetTileBasedOnRegion(bandRegions[t + 1]);
                    }
                }
            }
            //If there's only 1 region in this difficulty band
            else
            {
                //We set the tile's info using the only region we have
                tile.SetTileBasedOnRegion(bandRegions[0]);
            }
        }
    }
}
