using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RegionNameDisplay : MonoBehaviour
{
    //Reference to this object's Text component
    private Text ourText;
    //Reference to the party group's WASD Overworld Movement component
    private WASDOverworldMovement partyMovement;


	// Use this for initialization
	private void Start ()
    {
        //Setting the reference to our text component
        this.ourText = this.GetComponent<Text>();
        //Setting the reference to the party WASD movement component
        this.partyMovement = PartyGroup.group1.GetComponent<WASDOverworldMovement>();
	}
	

	// Update is called once per frame
	private void Update ()
    {
        //If the player party's tile decoration object is a map location, we read out the location name
        if (this.partyMovement.currentTile.decorationModel != null && this.partyMovement.currentTile.decorationModel.GetComponent<MapLocation>())
        {
            this.ourText.text = this.partyMovement.currentTile.decorationModel.GetComponent<MapLocation>().locationName;
        }
        //Otherwise we set our text to read the name of the region that our party is currently on
        else
        {
            this.ourText.text = this.partyMovement.currentTile.regionName;
        }
	}
}
