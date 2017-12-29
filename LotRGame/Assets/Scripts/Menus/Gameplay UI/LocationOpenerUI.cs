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

    //The canvas UI that's opened when the player wants to visit a vendor at this location
    public GameObject vendorUICanvas;

    [Space(8)]

    //The buttons for each of the different vendor UI elements that the city could have
    public Button generalStoreUI;
    public Text generalStoreName;

    public Button blacksmithUI;
    public Text blacksmithName;

    public Button tavernUI;
    public Text tavernName;

    public Button mageTowerUI;
    public Text mageTowerName;

    public Button churchUI;
    public Text churchName;

    public Button darkTempleUI;
    public Text darkTempleName;

    public Button castleUI;
    public Text castleName;

    //The reference to the player tile's map location
    private MapLocation playerTileLocation = null;



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
                    if (!this.generalStoreUI.enabled)
                    {
                        this.generalStoreUI.enabled = true;
                        this.generalStoreName.text = cityVendor.buildingName;
                    }
                    break;

                case Vendor.VendorTypes.Blacksmith:
                    if (!this.blacksmithUI.enabled)
                    {
                        this.blacksmithUI.enabled = true;
                        this.blacksmithName.text = cityVendor.buildingName;
                    }
                    break;

                case Vendor.VendorTypes.Tavern:
                    if (!this.tavernUI.enabled)
                    {
                        this.tavernUI.enabled = true;
                        this.tavernName.text = cityVendor.buildingName;
                    }
                    break;

                case Vendor.VendorTypes.MageTower:
                    if (!this.mageTowerUI.enabled)
                    {
                        this.mageTowerUI.enabled = true;
                        this.mageTowerName.text = cityVendor.buildingName;
                    }
                    break;

                case Vendor.VendorTypes.Church:
                    if (!this.churchUI.enabled)
                    {
                        this.churchUI.enabled = true;
                        this.churchName.text = cityVendor.buildingName;
                    }
                    break;

                case Vendor.VendorTypes.DarkTemple:
                    if (!this.darkTempleUI.enabled)
                    {
                        this.darkTempleUI.enabled = true;
                        this.darkTempleName.text = cityVendor.buildingName;
                    }
                    break;

                case Vendor.VendorTypes.Castle:
                    if (!this.castleUI.enabled)
                    {
                        this.castleUI.enabled = true;
                        this.castleName.text = cityVendor.buildingName;
                    }
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


    //Function called externally from UI buttons to display the vendor screen
    public void DisplayVendor(int typeOfVendor_)
    {
        //Reference to the vendor that we're going to display
        Vendor vendorToDisplay = null;

        //Getting the vendor type enum from the int given (since unity can't have enums as variables in UnityEvents)
        Vendor.VendorTypes vendorToVisit = Vendor.VendorTypes.GenderalStore;
        switch (typeOfVendor_)
        {
            case 0:
                vendorToVisit = Vendor.VendorTypes.GenderalStore;
                break;

            case 1:
                vendorToVisit = Vendor.VendorTypes.Blacksmith;
                break;

            case 2:
                vendorToVisit = Vendor.VendorTypes.Tavern;
                break;

            case 3:
                vendorToVisit = Vendor.VendorTypes.MageTower;
                break;

            case 4:
                vendorToVisit = Vendor.VendorTypes.Church;
                break;

            case 5:
                vendorToVisit = Vendor.VendorTypes.DarkTemple;
                break;

            case 6:
                vendorToVisit = Vendor.VendorTypes.Castle;
                break;

            default:
                vendorToVisit = Vendor.VendorTypes.GenderalStore;
                break;
        }

        //Looping through each of the vendors on this location
        CityLocation city = this.playerTileLocation as CityLocation;
        foreach(Vendor v in city.cityVendors)
        {
            //If the current vendor's type matches the one we're looking for, it's the one we display
            if(v.type == vendorToVisit)
            {
                vendorToDisplay = v;
                break;
            }
        }

        //If the city doesn't have that type of vendor, nothing happens (this SHOULDN'T happen, but just in case)
        if(vendorToDisplay == null)
        {
            return;
        }

        //Turning the vendor canvas object on
        this.vendorUICanvas.SetActive(true);

        Debug.Log("Vendor to open: " + vendorToDisplay.buildingName);
    }
}
