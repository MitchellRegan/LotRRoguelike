using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation : MonoBehaviour
{
    //The name of this location
    public string locationName = "";



    //Function that is overridden by inheriting classes
    public virtual void TravelToLocation()
    {
        //Content will be added in inheriting classes
    }
}
