using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerkList : MonoBehaviour
{
    //The list of perks that this character has earned
    public List<Perk> allPerks;



    //Function called when this object is created
    private void Awake()
    {
        //Initializing our perks list if it hasn't already been initialized
        if (this.allPerks == null)
        {
            this.allPerks = new List<Perk>();
        }
    }
}
