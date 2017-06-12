using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //The land tile that this character is currently on
    [HideInInspector]
    public LandTile currentTile;
    //The land tile that this character is traveling toward
    [HideInInspector]
    public LandTile tileToTravelTo;
    //The list of land tiles that make up the path that this character is following
    [HideInInspector]
    public List<LandTile> travelPath;

    //The amount of frames it takes to travel between the current tile and the tile to travel to
    private int totalTravelTime = 24;
    //The number of frames this character has spent traveling to the target tile
    private float currentTravelTime = 1;

    //Bool that when True allows this character to interpolate to the correct tile
    private bool isTraveling = false;


	
    //Function called externally to set the tile that this character is on
    public void SetCurrentTile(LandTile currentTile_)
    {
        //Leaving the previous tile
        if(this.currentTile != null)
        {
            this.currentTile.RemoveCharacterFromThisTile(this.gameObject);
        }
        //Setting the current tile
        this.currentTile = currentTile_;
        this.currentTile.AddCharacterToThisTile(this.gameObject);

        //Resetting the travel times and clearing the tile to travel to
        this.currentTravelTime = 0;
        this.tileToTravelTo = null;

        //Moving this character to the current tile's position
        this.transform.position = this.currentTile.transform.position;
    }


    //Function called externally to set the tile that this character should travel to
    public void TravelToPath(List<LandTile> pathToTravelOn_)
    {
        //Resetting the player back to the current tile to prevent weird position offsets
        this.SetCurrentTile(this.currentTile);

        //Setting the list of tiles to travel along
        this.travelPath = pathToTravelOn_;

        //Setting the tile to travel to as the first one from the travel path
        this.tileToTravelTo = pathToTravelOn_[1];

        //Popping the first two tiles from the travel path (index 0 is the current tile)
        this.travelPath.RemoveAt(0);
        this.travelPath.RemoveAt(0);
    }


    //Function called externally when time is advanced. This is used to update the travel time and move the player position
    public void OnTimeAdvanced()
    {
        //If there isn't a tile to travel to, nothing happens
        if(this.tileToTravelTo != null)
        {
            this.isTraveling = true;
        }
    }


    //Function called every frame
    private void Update()
    {
        //If not traveling, nothing happens
        if(!this.isTraveling)
        {
            return;
        }
        
        //Finds the difference between the the current tile and the tile to travel to
        Vector3 distDiff = new Vector3(this.tileToTravelTo.transform.position.x - this.currentTile.transform.position.x,
                                        this.tileToTravelTo.transform.position.y - this.currentTile.transform.position.y,
                                        this.tileToTravelTo.transform.position.z - this.currentTile.transform.position.z);
        //Multiplying the difference by the percentage of the travel time that's passed
        distDiff = distDiff / (this.totalTravelTime * 1f);
        
        //Adding the difference in distance to this character's position
        this.transform.position += distDiff;

        //Increases the amount of frames traved
        this.currentTravelTime += 1;
        //If the travel time is higher than the total travel time, it resets, stops traveling, and set the tile to travel to as the current tile
        if(this.currentTravelTime > this.totalTravelTime)
        {
            this.currentTravelTime = 1;
            this.isTraveling = false;
            this.currentTile = this.tileToTravelTo;


            //If there are still tiles left in the travel path, the one at the front becomes the tile to travel to
            if(this.travelPath.Count > 0)
            {
                this.tileToTravelTo = this.travelPath[0];
                //Popping the current tile from the travel path
                this.travelPath.RemoveAt(0);
            }
            //Otherwise, we clear the tile to travel to
            else
            {
                this.tileToTravelTo = null;
            }
        }
        /*//Adding the percentage of a day that's passed
        this.currentTravelTime += (TimePanelUI.globalReference.hoursAdvancedPerUpdate * 1f) / 24f;

        //If the current travel time has reached the total travel time, this character is now on that tile
        if (this.currentTravelTime >= this.totalTravelTime)
        {
            this.SetCurrentTile(this.tileToTravelTo);

            //If there are more tiles in the travel path, the next one is set as the tile to travel to
            if (this.travelPath.Count > 0)
            {
                this.tileToTravelTo = this.travelPath[0];

                //Removing the tile from the path
                this.travelPath.RemoveAt(0);
            }
        }
        //If the character is still traveling, their position is updated to move toward
        else
        {
            //Finding the difference in position between the current tile and the tile to travel toward
            Vector3 tilePosDiff = this.tileToTravelTo.transform.position - this.currentTile.transform.position;
            //Finding the percentage of the travel distance that this character has traveled
            float percentTraveled = this.currentTravelTime / this.totalTravelTime;

            //Setting this character's position to the offset location based on the percent traveled
            this.transform.position += tilePosDiff * percentTraveled;
        }*/
    }
}
