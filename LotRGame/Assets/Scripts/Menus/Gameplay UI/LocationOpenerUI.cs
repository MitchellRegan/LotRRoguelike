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

    //The buttons for each of the different vendor UI elements that the city could have
    public Button generalStoreUI;
    public Button blacksmithUI;
    public Button tavernUI;
    public Button mageTowerUI;
    public Button churchUI;
    public Button darkTempleUI;
    public Button castleUI;

    //The reference to the player tile's map location
    MapLocation playerTileLocation = null;



    //Function called every frame
    private void Update()
    {
        //Getting the reference to the current player party
        TileInfo partyTile = CharacterManager.globalReference.selectedGroup.GetComponent<WASDOverworldMovement>().currentTile;
        
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
            this.ClearVendorUI();
            this.DisplayCityUI(this.playerTileLocation.GetComponent<CityLocation>());
        }

        //If the location is a Dungeon
        if(this.playerTileLocation.GetComponent<DungeonLocation>())
        {
            this.playerTileLocation.TravelToLocation();
        }
    }


    //Function called from EnterTileLocation to display the correct city UI
    private void DisplayCityUI(CityLocation city_)
    {
        this.playerTileLocation.TravelToLocation();
        this.cityUICanvas.SetActive(true);

        //Looping through each of the vendors in the city
        foreach(Vendor cityVendor in city_.cityVendors)
        {
            //Activating the correct vendor UI based on this vendor's type
            switch(cityVendor.type)
            {
                case Vendor.VendorTypes.GenderalStore:
                    this.generalStoreUI.enabled = true;
                    break;

                case Vendor.VendorTypes.Blacksmith:
                    this.blacksmithUI.enabled = true;
                    break;

                case Vendor.VendorTypes.Tavern:
                    this.tavernUI.enabled = true;
                    break;

                case Vendor.VendorTypes.MageTower:
                    this.mageTowerUI.enabled = true;
                    break;

                case Vendor.VendorTypes.Church:
                    this.churchUI.enabled = true;
                    break;

                case Vendor.VendorTypes.DarkTemple:
                    this.darkTempleUI.enabled = true;
                    break;

                case Vendor.VendorTypes.Castle:
                    this.castleUI.enabled = true;
                    break;
            }
        }
    }


    //Function called externally from UI buttons to hide all of the vendor UI elements
    public void ClearVendorUI()
    {
        this.generalStoreUI.enabled = false;
        this.blacksmithUI.enabled = false;
        this.tavernUI.enabled = false;
        this.mageTowerUI.enabled = false;
        this.churchUI.enabled = false;
        this.darkTempleUI.enabled = false;
        this.castleUI.enabled = false;
    }
}
