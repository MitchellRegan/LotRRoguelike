using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityLocation : MapLocation
{
    //Function inherited from MapLocation.cs that lets the player 
    public override void TravelToLocation()
    {
        Debug.Log("Travel to " + this.locationName);
    }
}
