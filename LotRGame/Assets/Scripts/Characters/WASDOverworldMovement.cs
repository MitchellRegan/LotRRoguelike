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
    //The movement cost for the current tile to multiply the travel time by
    private int currentMoveCost = 1;
    //Bool that when True allows this character to interpolate to the correct tile
    private bool isTraveling = false;
    //Bool that when True prevents the player from moving while combat is happening
    private bool isInCombat = false;

    //The button for moving forward
    public KeyCode forwardButton = KeyCode.W;
    //The button for moving right
    public KeyCode rightButton = KeyCode.D;
    //The button for moving left
    public KeyCode leftButton = KeyCode.A;

    //Delegate event to listen for events to start and end combat
    private DelegateEvent<EVTData> combatTransitionListener;



    //Function called when this object is created to assign our delegate event
    private void Awake()
    {
        this.combatTransitionListener = new DelegateEvent<EVTData>(this.CombatTransitioning);
    }


    //Function called when this component is enabled
    private void OnEnable()
    {
        EventManager.StartListening(CombatTransitionEVT.eventNum, this.combatTransitionListener);
    }


    //Function called when this component is disabled
    private void OnDisable()
    {
        EventManager.StopListening(CombatTransitionEVT.eventNum, this.combatTransitionListener);
    }


    //Function called externally to set the tile that this character is on
    public void SetCurrentTile(TileInfo currentTile_, bool rollForEncounter_ = true)
    {
        //Leaving the previous tile
        if (this.currentTile != null)
        {
            this.currentTile.RemoveObjectFromThisTile(this.gameObject);
        }

        //Setting the current tile
        this.currentTile = currentTile_;
        this.currentTile.AddObjectToThisTile(this.gameObject, rollForEncounter_);

        //Resetting the travel times and clearing the tile to travel to
        this.currentTravelTime = 0;
        this.tileToTravelTo = null;
        
        //Moving this character to the current tile's position
        this.transform.position = new Vector3(this.currentTile.tilePosition.x, this.currentTile.elevation, this.currentTile.tilePosition.z);
        
        //If this movement script is attached to a player party group
        if (this.GetComponent<PartyGroup>())
        {
            //The tile grid needs to update the visible tiles
            CreateTileGrid.globalReference.GenerateVisibleLand(this.currentTile);
            if (QuestTracker.globalReference != null)
            {
                //Checking to see if our current tile is a destination for a quest
                QuestTracker.globalReference.CheckTravelDestinations(this.currentTile);
            }
        }
    }


    //Function called every frame
    private enum TileDirection { Forward, Left, Right };
    private void Update()
    {
        //If the combat canvas is open, we don't move
        if(CombatManager.globalReference.GetComponent<Canvas>().enabled)
        {
            return;
        }
        //If time is advancing and we're not moving, we don't let the player move (to prevent multiple time advancements at once)
        else if(TimePanelUI.globalReference.IsTimePassing() && !this.isTraveling)
        {
            return;
        }

        //If this party is currently traveling
        if(isTraveling)
        {
            //Finds the difference between the the current tile and the tile to travel to
            Vector3 distDiff = new Vector3(this.tileToTravelTo.tilePosition.x - this.currentTile.tilePosition.x,
                                            this.tileToTravelTo.elevation - this.currentTile.elevation,
                                            this.tileToTravelTo.tilePosition.z - this.currentTile.tilePosition.z);
            //Multiplying the difference by the percentage of the travel time that's passed
            distDiff = distDiff * (Time.deltaTime / (this.totalTravelTime * this.currentMoveCost));

            //Adding the difference in distance to this character's position
            this.transform.position += distDiff;

            //Increases the amount of time traved
            this.currentTravelTime += Time.deltaTime;

            //If the travel time is higher than the total travel time
            if (this.currentTravelTime > (this.totalTravelTime * this.currentMoveCost))
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
        //If this party is NOT currently traveling or in combat
        else if(!this.isInCombat)
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
            if(Input.GetKey(this.forwardButton))
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
                    //Setting the movement cost to multiply travel time
                    this.currentMoveCost = this.currentTile.movementCost;
                    //Dispatching the time advance event
                    TimePanelUI.globalReference.AdvanceTime(this.currentTile.movementCost * TimePanelUI.globalReference.hoursAdvancedPerUpdate);
                }
            }
            //If the player presses the left button
            else if(Input.GetKey(this.leftButton))
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
                    //Setting the movement cost to multiply travel time
                    this.currentMoveCost = this.currentTile.movementCost;
                    //Dispatching the time advance event
                    TimePanelUI.globalReference.AdvanceTime(this.currentTile.movementCost * TimePanelUI.globalReference.hoursAdvancedPerUpdate);
                }
            }
            //If the player presses the right button
            else if(Input.GetKey(this.rightButton))
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
                    //Setting the movement cost to multiply travel time
                    this.currentMoveCost = this.currentTile.movementCost;
                    //Dispatching the time advance event
                    TimePanelUI.globalReference.AdvanceTime(this.currentTile.movementCost * TimePanelUI.globalReference.hoursAdvancedPerUpdate);
                }
            }
        }
    }


    //Function called from the combatTransitionListener delegate from CombatTransitionEVT events
    private void CombatTransitioning(EVTData data_)
    {
        //Making sure the combat transition event data isn't null
        if(data_.combatTransition != null)
        {
            //Setting the isInCombat bool based on if combat is starting or ending
            this.isInCombat = data_.combatTransition.startingCombat;
        }
    }
}
