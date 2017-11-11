using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDOverworldMovement : MonoBehaviour
{
    //The land tile that this party is currently on
    [HideInInspector]
    public TileInfo currentTile;
    //The land tile that this party is traveling to
    private TileInfo tileToTravelTo;

    //The amount of time in seconds it takes to travel between the current tile and the next tile
    public float totalTravelTime = 0.75f;
    //The current travel time
    private float currentTravelTime = 0;
    //Bool that, when True, allows this character to interpolate to the correct tile
    private bool isTraveling = false;

    //The button for moving forward
    public KeyCode forwardButton = KeyCode.W;
    //The button for moving right
    public KeyCode rightButton = KeyCode.D;
    //The button for moving left
    public KeyCode leftButton = KeyCode.A;



    //Function called externally to set the tile that this character is on
    public void SetCurrentTile(TileInfo currentTile_)
    {
        //Leaving the previous tile
        if (this.currentTile != null)
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
            CreateTileGrid.globalReference.GenerateVisibleLand2(this.currentTile);
        }
    }


    //Function called every frame
    private enum TileDirection { Forward, Left, Right };
    private void Update()
    {
        //If this party is currently traveling
        if(isTraveling)
        {
            //Finds the difference between the the current tile and the tile to travel to
            Vector3 distDiff = new Vector3(this.tileToTravelTo.tilePosition.x - this.currentTile.tilePosition.x,
                                            this.tileToTravelTo.elevation - this.currentTile.elevation,
                                            this.tileToTravelTo.tilePosition.z - this.currentTile.tilePosition.z);
            //Multiplying the difference by the percentage of the travel time that's passed
            distDiff = distDiff * (Time.deltaTime / this.totalTravelTime);

            //Adding the difference in distance to this character's position
            this.transform.position += distDiff;

            //Increases the amount of time traved
            this.currentTravelTime += Time.deltaTime;

            //If the travel time is higher than the total travel time
            if (this.currentTravelTime > this.totalTravelTime)
            {
                //We reset the current travel time
                this.currentTravelTime = 0;
                //Indicate that we're done traveling
                this.isTraveling = false;
                //Setting our position to the tile that we're traveling to
                this.SetCurrentTile(this.tileToTravelTo);
                //And removing any indication that we're traveling anywhere else
                this.tileToTravelTo = null;
            }
        }
        //If this party is NOT currently traveling
        else
        {
            //We find the angle that the player camera is facing and adding 360 so it's easier to deal with angle wrap-around
            float cameraAngle = OrbitCamera.directionFacing + 360;

            //Creating a dictionary of tiles that we could potentially move toward and their direction
            Dictionary<TileDirection, TileInfo> potentialTravelTiles = new Dictionary<TileDirection, TileInfo>();

            //Looping through all of the tiles connected to our current tile
            foreach(TileInfo connectedTile in this.currentTile.connectedTiles)
            {
                //Making sure the connected tile isn't null first
                if (connectedTile != null)
                {
                    //Finding the angle between the connected tile and the current tile
                    float tileAngle = Mathf.Atan2(connectedTile.tilePosition.z - this.currentTile.tilePosition.z,
                                                    connectedTile.tilePosition.x - this.currentTile.tilePosition.x);

                    //Converting the tile angle to degrees because they're easier for me to work with than radians
                    tileAngle = tileAngle * Mathf.Rad2Deg;
                    

                    //Making sure the angle is between 0 and 360 instead of -180 and 180
                    if (tileAngle < 0)
                    {
                        tileAngle += 360;
                    }

                    tileAngle += 360;

                    //Adding 360 so it's easier to deal with angle wrap-around
                    if ((tileAngle - cameraAngle) < -180)
                    {
                        tileAngle += 360;
                    }
                    else if((tileAngle - cameraAngle) > 180)
                    {
                        tileAngle -= 360;
                    }

                    //If this current tile is within 60 degrees of the camera angle
                    if (tileAngle - cameraAngle >= -20 && tileAngle - cameraAngle <= 20)
                    {
                        //This tile is in the forward direction
                        if (!potentialTravelTiles.ContainsKey(TileDirection.Forward))
                        {
                            potentialTravelTiles.Add(TileDirection.Forward, connectedTile);
                            //Debug.Log("Forward: " + (connectedTile.tilePosition - this.currentTile.tilePosition) + ", angle: " + (tileAngle - 360));
                        }
                    }
                    //If this current tile is 60 degrees to the right
                    else if (tileAngle - cameraAngle > -80 && tileAngle - cameraAngle < -20)
                    {
                        //This tile is in the right direction
                        if (!potentialTravelTiles.ContainsKey(TileDirection.Right))
                        {
                            potentialTravelTiles.Add(TileDirection.Right, connectedTile);
                            //Debug.Log("Right: " + (connectedTile.tilePosition - this.currentTile.tilePosition) + ", angle: " + (tileAngle - 360));
                        }
                    }
                    //If this current tile is 60 degrees to the left
                    else if (tileAngle - cameraAngle > 20 && tileAngle - cameraAngle < 80)
                    {
                        //This tile is in the left direction
                        if (!potentialTravelTiles.ContainsKey(TileDirection.Left))
                        {
                            potentialTravelTiles.Add(TileDirection.Left, connectedTile);
                            //Debug.Log("Left: " + (connectedTile.tilePosition - this.currentTile.tilePosition) + ", angle: " + (tileAngle - 360));
                        }
                    }
                }
            }

            //If the player pressed the forward button
            if(Input.GetKeyDown(this.forwardButton))
            {
                //If there's a tile in the forward position
                if(potentialTravelTiles.ContainsKey(TileDirection.Forward))
                {
                    //We set the the tile as the location to move to
                    this.tileToTravelTo = potentialTravelTiles[TileDirection.Forward];
                    //Indicating that we're now traveling
                    this.isTraveling = true;
                    //Making sure our current travel time is reset
                    this.currentTravelTime = 0;
                    //Dispatching the time advance event
                    TimePanelUI.globalReference.AdvanceTime();
                }
            }
            //If the player presses the left button
            else if(Input.GetKeyDown(this.leftButton))
            {
                //If there's a tile in the left position
                if(potentialTravelTiles.ContainsKey(TileDirection.Left))
                {
                    //We set the the tile as the location to move to
                    this.tileToTravelTo = potentialTravelTiles[TileDirection.Left];
                    //Indicating that we're now traveling
                    this.isTraveling = true;
                    //Making sure our current travel time is reset
                    this.currentTravelTime = 0;
                    //Dispatching the time advance event
                    TimePanelUI.globalReference.AdvanceTime();
                }
            }
            //If the player presses the right button
            else if(Input.GetKeyDown(this.rightButton))
            {
                //If there's a tile in the right position
                if(potentialTravelTiles.ContainsKey(TileDirection.Right))
                {
                    //We set the the tile as the location to move to
                    this.tileToTravelTo = potentialTravelTiles[TileDirection.Right];
                    //Indicating that we're now traveling
                    this.isTraveling = true;
                    //Making sure our current travel time is reset
                    this.currentTravelTime = 0;
                    //Dispatching the time advance event
                    TimePanelUI.globalReference.AdvanceTime();
                }
            }
        }
    }
}
