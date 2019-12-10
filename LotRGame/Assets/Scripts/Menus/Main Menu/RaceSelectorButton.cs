using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSelectorButton : MonoBehaviour
{
    //Function called externally from UI Buttons
    public void SetStartingRace(int raceIndex_)
    {
        GameData.globalReference.SetStartingRace(raceIndex_);
    }
}
