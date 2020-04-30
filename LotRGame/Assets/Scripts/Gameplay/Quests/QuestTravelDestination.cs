using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used by Quest to define what a travel quest is
[System.Serializable]
public class QuestTravelDestination
{
    //The map locatios that is required to visit for this quest
    public MapLocation requiredLocation;
    //Bool that determines if this location has been visited
    [HideInInspector]
    public bool locationVisited = false;



    //Function called externally to check if a visited tile has one of our required locations
    public void CheckTileForDestination(TileInfo visitedTile_)
    {
        //If we've already visited the location, nothing happens
        if (this.locationVisited)
        {
            return;
        }

        //Checking to make sure the tile and the decoration object aren't null (just in case)
        if (visitedTile_ != null && visitedTile_.decorationModel != null)
        {
            //Checking to see if the decoration model is a map location
            if (visitedTile_.decorationModel.GetComponent<MapLocation>())
            {
                //If we find a match, we remove the location from our required list and mark it as being visited
                if (visitedTile_.decorationModel.GetComponent<MapLocation>().locationName == this.requiredLocation.locationName)
                {
                    this.locationVisited = true;
                }
            }
        }
    }
}