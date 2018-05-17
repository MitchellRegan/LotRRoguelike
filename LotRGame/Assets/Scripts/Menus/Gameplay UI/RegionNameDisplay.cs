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
        //Setting our text to read the name of the region that our party is currently on
        this.ourText.text = this.partyMovement.currentTile.regionName;
	}
}
