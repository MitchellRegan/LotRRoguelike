using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityLocation : MapLocation
{
    //The different vendors that this city has
    public List<Vendor> cityVendors;



    //Function inherited from MapLocation.cs that lets the player 
    public override void TravelToLocation()
    {
    }
}

