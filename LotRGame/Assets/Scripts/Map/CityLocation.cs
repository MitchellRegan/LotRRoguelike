using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityLocation : MapLocation
{
    //Function inherited from MapLocation.cs that lets the player 
    public override void TravelToLocation()
    {
        Debug.Log("Travel to " + this.locationName);
    }
}
