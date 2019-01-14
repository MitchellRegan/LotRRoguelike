using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //The land tile that this character is currently on
    [HideInInspector]
    public TileInfo currentTile;
    //The land tile that this character is traveling toward
    [HideInInspector]
    public TileInfo tileToTravelTo;
    //The list of land tiles that make up the path that this character is following
    [HideInInspector]
    public List<TileInfo> travelPath;

    //The amount of frames it takes to travel between the current tile and the tile to travel to
    private int totalTravelTime = 24;
    //The number of frames this character has spent traveling to the target tile
    private float currentTravelTime = 1;

    //Bool that when True allows this character to interpolate to the correct tile
    private bool isTraveling = false;


	
    //Function called externally to set the tile that this character is on
    public void SetCurrentTile(TileInfo currentTile_)
    {
        //Leaving the previous tile
        if(this.currentTile != null)
        {
            this.currentTile.RemoveObjectFromThisTile(this.gameObject);
        }

        //Setting the current tile
        this.currentTile = currentTile_;
        this.currentTile.AddObjectToThisTile(this.gameObject);
        
        //Resetting the travel times and clearing the tile to travel to
        this.currentTravelTime = 0;
        this.tileToTravelTo = null;
        
        //Moving this character to the current tile's position
        this.transform.position = new Vector3(this.currentTile.tilePosition.x, this.currentTile.elevation, this.currentTile.tilePosition.z);
        
        //If this movement script is attached to a player party group, the tile grid needs to update the visible tiles
        if (this.GetComponent<PartyGroup>())
        {
            Debug.Log("Movement.SetCurrentTile >>> Generating Visible Land");
            CreateTileGrid.globalReference.GenerateVisibleLand(this.currentTile);
        }
    }


    //Function called externally to set the tile that this character should travel to
    public void TravelToPath(List<TileInfo> pathToTravelOn_)
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
        Vector3 distDiff = new Vector3(this.tileToTravelTo.tilePosition.x - this.currentTile.tilePosition.x,
                                        this.tileToTravelTo.elevation - this.currentTile.elevation,
                                        this.tileToTravelTo.tilePosition.z - this.currentTile.tilePosition.z);
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
            this.SetCurrentTile(this.tileToTravelTo);


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
    }
}
