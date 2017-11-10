using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationOpenerUI : MonoBehaviour
{
    //Button that allows the player to go to the tile location
    public Button goToLocationButton;

    //The Canvas UI that's opened when the player travels to a city
    public GameObject cityUICanvas;

    //The reference to the player tile's map location
    MapLocation playerTileLocation = null;



    //Function called every frame
    private void Update()
    {
        //Getting the reference to the current player party
        TileInfo partyTile = CharacterManager.globalReference.selectedGroup.GetComponent<Movement>().currentTile;

        //If the player party tile's decoration object is a map location
        if(partyTile.decorationModel != null && partyTile.decorationModel.GetComponent<MapLocation>())
        {
            //We enable the goToLocationButton so that the player can travel there
            this.goToLocationButton.interactable = true;
            //Setting the playerTileLocation reference
            this.playerTileLocation = partyTile.decorationModel.GetComponent<MapLocation>();
            //We also end this function so we don't keep looping for no reason
            return;
        }

        //Checking to see if any of the objects on this tile are map locations
        /*foreach(GameObject obj in partyTile.objectsOnThisTile)
        {
            //If we find an object that is a map location
            if(obj.GetComponent<MapLocation>())
            {
                //We enable the goToLocationButton so that the player can travel there
                this.goToLocationButton.interactable = true;
                //Setting the playerTileLocation reference
                this.playerTileLocation = obj.GetComponent<MapLocation>();
                //We also end this function so we don't keep looping for no reason
                return;
            }
        }*/

        //If we made it past the loop, there were no map locations on the current tile, so we disable the goToLocationButton
        this.goToLocationButton.interactable = false;
        //Setting the playerTileLocation reference to null
        this.playerTileLocation = null;
        //We also make sure the cityUICanvas is disabled just in case
        this.cityUICanvas.SetActive(false);
    }


    //Function called externally to travel to a location on the player party's current tile
    public void EnterTileLocation()
    {
        //Making sure there's a MapLocation on this tile first
        if(this.playerTileLocation == null)
        {
            return;
        }

        //If the tile location is a city
        if(this.playerTileLocation.GetComponent<CityLocation>())
        {
            this.playerTileLocation.TravelToLocation();
        }

        //If the location is a Dungeon
        if(this.playerTileLocation.GetComponent<DungeonLocation>())
        {
            this.playerTileLocation.TravelToLocation();
        }
    }
}
